
using System;

namespace PeriwinkleApp.Android.Source.AdapterModels
{
	public class ResponseAdapterModel
	{
		public string Date { get; set; }
		public string BMI { get; set; }
		public EventHandler<int> ViewMbeClicked { get; set; }
	}
}