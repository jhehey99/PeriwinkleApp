using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using PeriwinkleApp.Core.Sources.Models.Common;
using PeriwinkleApp.Core.Sources.Utils;

namespace PeriwinkleApp.Core.Sources.Services
{
	public interface IStatisticsService
	{
		/*
		List<BasicEntry> GetMaxVoltages ();
		List<BasicEntry> GetMinVoltages ();
		float GetAverageVoltage ();
		List <FrequencyEntry> GetVoltageFrequencyDistribution (float step = 0.25f, float min = 0f, float max = 5f);
		*/
		float GetHighestPeak();
		float GetLowestPeak();
		float GetAveragePeak();
		float GetLongestInterval();
		float GetShortestInterval();
		float GetAverageInterval();
	}

	public class StatisticsService : IStatisticsService
    {
		public string Contents { get; set; }

		private List <(string, float)> contentSplit;
		private List <string> timeList;
		private List<float> voltageList;
		private List<float> seconds;
		private List<float> intervals;

		private int maxVol = 5;
		private int maxVal = 1024;
		private int threshold = (int)(1024 * 0.25);

		public StatisticsService (string contents)
		{
			if (contents == null)
				return;

			Contents = contents;
            contentSplit = new List <(string, float)> ();
			timeList = new List <string> ();
			voltageList = new List <float> ();
			seconds = new List<float>();
			intervals = new List<float>();

			//split
			using (StringReader reader = new StringReader(contents))
			{
				// "label,value"
				string line;
				while ((line = reader.ReadLine()) != null)
				{
					// ["time", "voltage"]
					string[] entryStr = line.Split(',');

					string time = entryStr[0];

					if(!float.TryParse (entryStr[1], out float voltage))
						continue;

					// pag lower than threshold skip na
					if (voltage < threshold)
						continue;

					var time_split = time.Split(':');
					int l = time_split.Length;
					if(l == 2)
						seconds.Add(float.Parse(time_split[0]) * 60 + float.Parse(time_split[1]));
					else if (l == 3)
						seconds.Add(float.Parse(time_split[0]) * 60 + float.Parse(time_split[1]) * 60 + float.Parse(time_split[2]));
					else
						continue;

					timeList.Add(time);
                    voltageList.Add (voltage);
					contentSplit.Add ((time, voltage));
                }
			}
			
			for(int i = 0; i < seconds.Count - 1; i ++)
			{
				var interval = MathF.Round(Math.Abs(seconds[i + 1] - seconds[i]), 4);
				if (interval > 0)
					intervals.Add(interval);
			}
        }

		~StatisticsService ()
		{
			Contents = null;
			contentSplit = null;
			timeList = null;
			voltageList = null;
		}

		public float GetHighestPeak()
		{
			if (voltageList.Count <= 0)
				return 0f;
			return MathF.Round(voltageList.Max() / maxVal * maxVol, 4);
		}

		public float GetLowestPeak()
		{
			if (voltageList.Count <= 0)
				return 0f;
			return MathF.Round(voltageList.Min() / maxVal * maxVol, 4);
		}

		public float GetAveragePeak()
		{
			if (voltageList.Count <= 0)
				return 0f;
			return MathF.Round(voltageList.Average() / maxVal * maxVol, 4);
		}

		public float GetLongestInterval()
		{
			if (intervals.Count <= 0)
				return 0f;
			return MathF.Round(intervals.Max(), 4);
		}

		public float GetShortestInterval()
		{
			if (intervals.Count <= 0)
				return 0f;
			return MathF.Round(intervals.Min(), 4);
		}

		public float GetAverageInterval()
		{
			if (intervals.Count <= 0)
				return 0f;
			return MathF.Round(intervals.Average(), 4);
		}
		/*
		public List<BasicEntry> GetMaxVoltages ()
		{
			float maxVoltage = voltageList.Max ();

			List <BasicEntry> maxVoltages = contentSplit
											.Where ((tuple, i) => tuple.Item2.Equals (maxVoltage))
											.Select ((tuple, i) => new BasicEntry (tuple.Item2 * 5f / 1024f, tuple.Item1))
											.ToList ();
			
			Logger.LogList (maxVoltages.Select ((entry => entry.ToString ())));
            return maxVoltages;
		}

		public List<BasicEntry> GetMinVoltages ()
		{
			float minVoltage = voltageList.Min ();

            List <BasicEntry> minVoltages = contentSplit
											.Where ((tuple, i) => tuple.Item2.Equals (minVoltage))
											.Select ((tuple, i) => new BasicEntry (tuple.Item2 * 5f / 1024f, tuple.Item1))
											.ToList ();

			Logger.LogList(minVoltages.Select((entry => entry.ToString())));
			return minVoltages;
		}
		
        public float GetAverageVoltage ()
		{
			float ave = voltageList.Average() * 5f / 1024f;
            Logger.Log ($"Average: {ave}");
			return ave;
		}
		
		public List <FrequencyEntry> GetVoltageFrequencyDistribution (float step = 0.25f, float min = 0f, float max = 5f)
		{
			// Round to nearest step size
			List <float> roundedVoltages = new List <float> (voltageList.Capacity);

			// round(voltage / step) * step
			foreach (float voltage in voltageList)
			{
				float roundedVoltage = (float) Math.Round (voltage * 5f / 1024f / step) * step;
				roundedVoltages.Add (roundedVoltage);
			}

//			Logger.LogList(roundedVoltages.Select((voltage, index) => voltage.ToString(CultureInfo.InvariantCulture)).ToList());

            if (roundedVoltages.Count <= 0)
				return null;

			// For every voltage step, bibilangin nya ung mga nasa rounded voltages kung ilan
            List <FrequencyEntry> frequencyEntries = new List <FrequencyEntry> ();
			for (float i = min; i <= max; i += step)
			{
				int frequency = roundedVoltages.Where ((voltage, index) => (int) (voltage * 1024f / 5f) == (int) (i * 1024f / 5f)).Count();
				
				FrequencyEntry frequencyEntry = new FrequencyEntry(frequency, i);
				frequencyEntries.Add (frequencyEntry);
			}

//			Logger.LogList(frequencyEntries.Select((entry, index) => entry.ToString()).ToList());
			
            return (frequencyEntries.Count <= 0) ? null : frequencyEntries;
		}
		*/
	}
}
