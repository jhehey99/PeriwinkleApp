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
		public void AddPiezoRecordTest()
		{
			string filename = randAlpha() + ".pbg";
			string content = "Hello sa mama mo piezo ng papa mo";

			SensorRecord record = new SensorRecord()
			{
				ClientId = 9,
				RecordType = SensorRecordType.Piezo,
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
		
		[Test]
		public void AddAccelerationRecordTest()
		{
			string filename = randAlpha() + ".par";
			string content = "Hello sa mama mo acceleration ng papa mo";

			SensorRecord record = new SensorRecord()
			{
				ClientId = 9,
				RecordType = SensorRecordType.Acceleration,
				Filename = filename,
				StartTime = DateTime.Now,
				StopTime = DateTime.Now
			};

			string url = ApiUri.AddSensorRecord.ToUrl();

			HttpService httpService = new HttpService();
			var task = httpService.PostMultipartFormDataContent
				<IEnumerable<ApiResponse>, SensorRecord>(url, record, content.ToBytesArray(), filename);
			task.Wait();
			var res = task.Result;

			Assert.AreEqual(res.First().Code.ToString(), "UpdateSuccess");
		}

		[Test]
		public void GetRecordsTest()
		{
			IClientService cliService = new ClientService();

			var task = cliService.GetSensorRecordByClientId(9);
			task.Wait();

			var res = task.Result;

			bool piezo = false;
			foreach (var record in res)
			{
				if (record.RecordType == SensorRecordType.Piezo || record.RecordType == SensorRecordType.Acceleration)
					piezo = true;
				else
					piezo = false;
			}

			Assert.AreEqual(piezo, true);
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
