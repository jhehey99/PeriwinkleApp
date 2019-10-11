using System;

namespace PeriwinkleApp.Android.Source.AdapterModels
{
	public class ReactiveAdapterModel
	{
		public string Title { get; set; }
		public string Message { get; set; }

		// bali ang mangyayare, per model, may kanya kanya silang Action Clicked event, para di pare pareho
		public EventHandler ActionClicked { get; set; }

		public ReactiveAdapterModel() { }

		public bool Equals(ReminderAdapterModel other)
		{
			return Title == other.Title && Message == other.Message;
		}
    }
}
