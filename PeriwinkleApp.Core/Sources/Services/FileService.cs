using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using PeriwinkleApp.Core.Sources.Utils;

namespace PeriwinkleApp.Core.Sources.Services
{
	public enum FileExtension
	{
		Graph, Journal, Accelerometer
	}

	public enum FileDirectory
	{
		Graph, Journal, Accelerometer
	}

	public interface IFileService
	{
		Task WriteAsync (string fileName, string fileContent);
		Task WriteLineAppendAsync (string fileName, string fileContent);
        Task <string> ReadToEndAsStringAsync (string fileName);
		bool DoesFileExist (string fileName);
	}

	public class FileService : IFileService
	{
		//TODO: data/data/PeriwinkleApp.Android.PeriwinkleApp.Android/files/graphs
		//TODO: Andito ung file haha
		public Environment.SpecialFolder FolderPath { get; set; }

		private string pathDirectory;

		public string PathDirectory
		{
			get => pathDirectory;
			set
			{
				pathDirectory = value;
				CreateDirectoryIfNotExist (pathDirectory);
			}
		}

#region Constructors

		public FileService ()
		{
			FolderPath = Environment.SpecialFolder.Personal;
		}

		public FileService (string directory = null)
		{
			FolderPath = Environment.SpecialFolder.Personal;
			PathDirectory = directory;
			Logger.Log (GetBackingDirectory (PathDirectory));
		}

		public FileService(FileDirectory directory)
		{
			FolderPath = Environment.SpecialFolder.Personal;
			PathDirectory = periwinkleDirectories[directory];
			Logger.Log(GetBackingDirectory(PathDirectory));
		}

        #endregion

        #region IFileService

        public async Task WriteAsync (string fileName = null, string fileContent = null)
		{
			if (fileName == null)
				throw new ArgumentNullException (nameof (fileName));

			string backingFile = GetBackingFile (fileName);

//			Logger.Log("WRITE " + backingFile);

			using (StreamWriter writer = File.CreateText (backingFile))
				await writer.WriteAsync (fileContent);
		}

		public async Task WriteLineAppendAsync (string fileName = null, string fileContent = null)
		{
			if (fileName == null)
				throw new ArgumentNullException (nameof (fileName));

			string backingFile = GetBackingFile (fileName);

			//			Logger.Log("WRITE " + backingFile);

			using (StreamWriter writer = new StreamWriter (path: backingFile, append: true))
				await writer.WriteLineAsync (fileContent);
		}

		public async Task <string> ReadToEndAsStringAsync (string fileName = null)
		{
			if (fileName == null)
				throw new ArgumentNullException (nameof (fileName));

			string backingFile = GetBackingFile (fileName);

//			Logger.Log("READ " + backingFile);

			// pag walang file, null string nalang
			if (!File.Exists (backingFile))
				return null;

			string fileContent = null;

			using (StreamReader reader = new StreamReader (backingFile, true))
				fileContent = await reader.ReadToEndAsync ();

			return fileContent;
		}

		public bool DoesFileExist (string fileName = null)
		{
			if (fileName == null)
				throw new ArgumentNullException(nameof(fileName));

			string backingFile = GetBackingFile(fileName);
			return File.Exists (backingFile);
		}

        #endregion

        protected void CreateDirectoryIfNotExist (string directory = null)
		{
			string backingDir = GetBackingDirectory (directory);
			if (!Directory.Exists (backingDir))
				Directory.CreateDirectory (backingDir);
		}

		private string GetBackingFile (string fileName)
		{
			// generates the whole path + specific directory + filename
			return Path.Combine (Environment.GetFolderPath (FolderPath), $"{PathDirectory}/{fileName}");
		}

		private string GetBackingDirectory (string directory)
		{
			// generates the whole path + specific directory
			return Path.Combine (Environment.GetFolderPath (FolderPath), directory);
		}


		public static string GenerateFilename (string prefix, string username, FileExtension extension)
		{
			DateTime now = DateTime.Now;
			string datetime = $"{now.Month}-{now.Day}-{now.Year}_{now.Hour}{now.Minute}{now.Second}";
			return
				$"{prefix}_{username}_{datetime}.{periwinkleExtensions[extension]}";
		}

		private static Dictionary <FileExtension, string> periwinkleExtensions =
			new Dictionary <FileExtension, string> ()
			{
				{FileExtension.Graph, "pbg"},
				{FileExtension.Journal, "pj"},
				{FileExtension.Accelerometer, "par"},
			};

		private static Dictionary<FileDirectory, string> periwinkleDirectories =
			new Dictionary<FileDirectory, string>()
			{
				{FileDirectory.Graph, "graphs"},
				{FileDirectory.Journal, "journals"},
				{FileDirectory.Accelerometer, "accel"}
			};

		public static string GetFileDirectory(FileDirectory dir)
		{
			return periwinkleDirectories[dir];
		}
	}
}
