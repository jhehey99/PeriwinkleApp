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
	public interface IClientAccelerometerPresenter
	{
		void StartTimer();
		void StopTimer();
		void AddEntry(int ax, int ay, int az);
	}

	public class ClientAccelerometerPresenter: IClientAccelerometerPresenter
	{
		private const float MaxVoltage = 5f;
		private const float Resolution = 1024f; // 10-bit adc

		private readonly IClientAccelerometerActivity activity;
		//private List<Entry> entries;
		//private Queue<Entry> displayQueue;
		//private LineChart lineChart;

		private int entryCount;
		private List<Entry> entriesAx, entriesAy, entriesAz;
		private Queue<Entry> queueAx, queueAy, queueAz;
		private LineChart lineAx, lineAy, lineAz;

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

		public ClientAccelerometerPresenter(IClientAccelerometerActivity activity)
		{
			this.activity = activity;
			entriesAx = entriesAx ?? new List<Entry>(EntriesCapacity);
			entriesAy = entriesAy ?? new List<Entry>(EntriesCapacity);
			entriesAz = entriesAz ?? new List<Entry>(EntriesCapacity);
			queueAx = queueAx ?? new Queue<Entry>();
			queueAy = queueAy ?? new Queue<Entry>();
			queueAz = queueAz ?? new Queue<Entry>();
			InitLineChart();
			fileService = new FileService(FileDirectory.Accelerometer);
			cliService = new ClientService();
			LoadClientSession();
			IsEnabled = false;
		}

		private void LoadClientSession()
		{
			cliSession = SessionFactory.ReadSession<ClientSession>(SessionKeys.LoggedClient);
		}

#region Chart
		private void InitLineChart()
		{
			lineAx = lineAx ??
						new LineChart()
						{
							Entries = entriesAx,
							LabelOrientation = Orientation.Vertical,
							ValueLabelOrientation = Orientation.Horizontal,
							MinValue = 0,
							MaxValue = MaxVoltage,
							IsAnimated = false,
							LabelColor = SKColor.Parse("#000000")
						};

			lineAy = lineAy ??
						new LineChart()
						{
							Entries = entriesAy,
							LabelOrientation = Orientation.Vertical,
							ValueLabelOrientation = Orientation.Horizontal,
							MinValue = 0,
							MaxValue = MaxVoltage,
							IsAnimated = false,
							LabelColor = SKColor.Parse("#000000")
						};

			lineAz = lineAz ??
						new LineChart()
						{
							Entries = entriesAz,
							LabelOrientation = Orientation.Vertical,
							ValueLabelOrientation = Orientation.Horizontal,
							MinValue = 0,
							MaxValue = MaxVoltage,
							IsAnimated = false,
							LabelColor = SKColor.Parse("#000000")
						};
		}

		private void UpdateLineChart()
		{
			//lineChart.Entries = displayQueue;
			lineAx.Entries = queueAx;
			lineAy.Entries = queueAy;
			lineAz.Entries = queueAz;
			
			activity.DisplayLineChart(lineAx, lineAy, lineAz);
		}

		#endregion


		#region Timer
		public void AddEntry(int ax, int ay, int az)
		{
			entryCount++;
			float vAx = MathF.Round(ax / Resolution * MaxVoltage, 4);
			float vAy = MathF.Round(ay / Resolution * MaxVoltage, 4);
			float vAz = MathF.Round(az / Resolution * MaxVoltage, 4);

			Entry eAx = new Entry(vAx)
			{
				ValueLabel = vAx.ToString(CultureInfo.InvariantCulture),
				Label = DateTime.Now.ToString("HH:mm:ss.f"),
				Color = GetRandomColor()
			};

			Entry eAy = new Entry(vAy)
			{
				ValueLabel = vAy.ToString(CultureInfo.InvariantCulture),
				Label = DateTime.Now.ToString("HH:mm:ss.f"),
				Color = GetRandomColor()
			};

			Entry eAz = new Entry(vAz)
			{
				ValueLabel = vAz.ToString(CultureInfo.InvariantCulture),
				Label = DateTime.Now.ToString("HH:mm:ss.f"),
				Color = GetRandomColor()
			};

			UpdateDisplayEntry(eAx, eAy, eAz);
			UpdateLineChart();

			Task.Run(async () => {
				await UpdateEntries(eAx, eAy, eAz);
			});
		}

		public void StartTimer()
		{
			// pag enabled na, return
			if (IsEnabled)
				return;

			// pag di pag enabled, saka natin i-start
			IsEnabled = true;

			// get time before starting
			startTime = DateTime.Now;
			startTimeStr = startTime.ToLongTimeString();
			activity.SetStartTime(startTimeStr);
			Logger.Log("Start Timer");
			currentFilename = FileService.GenerateFilename("Accelerometer", cliSession.Username, FileExtension.Accelerometer);
		}

		public async void StopTimer()
		{
			// pag di enabled, return lng
			if (!IsEnabled)
				return;

			// enabled, so pwede i-stop
			IsEnabled = false;

			// get time when stopped
			stopTime = DateTime.Now;
			stopTimeStr = stopTime.ToLongTimeString();
			activity.SetStopTime(stopTimeStr);

			// pagkastop write ulit last
			await WriteEntries();

			// create model TODO
			AccelerometerRecord record = new AccelerometerRecord()
			{
				Filename = currentFilename,
				ClientId = cliSession.ClientId,
				StartTime = startTime,
				StopTime = stopTime
			};

			Logger.Debug(record);

			// save to db
			var response = await cliService.AddAccelerometerRecord(record);
		}

		#endregion

		private void UpdateDisplayEntry(Entry eAx, Entry eAy, Entry eAz)
		{
			if (queueAx.Count >= MaxQueueSize)
				queueAx.Dequeue();

			queueAx.Enqueue(eAx);

			if (queueAy.Count >= MaxQueueSize)
				queueAy.Dequeue();

			queueAy.Enqueue(eAy);

			if (queueAz.Count >= MaxQueueSize)
				queueAz.Dequeue();

			queueAz.Enqueue(eAz);
		}

		private async Task UpdateEntries(Entry eAx, Entry eAy, Entry eAz)
		{
			if (entryCount >= EntriesBeforeRefresh)
			{
				await WriteEntries();
				entriesAx.Clear();
				entriesAy.Clear();
				entriesAz.Clear();
				entryCount = 0;
				return;
			}

			entriesAx.Add(eAx);
			entriesAy.Add(eAy);
			entriesAz.Add(eAz);
		}

		private async Task WriteEntries()
		{
			string content = string.Join(Environment.NewLine,
								entriesAx.Select((e, i) => string.Join(",", new[]
														   {	e.Label,
																((int)(entriesAx[i].Value / MaxVoltage * Resolution)).ToString(),
																((int)(entriesAy[i].Value / MaxVoltage * Resolution)).ToString(),
																((int)(entriesAz[i].Value / MaxVoltage * Resolution)).ToString()
														   })).ToList());

			entriesAx.Clear();
			entriesAy.Clear();
			entriesAz.Clear();
			entryCount = 0;

			await fileService.WriteLineAppendAsync(currentFilename, content);
		}

		private SKColor GetRandomColor()
		{
			return SKColor.Parse(GraphicsUtil.HexConverter());
		}
	}
}