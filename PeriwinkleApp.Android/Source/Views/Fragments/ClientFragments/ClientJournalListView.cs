using System;
using System.Collections.Generic;
using Android.Content;
using Android.Support.V4.App;
using PeriwinkleApp.Android.Source.AdapterModels;
using PeriwinkleApp.Android.Source.Adapters;
using PeriwinkleApp.Android.Source.Presenters.ClientPresenters;
using PeriwinkleApp.Android.Source.Views.Activities;
using PeriwinkleApp.Android.Source.Views.Fragments.Common;
using PeriwinkleApp.Core.Sources.Models.Domain;
using PeriwinkleApp.Core.Sources.Utils;

namespace PeriwinkleApp.Android.Source.Views.Fragments.ClientFragments
{
	public interface IClientJournalListView
	{
		void DisplayJournals (List <JournalAdapterModel> journalDataSet);
		void LaunchViewJournal (JournalEntry journal);
	}

    public class ClientJournalListView : RecyclerFragment <JournalRecyclerAdapter, JournalAdapterModel>,
										 IClientJournalListView
    {
		//TODO PRESENTER
		private IClientJournalListPresenter presenter;

        protected override void OnCreateInitialize ()
		{
			IsAnimated = true;
			presenter = new ClientJournalListPresenter (this);
        }

        protected override int ResourceLayout => Resource.Layout.list_frag_generic;
		protected override int? ResourceIdProgressBar => Resource.Id.list_frag_gen_progress;
		protected override int? ResourceIdLinearLayout => Resource.Id.list_frag_gen_linear;
		protected override int? ResourceIdRecyclerView => Resource.Id.list_frag_gen_recyclerView;
		protected override int? ResourceIdFab => Resource.Id.list_frag_gen_fab;

        protected override async void LoadInitialDataSet ()
		{
			//TODO PRESENTER LOAD DATA
			// GET JOURNALS FROM THE DATABASE
			await presenter.GetAllJournals ();
		}

		protected override void OnItemClick (object sender, int position)
		{
			base.OnItemClick (sender, position);
			presenter.ViewJournalClicked (sender, position);
		}

        protected override void OnFloatingActionButtonClicked (object sender, EventArgs e)
		{
			//TODO Dito ung pag create ng new journal
			base.OnFloatingActionButtonClicked (sender, e);
			Logger.Log("OnFloatingActionButtonClicked");

			Intent intent = new Intent(Context, typeof(ClientJournalCreateActivity));
			Activity.StartActivityForResult (intent, 1001);
        }

		#region IClientJournalListView

        public void DisplayJournals (List <JournalAdapterModel> journalDataSet)
		{
			UpdateAdapterDataSet(journalDataSet);
			HideProgressBar();
		}

		public void LaunchViewJournal (JournalEntry journal)
		{
			Logger.Log ("LaunchViewJournal");
			//TODO Dito ung Pag view nung journal
			FragmentTransaction ft = Activity.SupportFragmentManager.BeginTransaction();
			Fragment fragment = new ClientViewJournalView(journal);

			ft.Replace(Resource.Id.fragment_container, fragment);
			ft.AddToBackStack(null);
			ft.Commit();
		}

		#endregion

    }

}
