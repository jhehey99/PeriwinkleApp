using System;
using System.Collections.Generic;
using Android.Content;
using Android.Views;
using PeriwinkleApp.Android.Source.AdapterModels;
using PeriwinkleApp.Android.Source.Adapters;
using PeriwinkleApp.Android.Source.Presenters.ClientPresenters;
using PeriwinkleApp.Android.Source.Views.Activities;
using PeriwinkleApp.Android.Source.Views.Fragments.Common;
using PeriwinkleApp.Core.Sources.Utils;

namespace PeriwinkleApp.Android.Source.Views.Fragments.ClientFragments
{
	public interface IClientHomeBodyView
	{
//		void DisplayNotifications (List <ReminderAdapterModel> reminderDataSet);
		void DisplayNotifications (List <NotificationAdapterModel> notificationDataSet);

		void StartMbesActivity (object sender, EventArgs e);
	}

	public class ClientHomeBodyView : RecyclerFragment<NotificationRecyclerAdapter, NotificationAdapterModel>, 
									  IClientHomeBodyView
	{
		private IClientHomeBodyPresenter presenter;

        protected override void OnCreateInitialize ()
		{
			IsAnimated = true;
			presenter = new ClientHomeBodyPresenter(this);
//			await presenter.LoadLoggedClient ();
		}

        protected override int ResourceLayout => Resource.Layout.list_frag_generic;
		protected override int? ResourceIdLinearLayout => Resource.Id.list_frag_gen_linear;
		protected override int? ResourceIdRecyclerView => Resource.Id.list_frag_gen_recyclerView;
		protected override int? ResourceIdProgressBar => Resource.Id.list_frag_gen_progress;

		protected override void LoadInitialDataSet()
		{
            presenter.LoadNotifications ();
		}

//		protected override void SetViewReferences (View view)
//		{
//			base.SetViewReferences (view);
//			RfRecyclerAdapter.ActionClicked += (sender, args) => { Logger.Log ("Action Button Clicked"); };
//		}

#region IClientHomeBodyView

//        public void DisplayNotifications (List<ReminderAdapterModel> reminderDataSet)
//		{
//			UpdateAdapterDataSet (reminderDataSet);
//			HideProgressBar ();
//		}

		public void DisplayNotifications (List <NotificationAdapterModel> notificationDataSet)
		{
			UpdateAdapterDataSet(notificationDataSet);
			HideProgressBar();
		}

		public void StartMbesActivity (object sender, EventArgs e)
		{
			int CheckListRequestCode = 1;
			Intent intent = new Intent(Context, typeof(CheckListMainActivity));
			StartActivityForResult (intent, CheckListRequestCode);
			
			//TODO ON ACTIVITY RESULT, MAWAWALA NA DAPAT UNG REACTIVE NOTIFICATION NA UN KASI TAPOS NA
			// PARA DI MUKHANG EMPTY MAG DISPLAY TAYO MGA 3 NA REMINDER NOTIFICATIONS

			Logger.Log ("Start Mbes Activity");
		}

#endregion
    }
}
