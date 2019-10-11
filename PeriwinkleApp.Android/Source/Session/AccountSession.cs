using Android.App.Admin;
using Android.Content;
using PeriwinkleApp.Core.Sources.Models.Domain;
using PeriwinkleApp.Core.Sources.Utils;

namespace PeriwinkleApp.Android.Source.Session
{
    public class AccountSession : AppSession
    {
        protected const string KeyAccountId = "kAccId";
        protected const string KeyUsername = "kUsern";
        protected const string KeyAccountType = "kAccType";

        public int? AccountId => GetInt (KeyAccountId);
        
        public string Username => GetString (KeyUsername);

        public AccountType? AccountType => (AccountType?) GetInt (KeyAccountType);

        public AccountSession() { }

        public AccountSession (Context context) : base (context) { }

        public void AddAccountSession (int? accountId = null, 
                                       string username = null, 
                                       AccountType? accountType = null)
        {
            // pag hindi null, saka natin ilalagay sa editor
            // para pag kinuha ung key, tas walang value, null naman sya

            PutInt (KeyAccountId, accountId);
            PutString (KeyUsername, username);
            PutInt(KeyAccountType, (int?) accountType);

            Editor.Commit ();

            IsSet = true;
        }

        public void AddAccountSession (Account account)
        {
            PutInt (KeyAccountId, account.AccountId);
            PutString(KeyUsername, account.Username);
            PutInt(KeyAccountType, (int?) account.AccTypeId);

            Editor.Commit();

            IsSet = true;
        }

        public override void ClearSession ()
        {
            Logger.Log("AccountSession - ClearSession");

            Remove(KeyAccountId);
            Remove(KeyUsername);
            Remove(KeyAccountType);

            Editor.Commit ();
            
            IsSet = false;
        }
    }
}
