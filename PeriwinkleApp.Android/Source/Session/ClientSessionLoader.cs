using System.Threading.Tasks;
using PeriwinkleApp.Android.Source.Factories;
using PeriwinkleApp.Core.Sources.Models.Domain;
using PeriwinkleApp.Core.Sources.Services;
using PeriwinkleApp.Core.Sources.Services.Interfaces;

namespace PeriwinkleApp.Android.Source.Session
{
	public class ClientSessionLoader
	{
		private readonly IClientService cliService;
		public Client LoadedClient { get; protected set; }

		public ClientSessionLoader (IClientService cliService = null)
		{
			this.cliService = cliService ?? new ClientService ();
		}

		public async Task<bool> LoadClientSession()
		{
			AccountSession session = SessionFactory.ReadSession<AccountSession>(SessionKeys.LoginKey);

			if (session.AccountType != AccountType.Client)
				return false;

			// account is client, so get its info
            LoadedClient = await cliService.GetClientByUsername(session.Username);

            // add it to client session
            ClientSession cliSession =
				SessionFactory.CreateSession<ClientSession>(SessionKeys.LoggedClient);

			cliSession.AddClientSession(LoadedClient);

			return LoadedClient != null;
		}
    }
}
