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
	public interface IClientViewAccelerometerPresenter
	{
		Task LoadInitialLineChartData();
		void MoveAxTo(int startPosition);
		void MoveAyTo(int startPosition);
		void MoveAzTo(int startPosition);
	}


	public class ClientViewAccelerometerPresenter : IClientViewAccelerometerPresenter
	{
		private readonly IClientViewAccelerometerView view;
		private AccelerometerRecord record;

		private IFileService fileService;
		private IClientService cliService;
		private const float MaxVoltage = 5f;
		private const float Resolution = 1024f; // 10-bit adc

		private LineChart lineAx, lineAy, lineAz;
		private List<Entry> entriesAx, entriesAy, entriesAz;
		private string contents;

		private const int MaxDisplaySize = 16;

		private LinkedList<Entry> displayAx, displayAy, displayAz;

		public int AxEntryCount { get; private set; }
		public int AyEntryCount { get; private set; }
		public int AzEntryCount { get; private set; }

		public ClientViewAccelerometerPresenter(IClientViewAccelerometerView view, AccelerometerRecord record)
		{
			this.view = view;
			this.record = record;
			fileService = fileService ?? new FileService(FileDirectory.Accelerometer);
			cliService = cliService ?? new ClientService();
			entriesAx = new List<Entry>();
			entriesAy = new List<Entry>();
			entriesAz = new List<Entry>();
			displayAx = new LinkedList<Entry>();
			displayAy = new LinkedList<Entry>();
			displayAz = new LinkedList<Entry>();
			InitLineChart();
		}

		public async Task LoadInitialLineChartData()
		{
			string filename = record.Filename;

			// file doesnt exist on client machine, get from the server
			if (!fileService.DoesFileExist(filename))
			{
				string directory = FileService.GetFileDirectory(FileDirectory.Accelerometer);
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
					// ["label", "value"]
					string[] entryStr = line.Split(',');

					string time = entryStr[0];
					string ax = entryStr[1];
					string ay = entryStr[2];
					string az = entryStr[3];

					Entry entryAx = CreateEntryFromString(time, ax);
					Entry entryAy = CreateEntryFromString(time, ay);
					Entry entryAz = CreateEntryFromString(time, az);

					entriesAx.Add(entryAx);
					entriesAy.Add(entryAy);
					entriesAz.Add(entryAz);
				}
			}

			// Ax
			AxEntryCount = entriesAx.Count;
			for (int i = 0; i < MaxDisplaySize && i < AxEntryCount; i++)
				displayAx.AddLast(new LinkedListNode<Entry>(entriesAx[i]));

			UpdateLineChartAx();

			// Ay
			AyEntryCount = entriesAy.Count;
			for (int i = 0; i < MaxDisplaySize && i < AyEntryCount; i++)
				displayAy.AddLast(new LinkedListNode<Entry>(entriesAy[i]));

			UpdateLineChartAy();

			// Az
			AzEntryCount = entriesAz.Count;
			for (int i = 0; i < MaxDisplaySize && i < AzEntryCount; i++)
				displayAz.AddLast(new LinkedListNode<Entry>(entriesAz[i]));

			UpdateLineChartAz();
		}

		private void UpdateLineChartAx()
		{
			lineAx.Entries = displayAx;
			view.DisplayLineChartAx(lineAx);
		}
		private void UpdateLineChartAy()
		{
			lineAy.Entries = displayAy;
			view.DisplayLineChartAy(lineAy);
		}
		private void UpdateLineChartAz()
		{
			lineAz.Entries = displayAz;
			view.DisplayLineChartAz(lineAz);
		}

		public void MoveAxTo(int startPosition)
		{
			int posHigh = startPosition + MaxDisplaySize;

			if (posHigh >= AxEntryCount)
				return;

			displayAx.Clear();

			for (int i = startPosition; i < posHigh; i++)
			{
				if (i >= entriesAx.Count || i < 0)
					break;
				displayAx.AddLast(entriesAx[i]);
			}

			//TODO TEST MO KO
			UpdateLineChartAx();
		}

		public void MoveAyTo(int startPosition)
		{
			int posHigh = startPosition + MaxDisplaySize;

			if (posHigh >= AyEntryCount)
				return;

			displayAy.Clear();

			for (int i = startPosition; i < posHigh; i++)
			{
				if (i >= entriesAy.Count || i < 0)
					break;
				displayAy.AddLast(entriesAy[i]);
			}

			//TODO TEST MO KO
			UpdateLineChartAy();
		}
		public void MoveAzTo(int startPosition)
		{
			int posHigh = startPosition + MaxDisplaySize;

			if (posHigh >= AzEntryCount)
				return;

			displayAz.Clear();

			for (int i = startPosition; i < posHigh; i++)
			{
				if (i >= entriesAz.Count || i < 0)
					break;
				displayAz.AddLast(entriesAz[i]);
			}

			//TODO TEST MO KO
			UpdateLineChartAz();
		}

		private Entry CreateEntryFromString(string label, string valueStr)
		{
			// naka int pa sya pag niread mo sa file.
			if (!int.TryParse(valueStr, out int value))
				throw new Exception("CreateEntryFromString - Could not parse value.");

			float voltageValue = GetVoltage(value);

			return new Entry(voltageValue)
			{
				ValueLabel = voltageValue.ToString(CultureInfo.InvariantCulture),
				Label = label,
				Color = GetRandomColor()
			};
		}

		private float GetVoltage(int adcValue)
		{
			return MathF.Round(((float)adcValue / Resolution * MaxVoltage), 4);
		}
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

		private SKColor GetRandomColor()
		{
			return SKColor.Parse(GraphicsUtil.HexConverter());
		}
	}
}