using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Android.App;
using PeriwinkleApp.Android.Source.Session;
using PeriwinkleApp.Core.Sources.Utils;

namespace PeriwinkleApp.Android.Source.Factories
{
    public class SessionFactory
    {
        private static readonly IDictionary<string, AppSession> SessionDictionary;

        static SessionFactory ()
        {
            SessionDictionary = new Dictionary <string, AppSession> ();
        }

        public static T CreateSession<T> (string key = null)
            where T: AppSession, new ()
        {
            // di pwede null ung key
            if (key == null)
                return null;

            bool containsKey = SessionDictionary.ContainsKey(key);

            // key exists
            if (containsKey)
                return SessionDictionary[key] as T;

            // key doesn't exists, we create a new session and add it to the dictionary
            T session = new T ();
            SessionDictionary.Add (key, session);
            return session;
        }

        public static T ReadSession<T> (string key = null)
            where T : AppSession
        {
            // di pwede null ung key, or wala sa dictionary
            if (key == null || !SessionDictionary.ContainsKey(key))
                return null;

            return SessionDictionary[key] as T;
        }

		public static void ClearSession()
		{
			Logger.Log("SessionFactory - ClearSession()");
			foreach(var item in SessionDictionary)
			{
				item.Value.ClearSession();
			}
			SessionDictionary.Clear();
		}
        
    }
}
