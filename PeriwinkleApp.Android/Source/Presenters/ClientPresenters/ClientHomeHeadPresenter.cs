using System.Threading.Tasks;
using PeriwinkleApp.Android.Source.Cache;
using PeriwinkleApp.Android.Source.Factories;
using PeriwinkleApp.Android.Source.Session;
using PeriwinkleApp.Android.Source.Views.Fragments.ClientFragments;
using PeriwinkleApp.Core.Sources.Models.Domain;
using PeriwinkleApp.Core.Sources.Services;
using PeriwinkleApp.Core.Sources.Services.Interfaces;

namespace PeriwinkleApp.Android.Source.Presenters.ClientPresenters
{
    public interface IClientHomeHeadPresenter
    {
		Task LoadLoggedClient();
		void LoadHeadInfo ();
    }

    public class ClientHomeHeadPresenter : IClientHomeHeadPresenter
    {
        private readonly IClientHomeHeadView view;
        private readonly IClientService cliService;

		private Client client;

        public ClientHomeHeadPresenter (IClientHomeHeadView view)
        {
            this.view = view;
            cliService = cliService ?? new ClientService ();
        }

		public async Task LoadLoggedClient()
		{
			// check kung may session na ung logged client.
			// pag wala, i-load natin then saka idisplay ung nav head details

			if (CacheProvider.IsSet (CacheKey.LoggedClient))
			{
				client = CacheProvider.Get <Client> (CacheKey.LoggedClient);
				return;
			}

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
				CacheProvider.Set (CacheKey.LoggedClient, client);
			}
		}

        public void LoadHeadInfo ()
        {
            // build ung string
            string name = $"{client?.FirstName} {client?.LastName}";

            // display na ung info
            view.DisplayHeadInfo (name, client?.Username);
        }
    }
}
