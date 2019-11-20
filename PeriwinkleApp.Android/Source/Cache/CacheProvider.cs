using System;
using System.Collections.Generic;
using Microsoft.Extensions.Caching.Memory;
using PeriwinkleApp.Core.Sources.Utils;

namespace PeriwinkleApp.Android.Source.Cache
{
	public static class CacheKey
	{
		public static string LoggedClient { get; } = "CKClient";
		public static string LoggedConsultant { get; } = "CKConsul";
		public static string BackFragItem { get; } = "CKbfi";
		public static string ViewChartContents { get; } = "CKvcc";
		public static string MbesToAdd { get; } = "MBtadd";
    }

	public static class CacheProvider
	{
		private static readonly IMemoryCache Cache;

		private static readonly List <string> Keys;

		static CacheProvider ()
		{
			Cache = new MemoryCache (new MemoryCacheOptions ());
			Keys = new List <string> ();
			
		}

		public static void Set <T> (string key, T value)
		{
			if (!Keys.Contains(key))
				Keys.Add(key);

            Cache.Set (key, value);
		}
		
		public static T Get <T> (string key)
		{
			return Cache.TryGetValue (key, out T value) ? value : default (T);
		}

		public static bool IsSet (string key)
		{
			return Keys.Contains (key);
		}
		public static void Clear()
		{
			Logger.Log("CacheProvider - Clear()");
			Cache.Dispose();
		}
	}
}
