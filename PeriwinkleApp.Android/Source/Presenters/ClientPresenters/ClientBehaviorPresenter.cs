using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using Microcharts;
using PeriwinkleApp.Android.Source.Factories;
using PeriwinkleApp.Android.Source.Session;
using PeriwinkleApp.Android.Source.Utils;
using PeriwinkleApp.Android.Source.Views.Activities;
using PeriwinkleApp.Core.Sources.Models.Domain;
using PeriwinkleApp.Core.Sources.Services;
using PeriwinkleApp.Core.Sources.Services.Interfaces;
using PeriwinkleApp.Core.Sources.Utils;
using SkiaSharp;
using Timer = System.Timers.Timer;

namespace PeriwinkleApp.Android.Source.Presenters.ClientPresenters
{
	public interface IClientBehaviorPresenter
	{
		void LoadInitialLineChartData ();
		void StartTimer ();
		void StopTimer ();
		void CreateRandomEntry ();
        void AddEntry(int value);
    }

	public class ClientBehaviorPresenter : IClientBehaviorPresenter
	{
		private const float MaxVoltage = 5f;
		private const float Resolution = 1024f; // 10-bit adc

		private readonly IClientBehaviorActivity activity;
		private List <Entry> entries;
		private Queue <Entry> displayQueue;

		private LineChart lineChart;
		private Timer timer;
		private const int MaxQueueSize = 16;
		private const int EntriesCapacity = 1024;
		private const int EntriesBeforeRefresh = 100;

		private string startTimeStr, stopTimeStr; // TODO, Class? then elapsed time chu chu
		private DateTime startTime, stopTime;

		private readonly IFileService fileService;
		private readonly IClientService cliService;
		private Client client;
		private ClientSession cliSession;
		private string currentFilename;
		public bool IsEnabled { get; protected set; }

        public ClientBehaviorPresenter (IClientBehaviorActivity activity)
		{
			this.activity = activity;
			entries = entries ?? new List <Entry> (EntriesCapacity);
			displayQueue = displayQueue ?? new Queue <Entry> ();
            InitLineChart ();
			fileService = new FileService(FileDirectory.Graph);
			cliService = new ClientService ();
			LoadClientSession ();
			IsEnabled = false;
		}

		private void LoadClientSession ()
		{
			cliSession = SessionFactory.ReadSession <ClientSession> (SessionKeys.LoggedClient);
//			if (cliSession != null && cliSession.IsSet)
//				client = await cliService.GetClientByUsername(cliSession.Username);
        }

#region Chart

        private void InitLineChart ()
		{
			lineChart = lineChart ??
						new LineChart ()
						{
							Entries = entries,
							LabelOrientation = Orientation.Vertical,
							ValueLabelOrientation = Orientation.Horizontal,
							MinValue = 0,
							MaxValue = MaxVoltage,
							IsAnimated = false,
							LabelColor = SKColor.Parse ("#000000")
						};
		}

		private void SetQuickStartEntries ()
		{
			entries = new List <Entry> ()
					  {
						  new Entry (0)
						  {
							  Label = "9:01",
							  ValueLabel = "0",
							  Color = SKColor.Parse ("#266489"),
						  },
						  new Entry (1)
						  {
							  Label = "9:02",
							  ValueLabel = "1",
							  Color = SKColor.Parse ("#68B9C0"),
						  },
						  new Entry (2)
						  {
							  Label = "9:03",
							  ValueLabel = "2",
							  Color = SKColor.Parse ("#90D585"),
						  },
						  new Entry (3)
						  {
							  Label = "9:04",
							  ValueLabel = "3",
							  Color = SKColor.Parse ("#90D585"),
						  },
						  new Entry (4)
						  {
							  Label = "9:05",
							  ValueLabel = "4",
							  Color = SKColor.Parse ("#90D585"),
						  },
						  new Entry (5)
						  {
							  Label = "9:06",
							  ValueLabel = "5",
							  Color = SKColor.Parse ("#90D585"),
						  },
						  new Entry (5)
						  {
							  Label = "9:07",
							  ValueLabel = "5",
							  Color = SKColor.Parse ("#90D585"),
						  },
						  new Entry (4)
						  {
							  Label = "9:08",
							  ValueLabel = "4",
							  Color = SKColor.Parse ("#90D585"),
						  },
						  new Entry (3)
						  {
							  Label = "9:09",
							  ValueLabel = "3",
							  Color = SKColor.Parse ("#90D585"),
						  },
						  new Entry (2)
						  {
							  Label = "9:10",
							  ValueLabel = "2",
							  Color = SKColor.Parse ("#90D585"),
						  },
						  new Entry (1)
						  {
							  Label = "9:11",
							  ValueLabel = "1",
							  Color = SKColor.Parse ("#90D585"),
						  },
						  new Entry (0)
						  {
							  Label = "9:12",
							  ValueLabel = "0",
							  Color = SKColor.Parse ("#90D585"),
						  },
						  new Entry (0)
						  {
							  Label = "9:01",
							  ValueLabel = "0",
							  Color = SKColor.Parse ("#266489"),
						  },
						  new Entry (1)
						  {
							  Label = "9:02",
							  ValueLabel = "1",
							  Color = SKColor.Parse ("#68B9C0"),
						  },
						  new Entry (2)
						  {
							  Label = "9:03",
							  ValueLabel = "2",
							  Color = SKColor.Parse ("#90D585"),
						  },
						  new Entry (3)
						  {
							  Label = "9:04",
							  ValueLabel = "3",
							  Color = SKColor.Parse ("#90D585"),
						  },
                      };
		}
		
		public void LoadInitialLineChartData ()
		{
			SetQuickStartEntries ();

			activity.DisplayLineChart(lineChart);
		}

		private void UpdateLineChart ()
		{
			lineChart.Entries = displayQueue;
			activity.DisplayLineChart(lineChart);
        }

#endregion


#region Timer

        private void InitTimer()
		{
			const double timeInterval = 50.0;

			// AutoReset => every time interval, mag raise ung event
			timer = new Timer (timeInterval) {AutoReset = true};
			timer.Elapsed += OnTimerElapsedEvent;
		}
		
		private void OnTimerElapsedEvent(object sender, ElapsedEventArgs e)
		{
			var time = e.SignalTime;
			string timeToDisplay = e.SignalTime.ToString ("m:ss.ff");
//			Logger.Log (timeToDisplay);

			activity.CreateRandomEntryOnUiThread ();
//			CreateRandomEntry ();
		}
		
		public void CreateRandomEntry ()
		{
			Entry entry = CreateNewEntry();
			UpdateDisplayEntry(entry);
            UpdateLineChart();

			Task.Run (async () => { await UpdateEntries (entry); });
		}

        public void AddEntry(int value)
        {
			//float voltageValue = value;// / 1024f * 5f;
			float voltageValue = MathF.Round(value / Resolution * MaxVoltage, 4);// / 1024f * 5f;

			Entry entry = new Entry(voltageValue)
            {
                ValueLabel = voltageValue.ToString(CultureInfo.InvariantCulture),
                Label = DateTime.Now.ToString("HH:mm:ss.f"),
                Color = GetRandomColor()
            };

            UpdateDisplayEntry(entry);
            UpdateLineChart();

            Task.Run(async () => {
				await UpdateEntries(entry);
			});
        }

        public void StartTimer ()
		{
			// pag enabled na, return
			if (IsEnabled)
				return;

			// pag di pag enabled, saka natin i-start
			IsEnabled = true;

            // get time before starting
			startTime = DateTime.Now;
            startTimeStr = startTime.ToLongTimeString();
            activity.SetStartTime (startTimeStr);
			Logger.Log("Start Timer");
			currentFilename = FileService.GenerateFilename("Behavior", cliSession.Username, FileExtension.Graph);
		}
		
        public async void StopTimer ()
		{
			// pag di enabled, return lng
			if (!IsEnabled)
				return;

			// enabled, so pwede i-stop
			IsEnabled = false;

			// get time when stopped
			stopTime = DateTime.Now;
			stopTimeStr = stopTime.ToLongTimeString();
			activity.SetStopTime (stopTimeStr);

			// pagkastop write ulit last
			await WriteEntries ();

            // create model TODO
            BehaviorGraph graph = new BehaviorGraph()
								  {
									  Filename = currentFilename,
									  GraphClientId = cliSession.ClientId,
									  StartTime = startTime,
									  StopTime = stopTime
								  };

			Logger.Debug(graph);

			// save to db
			var response = await cliService.AddBehaviorGraphs (graph);
		}

        #endregion

		private Entry CreateNewEntry ()
		{
			float voltageValue = GetRandomVoltage();

			Entry entry = new Entry (voltageValue)
						  {
							  ValueLabel = voltageValue.ToString (CultureInfo.InvariantCulture),
							  Label = DateTime.Now.ToString ("m:ss.f"),
							  Color = GetRandomColor ()
						  };

			return entry;
		}

		private void UpdateDisplayEntry(Entry entry)
		{
			if (displayQueue.Count >= MaxQueueSize)
				displayQueue.Dequeue();

			displayQueue.Enqueue(entry);
		}

		private async Task UpdateEntries (Entry entry)
		{
			if (entries.Count >= EntriesBeforeRefresh)
			{
				await WriteEntries ();
                entries.Clear ();
				return;
			}
			
			entries.Add (entry);
		}

		private async Task WriteEntries ()
		{
			string content = string.Join(Environment.NewLine,
								entries.Select(e => 
								string.Join(",",new[] { e.Label, ((int)(e.Value / MaxVoltage * Resolution)).ToString ()  })).ToList());

			entries.Clear();

			await fileService.WriteLineAppendAsync(currentFilename, content);
        }

        Random rndVoltage = new Random();
		private float GetRandomVoltage ()
		{
			int adcValue = rndVoltage.Next (1024);
		
			return ((float) adcValue / Resolution * MaxVoltage);
		}

		private SKColor GetRandomColor ()
		{
			return SKColor.Parse (GraphicsUtil.HexConverter ());
		}
	}
}
