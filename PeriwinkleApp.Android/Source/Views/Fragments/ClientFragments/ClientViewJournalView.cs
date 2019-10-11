using Android.OS;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;
using PeriwinkleApp.Android.Source.Presenters.ClientPresenters;
using PeriwinkleApp.Core.Sources.Models.Domain;

namespace PeriwinkleApp.Android.Source.Views.Fragments.ClientFragments
{
	public interface IClientViewJournalView
	{

	}

	public class ClientViewJournalView : Fragment, IClientViewJournalView
    {
		private TextView txtTitle, txtBody;
		private ImageView imgPicture;

		private IClientViewJournalPresenter presenter;

		private readonly JournalEntry journal;

		public ClientViewJournalView (JournalEntry journal)
		{
			this.journal = journal;
		}

        public override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);
			presenter = new ClientViewJournalPresenter(this);
			
        }

		public override View OnCreateView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			return inflater.Inflate (Resource.Layout.client_frag_journal_view, container, false);
		}

		public override void OnViewCreated (View view, Bundle savedInstanceState)
		{
			base.OnViewCreated (view, savedInstanceState);

			// Text Views
			txtTitle = view.FindViewById <TextView> (Resource.Id.txt_view_journal_title);
			txtBody = view.FindViewById <TextView> (Resource.Id.txt_view_journal_body);

			// Image View
			imgPicture = view.FindViewById <ImageView> (Resource.Id.img_view_journal);

			// load the journal
			if (journal == null)
				return;

			txtTitle.Text = journal.Title;
			txtBody.Text = journal.Body;

			// TODO UNG IMAGE
		}

	}
}
