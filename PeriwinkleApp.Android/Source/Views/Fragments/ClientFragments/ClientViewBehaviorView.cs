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
	public enum GraphType
	{
		TimeVoltage,
		VoltageFrequency
	};

    public interface IClientViewBehaviorView
	{
		void DisplayChartStatistics(ChartStat stats);
        void DisplayLineChart (LineChart lineChart);
	}

    public class ClientViewBehaviorView : Fragment,
										  GestureDetector.IOnGestureListener,
										  View.IOnTouchListener,
                                          IClientViewBehaviorView
	{
		private TextView txtStartTimeVal,
						 txtStopTimeVal,
						 txtDurationVal,
						 txtHighPeak,
						 txtLowPeak,
						 txtAvePeak,
						 txtLongInterval,
						 txtShortInterval,
						 txtAveInterval;

		private SeekBar seekBar;

		private Button btnChangeGraph;

        private ChartView timeChartView;
        private ChartView freqChartView;
		private LineChart timeChart;
		private LineChart freqChart;

        private GestureDetector gestureDetector;

		private readonly IClientViewBehaviorPresenter presenter;
		private bool isSeekBar = false;

		public ClientViewBehaviorView (BehaviorGraph behaviorGraph)
		{
			presenter = new ClientViewBehaviorPresenter (this, behaviorGraph);
        }

		public override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);
		}

		public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			return inflater.Inflate(Resource.Layout.client_frag_view_behavior, container, false);
		}

		public override async void OnViewCreated (View view, Bundle savedInstanceState)
		{
			base.OnViewCreated (view, savedInstanceState);
			// set reference sa chartview
			timeChartView = view.FindViewById<ChartView>(Resource.Id.chart_view_behavior);
			timeChartView.SetOnTouchListener(this);
			freqChartView = view.FindViewById<ChartView>(Resource.Id.chart_view_frequency);
			freqChartView.SetOnTouchListener(this);

            // time text views
            txtStartTimeVal = view.FindViewById<TextView>(Resource.Id.txt_bhv_start_time_val);
			txtStopTimeVal = view.FindViewById<TextView>(Resource.Id.txt_bhv_stop_time_val);
			txtDurationVal = view.FindViewById<TextView>(Resource.Id.txt_bhv_dur_val);

			txtHighPeak = view.FindViewById<TextView>(Resource.Id.txt_bhv_hpeak_val);
			txtLowPeak = view.FindViewById<TextView>(Resource.Id.txt_bhv_lpeak_val);
			txtAvePeak = view.FindViewById<TextView>(Resource.Id.txt_bhv_apeak_val);
			txtLongInterval = view.FindViewById<TextView>(Resource.Id.txt_bhv_lint_val);
			txtShortInterval = view.FindViewById<TextView>(Resource.Id.txt_bhv_sint_val);
			txtAveInterval = view.FindViewById<TextView>(Resource.Id.txt_bhv_aint_val);

			// seek bar
			seekBar = view.FindViewById<SeekBar>(Resource.Id.seek_bar_behavior);
			seekBar.ProgressChanged += OnSeekBarProgressChanged;
			seekBar.StartTrackingTouch += OnSeekBarStart;
            //			seekBar.Enabled = false;

            // button
			//btnChangeGraph = view.FindViewById<Button>(Resource.Id.btn_change_graph);
			//btnChangeGraph.Click += OnChangeGraphClicked;

            gestureDetector = new GestureDetector (this.Context, this);
			
			await presenter.LoadInitialLineChartData();
			seekBar.Max = presenter.EntryCount;
			
//			presenter.LoadChartStatistics ();
		}

		private GraphType curGraphType = GraphType.TimeVoltage;

		private void OnChangeGraphClicked (object sender, EventArgs e)
		{
			if (curGraphType == GraphType.TimeVoltage)
			{
				btnChangeGraph.Text = "View Voltage-Frequency";
				timeChartView.Visibility = ViewStates.Visible;
				freqChartView.Visibility = ViewStates.Gone;
				curGraphType = GraphType.VoltageFrequency;
            }
            else if (curGraphType == GraphType.VoltageFrequency)
			{
				btnChangeGraph.Text = "View Time-Voltage";
				timeChartView.Visibility = ViewStates.Gone;
				freqChartView.Visibility = ViewStates.Visible;
                curGraphType = GraphType.TimeVoltage;
            }
//            presenter.ChangeGraphType (curGraphType);
		}

		private void OnSeekBarStart (object sender, SeekBar.StartTrackingTouchEventArgs e)
		{
			isSeekBar = true;
		}

		private void OnSeekBarProgressChanged (object sender, SeekBar.ProgressChangedEventArgs e)
		{
			if (!isSeekBar)
				return;
			
			int progress = e.Progress;
			presenter.MoveTo(progress);
        }

        #region IClientViewBehaviorView

		public void DisplayChartStatistics (ChartStat stats)
		{
			txtStartTimeVal.Text = stats.StartTime;
			txtStopTimeVal.Text = stats.StopTime;
			txtDurationVal.Text = stats.Duration;
			txtHighPeak.Text = stats.HighestPeak + " V";
			txtLowPeak.Text = stats.LowestPeak + " V";
			txtAvePeak.Text = stats.AveragePeak + " V";
			txtLongInterval.Text = stats.LongestInterval + " s";
			txtShortInterval.Text = stats.ShortestInterval + " s";
			txtAveInterval.Text = stats.AverageInterval + " s";
		}
		
		public void DisplayLineChart(LineChart lineChart)
		{
			this.timeChart = lineChart;
			Activity.RunOnUiThread(UpdateLineChart);
		}

		private void UpdateLineChart()
		{
			timeChartView.Chart = timeChart;
		}

        #endregion

        #region Scrolling

        private void OnScrollRight (int v)
		{
			Activity.RunOnUiThread(() => { presenter.MoveRight (v); });
		}

        private void OnScrollLeft (int v)
		{
			Activity.RunOnUiThread(() => { presenter.MoveLeft (v); });
		}

		#endregion

        #region View.IOnTouchListener

        public bool OnTouch(View v, MotionEvent e)
		{
			return gestureDetector.OnTouchEvent (e);
		}

        #endregion

        #region GestureDetector.IOnGestureListener

        public bool OnDown (MotionEvent e)
		{
            return true;
		}

        public bool OnFling (MotionEvent e1, MotionEvent e2, float velocityX, float velocityY)
		{
			return true;
		}

        public void OnLongPress (MotionEvent e)
		{
			return;
		}

		private int scrollCounter = 0;
		private const int ScrollCountDivider = 3;

        public bool OnScroll (MotionEvent e1, MotionEvent e2, float distanceX, float distanceY)
		{
			float dist = MathF.Sqrt(MathF.Pow(distanceX, 2) + MathF.Pow(distanceY, 2));
			int count = (int) Math.Ceiling(dist / 10f);

			if (count < 0)
				count = 1;
			else if (count > 5)
				count = 5;

			Logger.Log(count.ToString());

			int curProgress = seekBar.Progress;
			if(distanceX > 0)
			{
				presenter.MoveTo(curProgress + count);
				seekBar.SetProgress(curProgress + count, true);
			}
			else
			{
				presenter.MoveTo(curProgress - count);
				seekBar.SetProgress(curProgress - count, true);
			}

			return true;
		}

        public void OnShowPress (MotionEvent e)
		{
			return;
		}

        public bool OnSingleTapUp (MotionEvent e)
		{
			return true;
		}

        #endregion


    }
}
