using System;

namespace PeriwinkleApp.Android.Source.AdapterModels
{
	public class BehaviorAdapterModel
	{
		public string Filename { get; set; }
		public DateTime StartTime { get; set; }
		public DateTime StopTime { get; set; }
		public EventHandler<int> ViewReportClicked { get; set; }
		public int Position { get; set; }
    }
}
