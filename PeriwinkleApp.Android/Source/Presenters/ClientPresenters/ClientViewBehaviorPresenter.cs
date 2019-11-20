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
	public interface IClientViewBehaviorPresenter
	{
		//void ChangeGraphType (GraphType graphType);
		Task LoadInitialLineChartData ();
		void LoadChartStatistics();
        void MoveTo (int startPosition);
        void MoveRight (int count);
		void MoveLeft (int count);
		int CurrentIndex { get; }
		int EntryCount { get; }
    }

	public class ClientViewBehaviorPresenter : IClientViewBehaviorPresenter
	{
		private readonly IClientViewBehaviorView view;
		private BehaviorGraph behaviorGraph;
		private IFileService fileService;
		private IStatisticsService statService;
		private IClientService cliService;
		private const float MaxVoltage = 5f;
		private const float Resolution = 1024f; // 10-bit adc
		private LineChart lineChart;
        private List<Entry> entries;
		private string contents;

		private LinkedList <Entry> displayEntries;
		private List <Entry> fEntries;

        private const int MaxDisplaySize = 16;

		public int CurrentIndex { get; private set; } = 0;

		public int ContentSize { get; private set; } = 0;

		public int EntryCount { get; private set; }

		private GraphType type { get; set; }

		public ClientViewBehaviorPresenter (IClientViewBehaviorView view, BehaviorGraph behaviorGraph)
		{
			this.view = view;
			this.behaviorGraph = behaviorGraph;
			fileService = fileService ?? new FileService (FileDirectory.Graph);
			cliService = cliService ?? new ClientService ();
			entries = new List<Entry>();
			displayEntries = new LinkedList<Entry>();
			fEntries = new List <Entry> ();
            InitLineChart();
		}

		/*
		public void ChangeGraphType (GraphType graphType)
		{
			type = graphType;

            if (type == GraphType.VoltageFrequency && fEntries.Count <= 0)
			{
                // get frequency

				// frequency distribution
				List<FrequencyEntry> frequencyEntries = statService.GetVoltageFrequencyDistribution();

				foreach (FrequencyEntry frequencyEntry in frequencyEntries)
				{
					int frequency = frequencyEntry.Frequency;
					Entry entry = new Entry (frequency)
								  {
									  ValueLabel = frequency.ToString(CultureInfo.InvariantCulture),
									  Label = frequencyEntry.Voltage.ToString(CultureInfo.InvariantCulture),
									  Color = GetRandomColor ()
								  };

					fEntries.Add (entry);
                }
            }

			//TODO MAS MAGANDA KUNG IBANG CHART NALANG, TAS VISIBILITY NALANG I-BABAGO
			List<Entry> entriesToUse = (type == GraphType.TimeVoltage) ? entries : fEntries;
			
			EntryCount = entriesToUse.Count;
			//eto na ung display entries natin
			for (int i = 0; i < MaxDisplaySize && i < EntryCount; i++)
				displayEntries.AddLast(new LinkedListNode<Entry>(entriesToUse[i]));

			// kunyare 16 - 1, 15 ung index ung last from entries list.
			CurrentIndex = MaxDisplaySize - 1;

            UpdateLineChart ();
		}
		*/

		private void UpdateLineChart()
		{
			lineChart.Entries = displayEntries;
			view.DisplayLineChart(lineChart);
		}

		public async Task LoadInitialLineChartData()
		{
			string filename = behaviorGraph.Filename;

			// file doesnt exist on client machine, get from the server
			if (!fileService.DoesFileExist (filename))
			{
				//string directory = "graphs";
				string directory = FileService.GetFileDirectory(FileDirectory.Graph);
				contents = await cliService.GetFileContentByFilename (filename, directory);
				await fileService.WriteAsync (filename, contents);
			}

			// read contents from file
			contents = await fileService.ReadToEndAsStringAsync(filename);

			// pag walang laman ung file, wala na
			if (contents == null)
				return;

			// each entry (label,value) is separated by newline
            using (StringReader reader = new StringReader (contents))
			{
				// "label,value"
				string line;
				while ((line = reader.ReadLine ()) != null)
				{
					// ["label", "value"]
					string[] entryStr = line.Split (',');

					Entry entry = CreateEntryFromString (entryStr[0], entryStr[1]);
					entries.Add (entry);
				}
			}


			List<Entry> entriesToUse = (type == GraphType.TimeVoltage) ? entries : fEntries;

            EntryCount = entriesToUse.Count;
            //eto na ung display entries natin
            for (int i = 0; i < MaxDisplaySize && i < EntryCount; i++)
				displayEntries.AddLast (new LinkedListNode <Entry> (entriesToUse[i]));

			// kunyare 16 - 1, 15 ung index ung last from entries list.
			CurrentIndex = MaxDisplaySize - 1;

			UpdateLineChart ();
			LoadChartStatistics ();
        }

		public void LoadChartStatistics()
		{
			// todo 
			// cliService.GetBehaviorStatistics(

            statService = statService ?? new StatisticsServiceOld(contents);

			// Time
			string startTime = behaviorGraph.StartTime.ToString ("HH:mm:ss tt");
			string stopTime = behaviorGraph.StopTime.ToString ("HH:mm:ss tt");
			string duration = (behaviorGraph.StopTime - behaviorGraph.StartTime).ToString ("g");

			/*
			// max voltage
			List <BasicEntry> maxVoltages = statService.GetMaxVoltages();
			string maxValue = maxVoltages.ToFormattedString();

			// min voltage
            List<BasicEntry> minVoltages = statService.GetMinVoltages();
			string minValue = minVoltages.ToFormattedString();

			// average voltage
            string aveValue= statService.GetAverageVoltage().ToString(CultureInfo.InvariantCulture);
			*/
			ChartStat stats = new ChartStat()
							  {
								  StartTime = startTime,
								  StopTime = stopTime,
								  Duration = duration,
								  HighestPeak = statService.GetHighestPeak().ToString(),
								  LowestPeak = statService.GetLowestPeak().ToString(),
								  AveragePeak = statService.GetAveragePeak().ToString(),
								  LongestInterval = statService.GetLongestInterval().ToString(),
								  ShortestInterval = statService.GetShortestInterval().ToString(),
								  AverageInterval = statService.GetAverageInterval().ToString(),
            };
			
			view.DisplayChartStatistics(stats);

			// save memory? kasi tapos mo na basahin ung file eh.
            contents = null;
        }

        public void MoveTo (int startPosition)
		{
			List<Entry> entriesToUse = (type == GraphType.TimeVoltage) ? entries : fEntries;

            int posHigh = startPosition + MaxDisplaySize;
			CurrentIndex = posHigh;

            if (posHigh >= EntryCount)
				return;

			displayEntries.Clear ();

			for (int i = startPosition; i < posHigh; i++)
			{
				if (i >= entriesToUse.Count || i < 0)
					break;
				displayEntries.AddLast (entriesToUse[i]);
			}

			//TODO TEST MO KO
			UpdateLineChart ();
		}

		public void MoveRight (int count)
		{
			List<Entry> entriesToUse = (type == GraphType.TimeVoltage) ? entries : fEntries;

            for (int i = 0; i < count; i++)
			{
				if (CurrentIndex >= EntryCount)
					return;

				if (CurrentIndex < 0)
					CurrentIndex = 0;

				Entry entry = entriesToUse[CurrentIndex];
				displayEntries.RemoveFirst ();
				displayEntries.AddLast (new LinkedListNode <Entry> (entry));
				CurrentIndex++;
				UpdateLineChart ();
			}
		}
		
        public void MoveLeft(int count)
		{
			List<Entry> entriesToUse = (type == GraphType.TimeVoltage) ? entries : fEntries;

            for (int i = 0; i < count; i++)
			{
				if (CurrentIndex < 0)
					return;

				if (CurrentIndex >= EntryCount)
					CurrentIndex = EntryCount - 1;

				Entry entry = entriesToUse[CurrentIndex];
				displayEntries.RemoveLast ();
				displayEntries.AddFirst (new LinkedListNode <Entry> (entry));
				CurrentIndex--;
				UpdateLineChart ();
			}
		}

        private Entry CreateEntryFromString (string label, string valueStr)
		{
			// naka int pa sya pag niread mo sa file.
			if (!int.TryParse (valueStr, out int value))
				throw new Exception ("CreateEntryFromString - Could not parse value.");

			float voltageValue = GetVoltage (value);

			return new Entry (voltageValue)
				   {
					   ValueLabel = voltageValue.ToString (CultureInfo.InvariantCulture),
					   Label = label,
					   Color = GetRandomColor ()
				   };
		}

		private float GetVoltage (int adcValue)
		{
			return MathF.Round(((float)adcValue / Resolution * MaxVoltage), 4);
        }

		private void InitLineChart()
		{
			lineChart = lineChart ??
						new LineChart()
						{
							Entries = null,
							LabelOrientation = Orientation.Vertical,
							ValueLabelOrientation = Orientation.Vertical,
							MinValue = 0,
							MaxValue = 5,
							IsAnimated = false,
							LabelColor = SKColor.Parse("#000000")
						};
		}

        private SKColor GetRandomColor()
		{
			return SKColor.Parse(GraphicsUtil.HexConverter());
		}
    }
}
