using System.Threading.Tasks;
using PeriwinkleApp.Android.Source.Cache;
using PeriwinkleApp.Android.Source.Factories;
using PeriwinkleApp.Android.Source.Session;
using PeriwinkleApp.Core.Sources.Models.Domain;
using PeriwinkleApp.Core.Sources.Services;
using PeriwinkleApp.Core.Sources.Services.Interfaces;

namespace PeriwinkleApp.Android.Source.Presenters.ClientPresenters
{
	public interface IClientHomePresenter
	{
		Task<bool> LoadLoggedClient ();
	}

    public class ClientHomePresenter : IClientHomePresenter
	{
		private readonly IClientService cliService;
		private Client client;
		public ClientHomePresenter ()
		{
			cliService = cliService ?? new ClientService ();
		}

		public async Task<bool> LoadLoggedClient ()
		{
			ClientSession cliSession = SessionFactory.ReadSession<ClientSession>(SessionKeys.LoggedClient);

			// may client session na, so kunin nalang natin ung info nya.
			if (cliSession != null && cliSession.IsSet)
				client = await cliService.GetClientByUsername(cliSession.Username);

			// i-load muna ung client session from account session
			ClientSessionLoader cliLoader = new ClientSessionLoader(cliService);
			bool isLoaded = await cliLoader.LoadClientSession();

			if (!isLoaded)
				return false;
			
			client = cliLoader.LoadedClient;
			CacheProvider.Set(CacheKey.LoggedClient, client);
			return true;
		}
	}
}
