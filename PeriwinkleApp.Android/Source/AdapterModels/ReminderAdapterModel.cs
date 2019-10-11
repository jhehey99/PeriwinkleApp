namespace PeriwinkleApp.Android.Source.AdapterModels
{
	public class ReminderAdapterModel
	{
		public string Title { get; set; }
		public string Message { get; set; }
		
		public bool Equals(ReminderAdapterModel other)
		{
			return Title == other.Title && Message == other.Message;
		}
    }
}
