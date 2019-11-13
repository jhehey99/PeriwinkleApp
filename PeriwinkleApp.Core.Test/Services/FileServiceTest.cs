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
using PeriwinkleApp.Core.Sources.Utils;

namespace PeriwinkleApp.Core.Test.Services
{
	[TestFixture]
	public class FileServiceTest
    {
		[Test]
		public void FileTest ()
		{
			FileService fileService = new FileService ("graphs");
			string filename = "graph1.pg";
			string content = "Hello sa mama mo";

			Task saveTask = fileService.WriteAsync (filename, content);
			saveTask.Wait ();

			Task<string> readTask = fileService.ReadToEndAsStringAsync (filename);
			readTask.Wait ();
			string readContent = readTask.Result;

			byte[] bytes = readContent.ToBytesArray ();

			Assert.AreEqual (content, readContent);
		}

		[Test]
		public void BehaviorGraphFileWithHttpTest()
		{
			FileService fileService = new FileService(FileDirectory.Graph);
			string filename = "graph12.pbg";
			string content = "Hello sa mama mo";

			string url = ApiUri.AddBehaviorGraph.ToUrl ();

			HttpService httpService = new HttpService ();
			var task = httpService.PostMultipartFormDataContent<IEnumerable<ApiResponse>, BehaviorGraph>(url,
																									new BehaviorGraph()
																									{
																										GraphClientId = 9,
																										Filename = filename
																									},
																									content.ToBytesArray(),
																									filename);
			task.Wait();
			var res = task.Result;

			Assert.AreEqual(res.First().Code.ToString(), "UpdateSuccess");

			Logger.Log ("hehe");
		}

		[Test]
		public void AccelerometerRecordFileWithHttpTest()
		{
			FileService fileService = new FileService(FileDirectory.Accelerometer);
			string filename = "accell4.par";
			string content = "Hello sa mama mo acceleration ng papa mo";

			string url = ApiUri.AddAccelerometerRecord.ToUrl();

			HttpService httpService = new HttpService();
			var task = httpService.PostMultipartFormDataContent<IEnumerable<ApiResponse>, AccelerometerRecord>(url,
																									new AccelerometerRecord()
																									{
																										ClientId = 9,
																										Filename = filename
																									},
																									content.ToBytesArray(),
																									filename);
			task.Wait();
			var res = task.Result;

			Assert.AreEqual(res.First().Code.ToString(), "UpdateSuccess");

			Logger.Log("hehe");
		}

		[Test]
		public void FileAppendTest ()
		{
			FileService fileService = new FileService("graphs");

			string filename = "hehe.txt";
			string content = "hello\nhi\n";

			
			Task writeTask = fileService.WriteLineAppendAsync(filename, content);
			writeTask.Wait ();

			Task writeTask2 = fileService.WriteLineAppendAsync(filename, content);
			writeTask2.Wait();
        }


    }
}
