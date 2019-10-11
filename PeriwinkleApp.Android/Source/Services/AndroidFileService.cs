using System;
using System.IO;
using System.Threading.Tasks;
using PeriwinkleApp.Core.Sources.Utils;

namespace PeriwinkleApp.Android.Source.Services
{
	public class AndroidFileService
	{
		public async void SaveFile ()
		{
			string fileContent = "hello hehehe";

			string backingFile = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "count.txt");
			using (StreamWriter writer = File.CreateText(backingFile))
			{
				await writer.WriteAsync (fileContent);
			}
        }
		
		public async Task<string> ReadFile ()
		{
			string backingFile = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "count.txt");

            if (!File.Exists(backingFile))
                return null;

			string content = null;

            using (StreamReader reader = new StreamReader(backingFile, true))
				content = await reader.ReadToEndAsync ();
			
			Logger.Log (content);

			return content;
		}
    }
}
