using System.Collections.Generic;
using Android.Support.V4.App;
using PeriwinkleApp.Android.Source.AdapterModels;
using PeriwinkleApp.Android.Source.Adapters;
using PeriwinkleApp.Android.Source.Presenters.ConsultantPresenters;
using PeriwinkleApp.Android.Source.Session;
using PeriwinkleApp.Android.Source.Views.Fragments.ClientFragments;
using PeriwinkleApp.Android.Source.Views.Fragments.Common;
using PeriwinkleApp.Core.Sources.Models.Domain;
using PeriwinkleApp.Core.Sources.Utils;

namespace PeriwinkleApp.Android.Source.Views.Fragments.ConsultantFragments
{
	public interface IConsultantClientProfileBodyView
	{
		void DisplayNotifications(List<NotificationAdapterModel> notificationDataSet);
		void StartViewClientBehaviorList();
		void StartViewClientWeeklyReport ();
		void StartViewClientSurveyReport ();
	}

    public class ConsultantClientProfileBodyView : RecyclerFragment<NotificationRecyclerAdapter, NotificationAdapterModel>,
												   IConsultantClientProfileBodyView
    {
        private IConsultantClientProfileBodyPresenter presenter;
        protected override void OnCreateInitialize ()
		{
			IsAnimated = true;
			presenter = new ConsultantClientProfileBodyPresenter (this);
		}

		protected override int ResourceLayout => Resource.Layout.list_frag_generic;
		protected override int? ResourceIdLinearLayout => Resource.Id.list_frag_gen_linear;
		protected override int? ResourceIdRecyclerView => Resource.Id.list_frag_gen_recyclerView;
		protected override int? ResourceIdProgressBar => Resource.Id.list_frag_gen_progress;

        protected override void LoadInitialDataSet ()
		{
			presenter.LoadNotifications ();
		}

		public void DisplayNotifications(List<NotificationAdapterModel> notificationDataSet)
		{
			UpdateAdapterDataSet(notificationDataSet);
			HideProgressBar();
		}

		public void StartViewClientBehaviorList ()
		{
			Logger.Log ("StartViewClientBehavior");
			FragmentTransaction ft = Activity.SupportFragmentManager.BeginTransaction();

			//Fragment headFragment = new ClientBehaviorView(SessionKeys.ViewClient);
			Fragment headFragment = new ClientViewRecordsListView(SessionKeys.ViewClient);
			ft.Replace(Resource.Id.fragment_container, headFragment);
			ft.AddToBackStack (null);
			ft.Commit ();
		}
		
		public void StartViewClientWeeklyReport ()
		{
			Logger.Log("StartViewClientWeeklyReport");
        }

		public void StartViewClientSurveyReport()
		{
			Logger.Log("StartViewClientSurveyReport");
		}
    }
}
