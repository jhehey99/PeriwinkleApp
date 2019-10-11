using Android.Content;
using PeriwinkleApp.Core.Sources.Models.Domain;
using PeriwinkleApp.Core.Sources.Utils;

namespace PeriwinkleApp.Android.Source.Session
{
    public class ClientSession : AccountSession
    {
        protected const string KeyClientId = "kCliId";

        public int? ClientId => GetInt(KeyClientId);

        public ClientSession() { }

        public ClientSession (Context context) : base (context) { }


        public void AddClientSession (int? clientId = null, 
                                     int? accountId = null,
                                     string username = null,
                                     AccountType? accountType = null)
        {
            PutInt (KeyClientId, clientId);
            AddAccountSession (accountId, username, accountType);
        }

        public void AddClientSession (Client client)
        {
            PutInt (KeyClientId, client.ClientId);
            AddAccountSession (client);
        }

        public override void ClearSession ()
        {
            Logger.Log ("ClientSession - ClearSession");
            Remove (KeyClientId);
            base.ClearSession ();
        }
    }
}
