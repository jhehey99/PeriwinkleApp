using System;
using System.Collections.Generic;
using PeriwinkleApp.Core.Sources.CommonInterfaces;
using PeriwinkleApp.Core.Sources.Extensions;

namespace PeriwinkleApp.Core.Sources.Models.Domain
{
	public class BehaviorGraph : IDebugString
	{
		public int? GraphId { get; set; }
		public int? GraphClientId { get; set; }
		public string Filename { get; set; }
		public DateTime StartTime { get; set; }
		public DateTime StopTime { get; set; }
		public TimeSpan Duration { get; set; }
		public float MaxVoltage { get; set; }
		public List<DateTime> MaxVoltageTimes { get; set; }
		public float MinVoltage { get; set; }
		public List <DateTime> MinVoltageTime { get; set; }
		public float AverageVoltage { get; set; }


		public float HighestPeak { get; set; }
		public float LowestPeak { get; set; }
		public float AveragePeak { get; set; }
		public float LongestInterval { get; set; }
		public float ShortestInterval { get; set; }
		public float AverageInterval { get; set; }

		private string jsonString;

        public BehaviorGraph() { }

		public virtual string ToDebug()
		{
			return jsonString ?? (jsonString = this.PrettySerialize());
		}
    }
}
