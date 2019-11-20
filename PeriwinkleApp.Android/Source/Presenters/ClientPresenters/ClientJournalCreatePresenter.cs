using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PeriwinkleApp.Android.Source.Cache;
using PeriwinkleApp.Android.Source.Factories;
using PeriwinkleApp.Android.Source.Session;
using PeriwinkleApp.Android.Source.Views.Activities;
using PeriwinkleApp.Core.Sources.Extensions;
using PeriwinkleApp.Core.Sources.Models.Domain;
using PeriwinkleApp.Core.Sources.Models.Response;
using PeriwinkleApp.Core.Sources.Services;
using PeriwinkleApp.Core.Sources.Services.Interfaces;
using PeriwinkleApp.Core.Sources.Utils;

namespace PeriwinkleApp.Android.Source.Presenters.ClientPresenters
{
	public interface IClientJournalCreatePresenter
	{
		Task AddJournalEntry (JournalEntry journal);
	}

    public class ClientJournalCreatePresenter : IClientJournalCreatePresenter
	{
		private readonly IClientJournalCreateActivity view;
		private IClientService cliService;
//		private ClientSession cliSession;
		private Client client;

		public ClientJournalCreatePresenter (IClientJournalCreateActivity view)
		{
            this.view = view;
			cliService = cliService ?? new ClientService ();
			LoadLoggedClient ();
		}
		
		private void LoadLoggedClient ()
		{
//			cliSession = SessionFactory.ReadSession <ClientSession> (SessionKeys.LoggedClient);
			client = CacheProvider.Get <Client> (CacheKey.LoggedClient);
		}

#region IClientJournalCreatePresenter

        public async Task AddJournalEntry (JournalEntry journal)
		{
			journal.JournalClientId = client.ClientId;

			List <ApiResponse> response = await cliService.AddJournalEntry (journal);
			Logger.Debug (response.FirstOrDefault());
			view.BackToJournalListView ();
		}

#endregion

    }
}
