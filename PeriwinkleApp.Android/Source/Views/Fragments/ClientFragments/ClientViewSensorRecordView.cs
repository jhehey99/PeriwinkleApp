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
	public interface IClientViewSensorRecordView
	{
		void DisplayPiezoLineChart(LineChart linePiezo);
		void DisplayAccelerationLineChart(LineChart lineAx, LineChart lineAy, LineChart lineAz);
		void DisplayRecordStatistics(RecordStatistics stats);
	}
	public class ClientViewSensorRecordView : Fragment, IClientViewSensorRecordView
	{
		//TODO: gayahin mo to ClientViewJournalView
		private readonly IClientViewSensorRecordPresenter presenter;

		private ChartView chartPiezo, chartAx, chartAy, chartAz;
		private LineChart linePiezo, lineAx, lineAy, lineAz;
		private SeekBar seekPiezo, seekAccel;
		private bool isSeekingPiezo = false, isSeekingAccel = false;

		// Statistics
		private TextView txtStartTime, txtStopTime, txtDuration,
						 txtPiezoMax, txtPiezoMin, txtPiezoAverage,
						 txtAxMax, txtAxMin, txtAxAverage,
						 txtAyMax, txtAyMin, txtAyAverage,
						 txtAzMax, txtAzMin, txtAzAverage;

		public ClientViewSensorRecordView(SensorRecord record)
		{
			presenter = new ClientViewSensorRecordPresenter(this, record);
		}
	
		public override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
		}

		public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			return inflater.Inflate(Resource.Layout.client_frag_record_view, container, false);
		}

		public override async void OnViewCreated(View view, Bundle savedInstanceState)
		{
			base.OnViewCreated(view, savedInstanceState);

			// set reference sa chartview
			chartPiezo = view.FindViewById<ChartView>(Resource.Id.chart_piezo);
			chartAx = view.FindViewById<ChartView>(Resource.Id.chart_ax);
			chartAy = view.FindViewById<ChartView>(Resource.Id.chart_ay);
			chartAz = view.FindViewById<ChartView>(Resource.Id.chart_az);

			// seekbar
			seekPiezo = view.FindViewById<SeekBar>(Resource.Id.seek_bar_piezo);
			seekPiezo.StartTrackingTouch += OnSeekPiezoStart;
			seekPiezo.ProgressChanged += OnSeekPiezoProgressChanged;
			seekAccel = view.FindViewById<SeekBar>(Resource.Id.seek_bar_accel);
			seekAccel.StartTrackingTouch += OnSeekAccelStart;
			seekAccel.ProgressChanged += OnSeekAccelProgressChanged;

			// Statistics
			txtStartTime = view.FindViewById<TextView>(Resource.Id.txt_start_time_val);
			txtStopTime = view.FindViewById<TextView>(Resource.Id.txt_stop_time_val);
			txtDuration = view.FindViewById<TextView>(Resource.Id.txt_duration_val);
			txtPiezoMax = view.FindViewById<TextView>(Resource.Id.txt_piezomax_val);
			txtPiezoMin = view.FindViewById<TextView>(Resource.Id.txt_piezomin_val);
			txtPiezoAverage = view.FindViewById<TextView>(Resource.Id.txt_piezoave_val);
			txtAxMax = view.FindViewById<TextView>(Resource.Id.txt_axmax_val);
			txtAxMin = view.FindViewById<TextView>(Resource.Id.txt_axmin_val);
			txtAxAverage = view.FindViewById<TextView>(Resource.Id.txt_axave_val);
			txtAyMax = view.FindViewById<TextView>(Resource.Id.txt_aymax_val);
			txtAyMin = view.FindViewById<TextView>(Resource.Id.txt_aymin_val);
			txtAyAverage = view.FindViewById<TextView>(Resource.Id.txt_ayave_val);
			txtAzMax = view.FindViewById<TextView>(Resource.Id.txt_azmax_val);
			txtAzMin = view.FindViewById<TextView>(Resource.Id.txt_azmin_val);
			txtAzAverage = view.FindViewById<TextView>(Resource.Id.txt_azave_val);

			// Load
			await presenter.LoadInitialLineChartData();
			seekPiezo.Max = presenter.EntryCountPiezo;
			seekAccel.Max = presenter.EntryCountAcceleration;
			presenter.LoadChartStatistics();
		}

		private void OnSeekPiezoStart(object sender, SeekBar.StartTrackingTouchEventArgs e)
		{
			isSeekingPiezo = true;
		}

		private void OnSeekPiezoProgressChanged(object sender, SeekBar.ProgressChangedEventArgs e)
		{
			if (!isSeekingPiezo)
				return;

			int progress = e.Progress;
			presenter.MovePiezoTo(progress);
		}

		private void OnSeekAccelStart(object sender, SeekBar.StartTrackingTouchEventArgs e)
		{
			isSeekingAccel = true;
		}

		private void OnSeekAccelProgressChanged(object sender, SeekBar.ProgressChangedEventArgs e)
		{
			if (!isSeekingAccel)
				return;

			int progress = e.Progress;
			presenter.MoveAccelerationTo(progress);
		}

		public void DisplayPiezoLineChart(LineChart linePiezo)
		{
			this.linePiezo = linePiezo;
			Activity.RunOnUiThread(UpdateChartPiezo);
		}

		public void DisplayAccelerationLineChart(LineChart lineAx, LineChart lineAy, LineChart lineAz)
		{
			this.lineAx = lineAx;
			this.lineAy = lineAy;
			this.lineAz = lineAz;
			Activity.RunOnUiThread(UpdateChartAcceleration);
		}

		private void UpdateChartPiezo()
		{
			chartPiezo.Chart = linePiezo;
		}
		
		private void UpdateChartAcceleration()
		{
			chartAx.Chart = lineAx;
			chartAy.Chart = lineAy;
			chartAz.Chart = lineAz;
		}

		public void DisplayRecordStatistics(RecordStatistics stats)
		{
			txtStartTime.Text = stats.StartTime;
			txtStopTime.Text = stats.StopTime;
			txtDuration.Text = stats.Duration;
			txtPiezoMax.Text = stats.PiezoMax;
			txtPiezoMin.Text = stats.PiezoMin;
			txtPiezoAverage.Text = stats.PiezoAverage;
			txtAxMax.Text = stats.AxMax;
			txtAxMin.Text = stats.AxMin;
			txtAxAverage.Text = stats.AxAverage;
			txtAyMax.Text = stats.AyMax;
			txtAyMin.Text = stats.AyMin;
			txtAyAverage.Text = stats.AyAverage;
			txtAzMax.Text = stats.AzMax;
			txtAzMin.Text = stats.AzMin;
			txtAzAverage.Text = stats.AzAverage;
		}
	}
}