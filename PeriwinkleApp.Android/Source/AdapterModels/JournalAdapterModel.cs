using System;

namespace PeriwinkleApp.Android.Source.AdapterModels
{
	public class JournalAdapterModel
	{
		public string Title { get; set; }
		public DateTime DateCreated { get; set; }
		public EventHandler<int> ViewJournalClicked { get; set; }
    }
}
