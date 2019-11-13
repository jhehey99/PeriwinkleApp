using System;
using System.Collections.Generic;
using Android.App;
using Android.Bluetooth;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Widget;
using Microcharts;
using Microcharts.Droid;
using PeriwinkleApp.Android.Source.Presenters.ClientPresenters;
using PeriwinkleApp.Android.Source.Services.Bluetooth;
using PeriwinkleApp.Core.Sources.Utils;
using v7App = Android.Support.V7.App;

namespace PeriwinkleApp.Android.Source.Views.Activities
{
	[Activity(Label = "ClientCreateRecordActivity")]
	public class ClientNewRecordActivity : Activity
	{
		// TODO: gayahin mo to ClientBehaviorActivity

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			// Create your application here
		}
	}
}