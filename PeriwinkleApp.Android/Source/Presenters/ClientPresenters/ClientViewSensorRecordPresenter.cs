using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using Microcharts;
using PeriwinkleApp.Android.Source.Cache;
using PeriwinkleApp.Android.Source.Utils;
using PeriwinkleApp.Android.Source.Views.Fragments.ClientFragments;
using PeriwinkleApp.Core.Sources.Extensions;
using PeriwinkleApp.Core.Sources.Models.Common;
using PeriwinkleApp.Core.Sources.Models.Domain;
using PeriwinkleApp.Core.Sources.Services;
using PeriwinkleApp.Core.Sources.Services.Interfaces;
using SkiaSharp;

namespace PeriwinkleApp.Android.Source.Presenters.ClientPresenters
{
	public interface IClientViewSensorRecordPresenter
	{
		Task LoadInitialLineChartData();
		int CurrentIndexPiezo { get; }
		int CurrentIndexAcceleration { get; }
		int EntryCountPiezo { get; }
		int EntryCountAcceleration { get; }
		void MovePiezoTo(int startPosition);
		void MoveAccelerationTo(int startPosition);
	}
	public class ClientViewSensorRecordPresenter : IClientViewSensorRecordPresenter
	{
		private readonly IClientViewSensorRecordView view;
		private SensorRecord record;
		private IFileService fileService;
		private IStatisticsService statService;
		private IClientService cliService;

		private const int MaxValue = 1024;
		private const int MaxDisplaySize = 16;
		private const int LineEntryCount = 5;

		private string contents;
		private LineChart linePiezo, lineAx, lineAy, lineAz;
		private List<Entry> entriesPiezo;
		private List<Tuple<Entry, Entry, Entry>> entriesAccel;
		private LinkedList<Entry> displayPiezo, displayAx, displayAy, displayAz;


		public int CurrentIndexPiezo { get; private set; } = 0;
		public int CurrentIndexAcceleration { get; private set; } = 0;
		public int EntryCountPiezo { get; private set; }
		public int EntryCountAcceleration { get; private set; }


		public ClientViewSensorRecordPresenter(IClientViewSensorRecordView view, SensorRecord record)
		{
			this.view = view;
			this.record = record;

			fileService = fileService ?? new FileService(FileDirectory.SensorRecord);
			cliService = cliService ?? new ClientService();

			entriesPiezo = new List<Entry>();
			entriesAccel = new List<Tuple<Entry, Entry, Entry>>();

			displayPiezo = new LinkedList<Entry>();
			displayAx= new LinkedList<Entry>();
			displayAy = new LinkedList<Entry>();
			displayAz = new LinkedList<Entry>();

			InitLineChart();
		}

		private void UpdatePiezo()
		{
			linePiezo.Entries = displayPiezo;
			view.DisplayPiezoLineChart(linePiezo);
		}

		private void UpdateAcceleration()
		{
			lineAx.Entries = displayAx;
			lineAy.Entries = displayAy;
			lineAz.Entries = displayAz;
			view.DisplayAccelerationLineChart(lineAx, lineAy, lineAz);
		}
		
		public async Task LoadInitialLineChartData()
		{
			string filename = record.Filename;

			// file doesnt exist on client machine, get from the server and save to client machine
			if (!fileService.DoesFileExist(filename))
			{
				string directory = FileService.GetFileDirectory(FileDirectory.SensorRecord);
				contents = await cliService.GetFileContentByFilename(filename, directory);
				await fileService.WriteAsync(filename, contents);
			}

			// read contents from file
			contents = await fileService.ReadToEndAsStringAsync(filename);

			// pag walang laman ung file, wala na
			if (contents == null)
				return;

			// each entry (label,value) is separated by newline
			using (StringReader reader = new StringReader(contents))
			{
				// "label,value"
				string line;
				while ((line = reader.ReadLine()) != null)
				{
					// time, piezo, ax, ay, az
					string[] entryStr = line.Split(',');

					// dapat 5 ung entry,
					if (entryStr.Length != LineEntryCount)
						continue;

					string recordTime = entryStr[0];
					float.TryParse(entryStr[1], out float piezo);
					float.TryParse(entryStr[2], out float ax);
					float.TryParse(entryStr[3], out float ay);
					float.TryParse(entryStr[4], out float az);

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

					entriesPiezo.Add(ePiezo);
					entriesAccel.Add(Tuple.Create(eAx, eAy, eAz));
				}
			}

			EntryCountPiezo = entriesPiezo.Count;
			EntryCountAcceleration = entriesAccel.Count;

			// piezo display entries
			for (int i = 0; i < MaxDisplaySize && i < entriesPiezo.Count; i++)
				displayPiezo.AddLast(new LinkedListNode<Entry>(entriesPiezo[i]));

			// acceleration display entries
			for (int i = 0; i < MaxDisplaySize && i < entriesAccel.Count; i++)
			{
				displayAx.AddLast(new LinkedListNode<Entry>(entriesAccel[i].Item1));
				displayAy.AddLast(new LinkedListNode<Entry>(entriesAccel[i].Item2));
				displayAz.AddLast(new LinkedListNode<Entry>(entriesAccel[i].Item3));
			}

			// kunyare 16 - 1, 15 ung index ung last from entries list.
			CurrentIndexPiezo = CurrentIndexAcceleration = MaxDisplaySize - 1;

			UpdatePiezo();
			UpdateAcceleration();
		}

		private void InitLineChart()
		{
			linePiezo = linePiezo ??
						new LineChart()
						{
							Entries = displayPiezo,
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
							Entries = displayAx,
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
							Entries = displayAy,
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
							Entries = displayAz,
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

		public void MovePiezoTo(int startPosition)
		{
			int posHigh = startPosition + MaxDisplaySize;
			CurrentIndexPiezo = posHigh;

			if (posHigh >= entriesPiezo.Count)
				return;

			displayPiezo.Clear();
			for(int i = startPosition; i < posHigh; i++)
			{
				if (i >= entriesPiezo.Count || i < 0)
					break;
				displayPiezo.AddLast(entriesPiezo[i]);
			}
			UpdatePiezo();
		}

		public void MoveAccelerationTo(int startPosition)
		{
			int posHigh = startPosition + MaxDisplaySize;
			CurrentIndexAcceleration = posHigh;

			if (posHigh > entriesAccel.Count)
				return;

			displayAx.Clear();
			displayAy.Clear();
			displayAz.Clear();
			for(int i = startPosition; i < posHigh; i++)
			{
				if (i >= entriesAccel.Count || i < 0)
					break;

				displayAx.AddLast(entriesAccel[i].Item1);
				displayAy.AddLast(entriesAccel[i].Item2);
				displayAz.AddLast(entriesAccel[i].Item3);
			}
			UpdateAcceleration();
		}
	}
}