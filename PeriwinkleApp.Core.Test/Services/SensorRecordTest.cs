using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using PeriwinkleApp.Core.Sources.Extensions;
using PeriwinkleApp.Core.Sources.Models.Domain;
using PeriwinkleApp.Core.Sources.Models.Response;
using PeriwinkleApp.Core.Sources.Services;
using PeriwinkleApp.Core.Sources.Services.Interfaces;
using PeriwinkleApp.Core.Sources.Utils;

namespace PeriwinkleApp.Core.Test.Services
{
	[TestFixture]
	public class SensorRecordTest
	{
		private static readonly Random Getrandom = new Random();

		[Test]
		public void AddSensorRecordTest()
		{
			string filename = randAlpha() + ".rec";
			string content = "00:00:00.000,69,0.69,69.99,99.69";

			SensorRecord record = new SensorRecord()
			{
				ClientId = 9,
				Filename = filename,
				StartTime = DateTime.Now,
				StopTime = DateTime.Now
			};

			string url = ApiUri.AddSensorRecord.ToUrl();

			HttpService httpService = new HttpService();
			var task = httpService.PostMultipartFormDataContent
				<IEnumerable<ApiResponse>, SensorRecord>(url, record,content.ToBytesArray(),filename);
			task.Wait();
			var res = task.Result;

			Assert.AreEqual(res.First().Code.ToString(), "UpdateSuccess");
		}
		
		private static string randAlpha(int min = 5, int max = 20)
		{
			var chars = "abcdefghijklmnopqrstuvwxyz";

			int strSize = Getrandom.Next(min, max);
			var stringChars = new char[strSize];

			for (int i = 0; i < strSize; i++)
				stringChars[i] = chars[Getrandom.Next(chars.Length)];

			return new String(stringChars);
		}
	}
}
