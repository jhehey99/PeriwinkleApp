namespace PeriwinkleApp.Core.Sources.Models.Common
{
	public class BasicEntry
	{
		public float Voltage { get; set; }
		public string Time { get; set; }

		public BasicEntry (float voltage, string time)
		{
			Voltage = voltage;
			Time = time;
		}

		public override string ToString ()
		{
			return $"Time: {Time} | Voltage: {Voltage}";
		}
	}
}
