namespace PeriwinkleApp.Core.Sources.Models.Common
{
	public class ChartStat
	{
		// all in string so display nlng problem
		public string StartTime { get; set; }
		public string StopTime { get; set; }
		public string Duration { get; set; }
		/*
		public string MaxValue { get; set; }
		public string MinValue { get; set; }
		public string AveValue { get; set; }
		*/
		public string HighestPeak { get; set; }
		public string LowestPeak { get; set; }
		public string AveragePeak { get; set; }
		public string LongestInterval { get; set; }
		public string ShortestInterval { get; set; }
		public string AverageInterval { get; set; }
	}
}
