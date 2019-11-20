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
	public interface IClientNewRecordPresenter
	{
		void AddEntry(float piezo, float ax, float ay, float az);
		void StartTimer();
		void StopTimer();
		bool IsEnabled { get; }
	}
	public class ClientNewRecordPresenter : IClientNewRecordPresenter
	{
		private readonly IClientNewRecordActivity activity;

		private List<Tuple<string, string, string, string, string>> entries;

		private List<string> recordTimes;
		private List<Entry> entriesPiezo, entriesAx, entriesAy, entriesAz;
		private Queue<Entry> queuePiezo, queueAx, queueAy, queueAz;
		private LineChart linePiezo, lineAx, lineAy, lineAz;

		private Timer timer;
		private const int MaxValue = 1024;
		private const int MaxQueueSize = 16;
		private const int EntriesCapacity = 256;
		private const int EntriesBeforeRefresh = 100;

		private string startTimeStr, stopTimeStr;
		private DateTime startTime, stopTime;

		private readonly IFileService fileService;
		private readonly IClientService cliService;
		private Client client;
		private ClientSession cliSession;
		private string currentFilename;
		public bool IsEnabled { get; protected set; }

		public ClientNewRecordPresenter(IClientNewRecordActivity activity)
		{
			this.activity = activity;
			entries = entries ?? new List<Tuple<string, string, string, string, string>>(EntriesCapacity);
			recordTimes = recordTimes ?? new List<string>(EntriesCapacity);
			entriesPiezo = entriesPiezo ?? new List<Entry>(EntriesCapacity);
			entriesAx = entriesAx ?? new List<Entry>(EntriesCapacity);
			entriesAy = entriesAy ?? new List<Entry>(EntriesCapacity);
			entriesAz = entriesAz ?? new List<Entry>(EntriesCapacity);
			queuePiezo = queuePiezo ?? new Queue<Entry>();
			queueAx = queueAx ?? new Queue<Entry>();
			queueAy = queueAy ?? new Queue<Entry>();
			queueAz = queueAz ?? new Queue<Entry>();
			fileService = new FileService(FileDirectory.SensorRecord);
			cliService = new ClientService();
			LoadClientSession();
			IsEnabled = false;
			InitLineChart();
		}

		#region Chart
		private void InitLineChart()
		{
			linePiezo = linePiezo ??
						new LineChart()
						{
							Entries = queuePiezo,
							LabelOrientation = Orientation.Vertical,
							ValueLabelOrientation = Orientation.Horizontal,
							MinValue = 0,
							MaxValue = MaxValue,
							IsAnimated = false,
							LineAreaAlpha = 0,
							PointSize = 6,
							LabelTextSize = 10,
							LineMode = LineMode.Spline,
							BackgroundColor = SKColor.Empty,
							LabelColor = SKColor.Parse("#000000")
						};
			lineAx = lineAx ??
						new LineChart()
						{
							Entries = queueAx,
							LabelOrientation = Orientation.Vertical,
							ValueLabelOrientation = Orientation.Horizontal,
							MinValue = 0,
							MaxValue = MaxValue,
							IsAnimated = false,
							LineAreaAlpha = 0,
							PointSize = 6,
							LabelTextSize = 10,
							LineMode = LineMode.Spline,
							BackgroundColor = SKColor.Empty,
							LabelColor = SKColor.Empty
						};
			lineAy = lineAy ??
						new LineChart()
						{
							Entries = queueAy,
							LabelOrientation = Orientation.Vertical,
							ValueLabelOrientation = Orientation.Horizontal,
							MinValue = 0,
							MaxValue = MaxValue,
							IsAnimated = false,
							LineAreaAlpha = 0,
							PointSize = 6,
							LabelTextSize = 10,
							LineMode = LineMode.Spline,
							BackgroundColor = SKColor.Empty,
							LabelColor = SKColor.Empty
						};

			lineAz = lineAz ??
						new LineChart()
						{
							Entries = queueAz,
							LabelOrientation = Orientation.Vertical,
							ValueLabelOrientation = Orientation.Horizontal,
							MinValue = 0,
							MaxValue = MaxValue,
							IsAnimated = false,
							LineAreaAlpha = 0,
							PointSize = 6,
							LabelTextSize = 10,
							LineMode = LineMode.Spline,
							BackgroundColor = SKColor.Empty,
							LabelColor = SKColor.Parse("#000000")
						};
		}

		public void AddEntry(float piezo, float ax, float ay, float az)
		{
			string recordTime = DateTime.Now.ToString("HH:mm:ss.f");

			Entry ePiezo = new Entry(piezo)
			{
				ValueLabel = piezo.ToString(CultureInfo.InvariantCulture),
				Label = recordTime,
				Color = SKColor.Parse("#03A9F4")
			};
			Entry eAx = new Entry(ax)
			{
				ValueLabel = ax.ToString(CultureInfo.InvariantCulture),
				Label = recordTime,
				Color = SKColor.Parse("#E91E63")
			};
			Entry eAy = new Entry(ay)
			{
				ValueLabel = ay.ToString(CultureInfo.InvariantCulture),
				Label = recordTime,
				Color = SKColor.Parse("#FF9800")
			};
			Entry eAz = new Entry(az)
			{
				ValueLabel = az.ToString(CultureInfo.InvariantCulture),
				Label = recordTime,
				Color = SKColor.Parse("#4CAF50")
			};

			UpdateDisplayEntry(ePiezo, eAx, eAy, eAz);
			UpdateLineChart();

			//UpdateEntries(recordTime, ePiezo, eAx, eAy, eAz);

			if (IsEnabled)
			{
				Task.Run(async () => {
					await UpdateEntries(recordTime, ePiezo, eAx, eAy, eAz);
				});
			}
		}

		private async Task UpdateEntries(string recordTime, Entry ePiezo, Entry eAx, Entry eAy, Entry eAz)
		{
			if (entries.Count >= EntriesBeforeRefresh)
			{
				// TODO: Write lang pag enabled na. 
				await WriteEntries();
				entries.Clear();
				return;
			}
			entries.Add(Tuple.Create(recordTime, ePiezo.Value.ToString(), eAx.Value.ToString(), eAy.Value.ToString(), eAz.Value.ToString()));
		}

		private void UpdateDisplayEntry(Entry ePiezo, Entry eAx, Entry eAy, Entry eAz)
		{
			// Piezo
			if (queuePiezo.Count >= MaxQueueSize)
				queuePiezo.Dequeue();
			queuePiezo.Enqueue(ePiezo);

			// Ax
			if (queueAx.Count >= MaxQueueSize)
				queueAx.Dequeue();
			queueAx.Enqueue(eAx);

			// Ay
			if (queueAy.Count >= MaxQueueSize)
				queueAy.Dequeue();
			queueAy.Enqueue(eAy);

			// Az
			if (queueAz.Count >= MaxQueueSize)
				queueAz.Dequeue();
			queueAz.Enqueue(eAz);
		}

		private void UpdateLineChart()
		{
			linePiezo.Entries = queuePiezo;
			lineAx.Entries = queueAx;
			lineAy.Entries = queueAy;
			lineAz.Entries = queueAz;

			activity.DisplayLineChart(linePiezo, lineAx, lineAy, lineAz);
		}
		#endregion

		#region File Entries
		 private async Task WriteEntries()
		{
			Logger.Log("Write Entries");
			string content = String.Join(Environment.NewLine,
							entries.Select(t => String.Format($"{t.Item1},{t.Item2},{t.Item3},{t.Item4},{t.Item5}")));

			Logger.Log(content);
			entries.Clear();

			await fileService.WriteLineAppendAsync(currentFilename, content);
		}
		#endregion

		#region Timer
		public void StartTimer()
		{
			// pag enabled na, return
			if (IsEnabled)
				return;

			// pag di pag enabled, saka natin i-start
			IsEnabled = true;

			// get time before starting
			Logger.Log("Start Timer");
			startTime = DateTime.Now;
			startTimeStr = startTime.ToLongTimeString();
			activity.SetStartTime(startTimeStr);
			currentFilename = FileService.GenerateFilename("Record", cliSession.Username, FileExtension.SensorRecord);
		}

		public async void StopTimer()
		{
			// pag di enabled, return lng
			if (!IsEnabled)
				return;

			// enabled, so pwede i-stop
			IsEnabled = false;

			// get time when stopped
			Logger.Log("Stop Timer");
			stopTime = DateTime.Now;
			stopTimeStr = stopTime.ToLongTimeString();
			activity.SetStopTime(stopTimeStr);

			// pagkastop write ulit last
			await WriteEntries();

			// create model TODO
			SensorRecord record = new SensorRecord()
			{
				Filename = currentFilename,
				ClientId = cliSession.ClientId,
				StartTime = startTime,
				StopTime = stopTime
			};

			Logger.Debug(record);

			// save to db
			var response = await cliService.AddSensorRecord(record);
		}

		#endregion

		#region Utilities
		private void LoadClientSession()
		{
			cliSession = SessionFactory.ReadSession<ClientSession>(SessionKeys.LoggedClient);
		}

		private SKColor GetRandomColor()
		{
			return SKColor.Parse(GraphicsUtil.HexConverter());
		}
		#endregion
	}
}