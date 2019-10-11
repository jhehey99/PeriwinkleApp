using Android.Content;
using PeriwinkleApp.Core.Sources.Models.Domain;
using PeriwinkleApp.Core.Sources.Utils;

namespace PeriwinkleApp.Android.Source.Session
{
    public class ConsultantSession : AccountSession
    {
        protected const string KeyConsultantId = "kConId";

        public int? ConsultantId => GetInt(KeyConsultantId);

        public ConsultantSession () { }

        public ConsultantSession (Context context) : base (context) { }

        public void AddConsultantSession(int? consultantId = null,
                                     int? accountId = null,
                                     string username = null,
                                     AccountType? accountType = null)
        {
            PutInt(KeyConsultantId, consultantId);
            AddAccountSession(accountId, username, accountType);
        }

        public void AddConsultantSession (Consultant consultant)
        {
            PutInt (KeyConsultantId, consultant.ConsultantId);
            AddAccountSession (consultant);
        }

        public override void ClearSession()
        {
            Logger.Log("ConsultantSession - ClearSession");
            Remove(KeyConsultantId);
            base.ClearSession();
        }
    }
}
