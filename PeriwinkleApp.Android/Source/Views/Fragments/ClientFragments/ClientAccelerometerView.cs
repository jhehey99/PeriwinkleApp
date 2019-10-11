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
	public class ClientAccelerometerView : Fragment
	{
		private Button btnCalibrate, btnRecordAccelerometer;

		private void onCalibrateClicked(object sender, EventArgs e)
		{
			Logger.Log("onCalibrateClicked");
			Intent intent = new Intent(Context, typeof(ClientCalibrateActivity));
			StartActivity(intent);
		}

		private void onRecordAccelerometerClicked(object sender, EventArgs e)
		{
			Logger.Log("onRecordAccelerometerClicked");
			//TODO DAPAT ACTIVITY DIN I-START NETO KASI MAY BLUETOOTH
			// THEN ETONG VIEW NA TO, RECYCLERVIEW NG MGA PREVIOUS DATA 
			/*
			FragmentTransaction ft = Activity.SupportFragmentManager.BeginTransaction();
			Fragment fragment = new ClientCalibrateView();

			ft.Replace(Resource.Id.fragment_container, fragment);
			ft.AddToBackStack(null);
			ft.Commit();
			*/
		}

		public override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			// Create your fragment here
		}

		public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			// Use this to return your custom view for this Fragment
			return inflater.Inflate(Resource.Layout.client_frag_accelerometer, container, false);
		}

		public override void OnViewCreated(View view, Bundle savedInstanceState)
		{
			base.OnViewCreated(view, savedInstanceState);

			btnCalibrate = view.FindViewById<Button>(Resource.Id.btn_calibrate);
			btnRecordAccelerometer = view.FindViewById<Button>(Resource.Id.btn_record_accelerometer);

			btnCalibrate.Click += onCalibrateClicked;
			btnRecordAccelerometer.Click += onRecordAccelerometerClicked;
		}

	}
}