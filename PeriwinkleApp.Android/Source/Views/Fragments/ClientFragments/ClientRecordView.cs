using System;
using System.Collections.Generic;
using Android.Content;
using Android.OS;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;
using PeriwinkleApp.Android.Source.Views.Activities;
using PeriwinkleApp.Core.Sources.Utils;

namespace PeriwinkleApp.Android.Source.Views.Fragments.ClientFragments
{
	public class ClientRecordView : Fragment
	{
		private Button btnNewRecord, btnViewRecords, btnCalibrate;

		public override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			// Create your fragment here
		}

		public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			// Use this to return your custom view for this Fragment
			return inflater.Inflate(Resource.Layout.client_frag_record, container, false);
		}

		public override void OnViewCreated(View view, Bundle savedInstanceState)
		{
			base.OnViewCreated(view, savedInstanceState);

			btnNewRecord = view.FindViewById<Button>(Resource.Id.btn_new_record);
			btnViewRecords= view.FindViewById<Button>(Resource.Id.btn_view_record);
			btnCalibrate = view.FindViewById<Button>(Resource.Id.btn_calibrate);

			btnNewRecord.Click += OnNewRecordClicked;
			btnViewRecords.Click += OnViewRecordsClicked;
			btnCalibrate.Click += OnCalibrateClicked;
		}

		private void OnNewRecordClicked(object sender, EventArgs e)
		{
			Logger.Log("New Record");
			Intent intent = new Intent(Activity.ApplicationContext, typeof(ClientNewRecordActivity));
			StartActivity(intent);
		}

		private void OnViewRecordsClicked(object sender, EventArgs e)
		{
			Logger.Log("View Records");
			FragmentTransaction ft = Activity.SupportFragmentManager.BeginTransaction();
			Fragment fragment = new ClientViewRecordsListView();

			ft.Replace(Resource.Id.fragment_container, fragment);
			ft.AddToBackStack(null);
			ft.Commit();
		}

		private void OnCalibrateClicked(object sender, EventArgs e)
		{
			Logger.Log("Calibrate");
			Intent intent = new Intent(Activity.ApplicationContext, typeof(ClientCalibrateActivity));
			StartActivity(intent);
		}
	}
}