using System;

namespace PeriwinkleApp.Android.Source.AdapterModels
{
	public class NotificationAdapterModel
	{
		public string Title { get; set; }
		public string Message { get; set; }

		// bali ang mangyayare, per model, may kanya kanya silang Action Clicked event, para di pare pareho
		public EventHandler ActionClicked { get; set; }

		public bool HasAction { get; set; }

		public NotificationAdapterModel() { }

		public bool Equals(ReminderAdapterModel other)
		{
			return Title == other.Title && Message == other.Message;
		}
    }
}
