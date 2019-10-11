using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PeriwinkleApp.Android.Source.AdapterModels;
using PeriwinkleApp.Android.Source.Cache;
using PeriwinkleApp.Android.Source.Factories;
using PeriwinkleApp.Android.Source.Session;
using PeriwinkleApp.Android.Source.Views.Fragments.ClientFragments;
using PeriwinkleApp.Core.Sources.Models.Domain;
using PeriwinkleApp.Core.Sources.Services;
using PeriwinkleApp.Core.Sources.Services.Interfaces;
using PeriwinkleApp.Core.Sources.Utils;

namespace PeriwinkleApp.Android.Source.Presenters.ClientPresenters
{
	public interface IClientHomeBodyPresenter
	{
		Task LoadLoggedClient ();
		void LoadNotifications ();
	}
	public class ClientHomeBodyPresenter : IClientHomeBodyPresenter
	{
		private readonly IClientHomeBodyView view;

        //TODO SERVICE????
        private readonly IClientService cliService;
		private Client loggedClient;
		private const int NotificationCount = 3;
		private Random random;
		private ClientSession cliSession;

        public ClientHomeBodyPresenter (IClientHomeBodyView view)
		{
			this.view = view;
			cliService = cliService ?? new ClientService ();
			random = new Random ();

			// dapat na-set na sa bandang part na to
//			if (!CacheProvider.IsSet (CacheKey.LoggedClient))
//				throw new Exception ("CacheProvider - LoggedClient not set");
//			
			loggedClient = CacheProvider.Get<Client>(CacheKey.LoggedClient);
        }

        #region IClientHomeBodyPresenter

		public async Task LoadLoggedClient()
		{
			// check kung may session na ung logged client.
			// pag wala, i-load natin then saka idisplay ung nav head details

			if (CacheProvider.IsSet(CacheKey.LoggedClient))
			{
				loggedClient = CacheProvider.Get<Client>(CacheKey.LoggedClient);
				return;
			}

			cliSession = SessionFactory.ReadSession<ClientSession>(SessionKeys.LoggedClient);

			// may client session na, so kunin nalang natin ung info nya.
			if (cliSession != null && cliSession.IsSet)
				loggedClient = await cliService.GetClientByUsername(cliSession.Username);

			// i-load muna ung client session from account session
			ClientSessionLoader cliLoader = new ClientSessionLoader(cliService);
			bool isLoaded = await cliLoader.LoadClientSession();

			if (isLoaded)
			{
				loggedClient = cliLoader.LoadedClient;
				CacheProvider.Set(CacheKey.LoggedClient, loggedClient);
			}
		}

        public void LoadNotifications ()
		{
			Logger.Log ("LoadNotifications");

            // build ung adapter model objects

            //			List<string> messages = new List <string> ()
            //									{
            //										"hehehe1","hehehe2",
            //										"hehehe3","hehehe4",
            //										"hehehe5","hehehe6",
            //                                    };
            //			
            //			List <ReactiveAdapterModel> dataSet =
            //				messages.Select ((m, i) => new ReactiveAdapterModel() {Title = $"title {i + 1}", Message = m}).ToList ();

            // bool allowed = cliService.AllowMbesAttempt(cliSession.clientId);
            // if(allowed) create model

            List <NotificationAdapterModel> dataSet = new List <NotificationAdapterModel> (NotificationCount);
			
			// client is allowed to take the mbes so we add it first
			if (loggedClient.MbesAllowAttempt)
			{
				NotificationAdapterModel model = new NotificationAdapterModel()
												 {
													 Title = "Take the MBES-21",
													 Message = "You are required to take the MBES-21 and answer as honestly as possible.",
													 HasAction = true
												 };
				model.ActionClicked += StartMbesActivity;
				dataSet.Add (model);
            }

            // add random reminders to the dataset
            // 3 notifs, either 0 - mbes | 3 - reminders, or 1 - mbes | 2 - reminders

			List<NotificationAdapterModel> reminders = GetReminders;
			int count = reminders.Count;
			int randInt = random.Next(count);

            List<int> randIndeces = new List <int> ();
            for (int i = dataSet.Count; i < NotificationCount; i++)
			{
				// consecutive items makukuha, last<-->first connected
				int index = (randInt + i) % count;
				dataSet.Add (reminders[index]);
			}
            
			

//			List <NotificationAdapterModel> dataSets = new List <NotificationAdapterModel> () {model};//, model, model, model, model, model, model, model, model, model, model, model, model};
            view.DisplayNotifications (dataSet);
		}

		private void StartMbesActivity (object sender, EventArgs e)
		{
			view.StartMbesActivity (sender, e);
		}

		private static List <NotificationAdapterModel> GetReminders =>
			new List <NotificationAdapterModel> ()
			{
				new NotificationAdapterModel ()
				{
					Title = "Self-Control",
					Message = "Refrain from consuming large amounts of food, even when you're not hungry."
				},
				new NotificationAdapterModel ()
				{
					Title = "Believe In Yourself!",
					Message = "You can get through this"
                },
				new NotificationAdapterModel ()
				{
					Title = "Recovery",
					Message = "Small victories that make the eating disorder wish it never touched your life."
                },
				new NotificationAdapterModel ()
				{
					Title = "You Are Allowed To Eat",
					Message = "Feeling guilty for eating when you're hungry is like feeling guilty for breathing when your lungs need oxygen."
                },
				new NotificationAdapterModel ()
				{
					Title = "Confidence",
					Message = "To lose confidence in one's body is to lose confidence in oneself"
                },
				new NotificationAdapterModel ()
				{
					Title = "Anorexia Nervosa",
					Message = "An eating disorder characterized by low weight, fear of gaining weight, and a strong desire to be thin."
                },
				new NotificationAdapterModel ()
				{
					Title = "Bulimia Nervosa",
					Message = "An eating disorder characterized by a cycle of binge eating and purging by vomiting, using laxatives and etc. to compensate for overeating."
                },
				new NotificationAdapterModel ()
				{
					Title = "Binge Eating",
					Message = "An eating disorder characterized by uncontrollable consumption of large amounts of food in a short period of time."
                },
				new NotificationAdapterModel ()
				{
					Title = "Memories",
					Message = "Count the memories, NOT the calories."
                },
				new NotificationAdapterModel ()
				{
					Title = "High Mortality Rate",
					Message = "Eating disorders have the highest mortality rate of any mental illness."
                },
				new NotificationAdapterModel ()
				{
					Title = "Anyone Can Be Affected",
					Message = "Eating disorders affect all races and ethnic groups."
                },
				new NotificationAdapterModel ()
				{
					Title = "Everything Matters",
					Message = "Genetics, environmental factors, and personality traits all combine to create risk for an eating disorder."
                },
				new NotificationAdapterModel ()
				{
					Title = "Do Not Worry",
					Message = "Eating disorder is treatable, the sooner the treatment, the better."
                },
				new NotificationAdapterModel ()
				{
					Title = "Willingness Is Essential",
					Message = "You don't have to be ready to recover, you need only to be willing."
                },
            };
		
#endregion
    }
}
