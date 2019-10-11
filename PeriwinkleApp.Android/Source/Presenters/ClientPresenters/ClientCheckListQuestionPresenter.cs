using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Android.Content;
using PeriwinkleApp.Android.Source.AdapterModels;
using PeriwinkleApp.Android.Source.Cache;
using PeriwinkleApp.Android.Source.Factories;
using PeriwinkleApp.Android.Source.Services;
using PeriwinkleApp.Android.Source.Session;
using PeriwinkleApp.Android.Source.Views.Fragments.ClientFragments;
using PeriwinkleApp.Core.Sources.Models.Domain;
using PeriwinkleApp.Core.Sources.Services;
using PeriwinkleApp.Core.Sources.Services.Interfaces;
using PeriwinkleApp.Core.Sources.Utils;

namespace PeriwinkleApp.Android.Source.Presenters.ClientPresenters
{
    public interface IClientCheckListQuestionPresenter
    {
        void LoadQuestions ();
		Task AddClientMbesResponse (List <int> scaleValueList);
	}

    public class ClientCheckListQuestionPresenter : IClientCheckListQuestionPresenter
    {
        private readonly IClientCheckListQuestionView view;
        private readonly IMbesAssetService mbesAssetService;
		private readonly IClientService cliService;
		private readonly IMbesService mbesService;
		private Client client;

		public ClientCheckListQuestionPresenter (IClientCheckListQuestionView view, Context context)
        {
            this.view = view;
			mbesAssetService = mbesAssetService ?? new MbesAssetService(context);
			cliService = cliService ?? new ClientService();
			mbesService = mbesService ?? new MbesService();
        }

#region IClientCheckListQuestionPresenter

        public void LoadQuestions ()
        {
            List <string> questions = mbesAssetService.GetQuestions ().ToList ();

            List <QuestionAdapterModel> dataSet =
                questions.Select ((t, i) => new QuestionAdapterModel () {Id = i + 1, Question = t}).ToList ();
            
            view.DisplayQuestions (dataSet);
        }

		public async Task AddClientMbesResponse (List<int> scaleValueList)
		{
			if (!CacheProvider.IsSet (CacheKey.LoggedClient))
			{
				// get client info given session username
				ClientSession cliSession = SessionFactory.ReadSession <ClientSession> (SessionKeys.LoggedClient);

				//TOdo show error, session has been lost login again
				if (cliSession == null || !cliSession.IsSet)
					return;

				client = await cliService.GetClientByUsername (cliSession.Username);
			}
			else
			{
				client = CacheProvider.Get <Client> (CacheKey.LoggedClient);
			}

			// build the mbes response
			int clientId = client.ClientId.GetValueOrDefault();
			int attemptId = ++client.MbesAttemptCount;
			client.MbesAllowAttempt = false;
            List<int> qids = Enumerable.Range(0, 21).ToList();

			// cm -> m
			float mheight = client.Height.Value / 100;

			// bmi = kg/m^2
			float bmi = client.Weight.Value / (mheight * mheight);

			//TODO UNG BMI
			Mbes mbes = new Mbes()
			{
				MbesClientId = clientId,
				Height = client.Height,
				Weight = client.Weight,
				BMI = bmi,
				DateCreated = DateTime.Now.Date
			};

			// save muna tong mbes
			// then get mbes id
			// then saka isend ung mbes response
			int mbesId = await mbesService.AddMbes(mbes);

			Logger.Log("MBES ID: " + mbesId);
			if (mbesId == -1)
				return;
			
			MbesResponse mbesResponse = new MbesResponse ()
										{
											MbesResponseId = mbesId,
											AttemptId = attemptId,
											QuestionIds = qids,
											ScaleValues = scaleValueList
										};

			// add an attempt count
			var countResponses = await cliService.AddMbesAttemptCount(client);
			
			// add the response to db
            var mbesResponses = await mbesService.AddClientMbesResponse (mbesResponse);

			CacheProvider.Set (CacheKey.LoggedClient, client);
			
			//TODO CHECK IF RESPONSE IS SUCCESSFUL
		}
		
#endregion

    }
}
