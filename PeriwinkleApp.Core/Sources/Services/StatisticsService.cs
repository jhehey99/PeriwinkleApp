using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace PeriwinkleApp.Core.Sources.Services
{
	public interface IStatisticsService
	{
		float GetPiezoMax();
		float GetPiezoMin();
		float GetPiezoAverage();
		float GetAxMax();
		float GetAxMin();
		float GetAxAverage();
		float GetAyMax();
		float GetAyMin();
		float GetAyAverage();
		float GetAzMax();
		float GetAzMin();
		float GetAzAverage();
	}

	public class StatisticsService : IStatisticsService
	{
		public string Contents { get; set; }
		private List<string> timeList;
		private List<float> piezoList;
		private List<float> axList;
		private List<float> ayList;
		private List<float> azList;

		private const int MaxValue = 1024;
		private const int LineEntryCount = 5;

		public StatisticsService(string contents)
		{
			if (contents == null)
				return;

			Contents = contents;
			timeList = new List<string>();
			piezoList = new List<float>();
			axList = new List<float>();
			ayList = new List<float>();
			azList = new List<float>();

			using (StringReader reader = new StringReader(Contents))
			{
				string line;
				while((line = reader.ReadLine()) != null)
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

					timeList.Add(recordTime);
					piezoList.Add(piezo);
					axList.Add(ax);
					ayList.Add(ay);
					azList.Add(az);
				}
			}
		}

		~StatisticsService()
		{
			Contents = null;
			timeList = null;
			piezoList = null;
			axList = ayList = azList = null;
		}

		public float GetPiezoMax()
		{
			if (piezoList.Count <= 0)
				return 0f;
			return MathF.Round(piezoList.Max(), 4);
		}
		public float GetPiezoMin()
		{
			if (piezoList.Count <= 0)
				return 0f;
			return MathF.Round(piezoList.Min(), 4);
		}
		public float GetPiezoAverage()
		{
			if (piezoList.Count <= 0)
				return 0f;
			return MathF.Round(piezoList.Average(), 4);
		}


		public float GetAxMax()
		{
			if (axList.Count <= 0)
				return 0f;
			return MathF.Round(axList.Max(), 4);
		}
		public float GetAxMin()
		{
			if (axList.Count <= 0)
				return 0f;
			return MathF.Round(axList.Min(), 4);
		}
		public float GetAxAverage()
		{
			if (axList.Count <= 0)
				return 0f;
			return MathF.Round(axList.Average(), 4);
		}

		public float GetAyMax()
		{
			if (ayList.Count <= 0)
				return 0f;
			return MathF.Round(ayList.Max(), 4);
		}
		public float GetAyMin()
		{
			if (ayList.Count <= 0)
				return 0f;
			return MathF.Round(ayList.Min(), 4);
		}
		public float GetAyAverage()
		{
			if (ayList.Count <= 0)
				return 0f;
			return MathF.Round(ayList.Average(), 4);
		}

		public float GetAzMax()
		{
			if (azList.Count <= 0)
				return 0f;
			return MathF.Round(azList.Max(), 4);
		}
		public float GetAzMin()
		{
			if (azList.Count <= 0)
				return 0f;
			return MathF.Round(azList.Min(), 4);
		}
		public float GetAzAverage()
		{
			if (azList.Count <= 0)
				return 0f;
			return MathF.Round(azList.Average(), 4);
		}
	}
}