using System;
using Android.OS;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;
using Microcharts;
using Microcharts.Droid;
using PeriwinkleApp.Android.Source.Presenters.ClientPresenters;
using PeriwinkleApp.Core.Sources.Models.Common;
using PeriwinkleApp.Core.Sources.Models.Domain;
using PeriwinkleApp.Core.Sources.Utils;

namespace PeriwinkleApp.Android.Source.Views.Fragments.ClientFragments
{
	public class ClientViewSensorRecordView : Fragment
	{
		//TODO: gayahin mo to ClientViewJournalView
		private readonly SensorRecord record;

		public ClientViewSensorRecordView(SensorRecord record)
		{
			this.record = record;
		}

		public override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			// Create your fragment here
		}

		public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			// Use this to return your custom view for this Fragment
			// return inflater.Inflate(Resource.Layout.YourFragment, container, false);

			return base.OnCreateView(inflater, container, savedInstanceState);
		}
	}
}