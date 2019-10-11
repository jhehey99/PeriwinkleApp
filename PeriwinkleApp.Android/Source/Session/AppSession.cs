using System;
using System.Collections.Generic;
using System.Timers;
using Android.App;
using Android.Content;
using Android.Preferences;
using PeriwinkleApp.Core.Sources.Utils;

namespace PeriwinkleApp.Android.Source.Session
{
    public abstract class AppSession
    {
        protected ISharedPreferences Preferences;
        protected ISharedPreferencesEditor Editor;
        public Context Context;

        // para tuwing gagawa ng session, magkakaroon ng unique
        // na idadagdag sa preferences for unique-ing
        private static int sessionId;
        protected int SessionId;
        
        // ginagamit sa mga read then display, para malaman kung na-set bago i-display or null hehe

		public bool IsSet { get; protected set; }

        protected AppSession(Context context)
        {
            this.Context = context;
            Preferences = PreferenceManager.GetDefaultSharedPreferences(this.Context);
            Editor = Preferences.Edit();
            SessionId = sessionId++;
        }

        protected AppSession ()
        {
            // eto ung ginamit ng session factory, sa session factory i-seset through initializer
            Context = Application.Context;
            Preferences = PreferenceManager.GetDefaultSharedPreferences(this.Context);
            Editor = Preferences.Edit();
            SessionId = sessionId++;
        }
        
        public virtual void ClearSession ()
        {
            // pag sa AppSession mo cinlear, ung buo mawawala
            // pag sa derived, ung sarili lng nila
            Logger.Log("AppSession - ClearSession");

            Editor.Clear();
            Editor.Commit();

            IsSet = false;
        }

		protected void PutInt (string key, int? value)
        {
            if (value != null)
                Editor.PutInt (key + SessionId, (int) value);
        }
        
        protected void PutString (string key, string value)
        {
            if (value != null)
                Editor.PutString (key + SessionId, value);
        }

        protected int? GetInt (string key)
        {
            int? id = Preferences.GetInt(key + SessionId, -1);
            return (id != -1) ? id : null;
        }

        protected string GetString (string key)
        {
            return Preferences.GetString (key + SessionId, null);
        }

        protected void Remove (string key)
        {
            Editor.Remove (key + SessionId);
        }

    }
}
