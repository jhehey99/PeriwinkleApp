using System.Collections.Generic;
using System.Linq;
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
	public interface IClientJournalListPresenter
	{
		Task GetAllJournals ();
		void ViewJournalClicked (object sender, int position);
	}

    public class ClientJournalListPresenter : IClientJournalListPresenter
	{
		private readonly IClientJournalListView view;
		private readonly IClientService cliService;
		private ClientSession cliSession;
		private List <JournalEntry> journals;
		private Client loggedClient;

        public ClientJournalListPresenter (IClientJournalListView view)
		{
			this.view = view;
			cliService = cliService ?? new ClientService();
			LoadClientSession ();
		}

		private void LoadClientSession()
		{
			cliSession = SessionFactory.ReadSession<ClientSession>(SessionKeys.LoggedClient);

			loggedClient = CacheProvider.Get <Client> (CacheKey.LoggedClient);
		}

		#region IClientJournalListPresenter

        public async Task GetAllJournals ()
		{
            //todo client service get all journal by client id
            // pati sql
            //			journals = await cliService.jo
//			journals = await cliService.GetAllJournalsByClientId(cliSession.ClientId);
			journals = await cliService.GetAllJournalsByClientId(loggedClient.ClientId);

            // pag walang laman, avoid na magdisplay, TODO OR lagay na no items yet bla bla chu chu
            if (journals == null)
				return;

			List <JournalAdapterModel> dataSet =
				journals.Select ((t, i) => new JournalAdapterModel ()
										   {
											   Title = t.Title,
											   DateCreated = t.DateTimeCreated
										   }).ToList ();

			foreach (JournalAdapterModel model in dataSet)
				model.ViewJournalClicked += ViewJournalClicked;

			view.DisplayJournals (dataSet);
		}

		public void ViewJournalClicked (object sender, int position)
		{
			Logger.Log($"ViewReportClicked - {position}");

			JournalEntry journal = journals[position];
			view.LaunchViewJournal (journal);
		}

#endregion

    }
}
