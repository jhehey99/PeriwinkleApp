using System.Threading.Tasks;
using PeriwinkleApp.Android.Source.Views.Fragments.ClientFragments;

namespace PeriwinkleApp.Android.Source.Presenters.ClientPresenters
{
	public interface IClientViewJournalPresenter
	{
		Task LoadJournal ();
	}

    public class ClientViewJournalPresenter : IClientViewJournalPresenter
	{
		private readonly IClientViewJournalView view;

		public ClientViewJournalPresenter (IClientViewJournalView view)
		{
			this.view = view;
		}

		public Task LoadJournal ()
		{
			throw new System.NotImplementedException ();
		}
	}
}
