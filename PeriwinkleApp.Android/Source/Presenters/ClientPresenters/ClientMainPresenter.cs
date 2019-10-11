using System.Threading;
using System.Threading.Tasks;
using Android.Util;
using PeriwinkleApp.Android.Source.Cache;
using PeriwinkleApp.Android.Source.Factories;
using PeriwinkleApp.Android.Source.Session;
using PeriwinkleApp.Android.Source.Views.Activities;
using PeriwinkleApp.Core.Sources.Models.Domain;
using PeriwinkleApp.Core.Sources.Services;
using PeriwinkleApp.Core.Sources.Services.Interfaces;

namespace PeriwinkleApp.Android.Source.Presenters.ClientPresenters
{
    public interface IClientMainPresenter
	{
		Task LoadSession ();
        void LoadNavHeaderDetails ();
		void LoadLoggedClient ();
	}

    public class ClientMainPresenter : IClientMainPresenter
    {
        private readonly IClientMainActivity activity;
        private readonly IClientService cliService;

        private Client client;

        public ClientMainPresenter (IClientMainActivity activity)
        {
            this.activity = activity;
            cliService = cliService ?? new ClientService ();
        }

		public async Task LoadSession ()
		{
			// check kung may session na ung logged client.
			// pag wala, i-load natin then saka idisplay ung nav head details
			ClientSession cliSession = SessionFactory.ReadSession<ClientSession>(SessionKeys.LoggedClient);

			// may client session na, so kunin nalang natin ung info nya.
            if (cliSession != null && cliSession.IsSet)
				client = await cliService.GetClientByUsername(cliSession.Username);

            // i-load muna ung client session from account session
            ClientSessionLoader cliLoader = new ClientSessionLoader(cliService);
			bool isLoaded = await cliLoader.LoadClientSession();

			if (isLoaded)
			{
				client = cliLoader.LoadedClient;
				CacheProvider.Set<Client> (CacheKey.LoggedClient, client);
			}
        }
		
        public void LoadNavHeaderDetails ()
        {
            string name = $"{client?.FirstName} {client?.LastName}";
            activity.SetNavHeaderDetails(name, client?.Email);
        }

		public void LoadLoggedClient ()
		{
			//start progress loading
			AccountSession session = SessionFactory.ReadSession<AccountSession>(SessionKeys.LoginKey);

			if (session.AccountType != AccountType.Client)
				return;
			
			// account is client, so get its info
			Task <Client> task = Task.Run (() => cliService.GetClientByUsername (session.Username));
			Client loadedClient = task.Result;

            // add it to client session
            ClientSession cliSession =
				SessionFactory.CreateSession<ClientSession>(SessionKeys.LoggedClient);

			cliSession.AddClientSession(loadedClient);

			// add to cache
			CacheProvider.Set (CacheKey.LoggedClient, loadedClient);


			//end loading
		}
	}
}
