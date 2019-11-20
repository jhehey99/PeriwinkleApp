using System;
using System.Collections.Generic;
using PeriwinkleApp.Core.Sources.CommonInterfaces;
using PeriwinkleApp.Core.Sources.Extensions;

namespace PeriwinkleApp.Core.Sources.Models.Domain
{
	public class SensorRecord : IDebugString
	{
		public int? RecordId { get; set; }
		public int? ClientId { get; set; }
		public string Filename { get; set; }
		public DateTime StartTime { get; set; }
		public DateTime StopTime { get; set; }

		private string jsonString;

		public SensorRecord() { }

		public virtual string ToDebug()
		{
			return jsonString ?? (jsonString = this.PrettySerialize());
		}
	}
}