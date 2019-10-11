namespace PeriwinkleApp.Core.Sources.Models.Common
{
	public class FrequencyEntry
	{
		public int Frequency { get; set; }
		public float Voltage { get; set; }

		public FrequencyEntry (int frequency, float voltage)
		{
			Frequency = frequency;
			Voltage = voltage;
        }

		public override string ToString()
		{
			return $"Frequency: {Frequency} | Voltage: {Voltage}";
		}
    }
}
