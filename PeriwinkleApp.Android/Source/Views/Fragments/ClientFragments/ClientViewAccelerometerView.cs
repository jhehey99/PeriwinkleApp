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
	public interface IClientViewAccelerometerView
	{
		//void DisplayChartStatistics(ChartStat stats);
		void DisplayLineChartAx(LineChart lineAx);
		void DisplayLineChartAy(LineChart lineAy);
		void DisplayLineChartAz(LineChart lineAz);
	}
	public class ClientViewAccelerometerView : Fragment,
												IClientViewAccelerometerView
	{
		private SeekBar seekAx, seekAy, seekAz;
		private LineChart lineAx, lineAy, lineAz;
		private ChartView chartAx, chartAy, chartAz;
		private bool isSeekAx = false, isSeekAy = false, isSeekAz = false;

		private readonly IClientViewAccelerometerPresenter presenter;

		public ClientViewAccelerometerView(AccelerometerRecord record)
		{
			presenter = new ClientViewAccelerometerPresenter(this, record);
		}

		private void OnSeekAxStart(object sender, SeekBar.StartTrackingTouchEventArgs e)
		{
			isSeekAx = true;
		}

		private void OnSeekAyStart(object sender, SeekBar.StartTrackingTouchEventArgs e)
		{
			isSeekAy = true;
		}

		private void OnSeekAzStart(object sender, SeekBar.StartTrackingTouchEventArgs e)
		{
			isSeekAz = true;
		}

		private void OnSeekBarAxProgressChanged(object sender, SeekBar.ProgressChangedEventArgs e)
		{
			if (!isSeekAx)
				return;

			int progress = e.Progress;
			presenter.MoveAxTo(progress);
			//presenter.MoveTo(progress);
		}

		private void OnSeekBarAyProgressChanged(object sender, SeekBar.ProgressChangedEventArgs e)
		{
			if (!isSeekAy)
				return;

			int progress = e.Progress;
			presenter.MoveAyTo(progress);
		}

		private void OnSeekBarAzProgressChanged(object sender, SeekBar.ProgressChangedEventArgs e)
		{
			if (!isSeekAz)
				return;

			int progress = e.Progress;
			presenter.MoveAzTo(progress);
		}

		public void DisplayLineChartAx(LineChart lineAx)
		{
			this.lineAx = lineAx;
			Activity.RunOnUiThread(UpdateLineChartAx);
		}

		public void DisplayLineChartAy(LineChart lineAy)
		{
			this.lineAy = lineAy;
			Activity.RunOnUiThread(UpdateLineChartAy);
		}

		public void DisplayLineChartAz(LineChart lineAz)
		{
			this.lineAz = lineAz;
			Activity.RunOnUiThread(UpdateLineChartAz);
		}

		private void UpdateLineChartAx()
		{
			chartAx.Chart = lineAx;
		}

		private void UpdateLineChartAy()
		{
			chartAy.Chart = lineAy;
		}

		private void UpdateLineChartAz()
		{
			chartAz.Chart = lineAz;
		}
	}
}