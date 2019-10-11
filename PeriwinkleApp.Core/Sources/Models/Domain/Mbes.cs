using System;

namespace PeriwinkleApp.Core.Sources.Models.Domain
{
	public class Mbes
	{
		public int? MbesId { get; set; }
		public int? MbesClientId { get; set; }
		public float? Height { get; set; }
		public float? Weight { get; set; }
		public float? BMI { get; set; }
		public DateTime DateCreated { get; set; }
	}
}