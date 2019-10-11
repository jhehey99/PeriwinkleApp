using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PeriwinkleApp.Core.Sources.Extensions;
using PeriwinkleApp.Core.Sources.Models.Bridge;
using PeriwinkleApp.Core.Sources.Models.Domain;
using PeriwinkleApp.Core.Sources.Models.Response;
using PeriwinkleApp.Core.Sources.Services.Interfaces;

namespace PeriwinkleApp.Core.Sources.Services
{
    public class AccountService : PeriwinkleHttpService, IAccountService
    {
        private new static string Tag => "AccountService";

    #region Public Methods

        public async Task<List <ApiResponse>> RegisterAccount (Account account)
        {
            string url = ApiUri.RegisterAccount.ToUrl ();

            var response = await httpService.PostReadResponse 
                               <IEnumerable <ApiResponse>, Account> (url, account);

            return response.ToList ();
        }

        public async Task<AccountType?> GetAccountType(string username)
        {
            string url = ApiUri.GetAccountTypeByUsername.ToUrl();

            // gawa tayo kvPair para gawing GET Parameters
            var keyVal = new KeyValuePair<string, string>("username", username.ToBase64());

            // GET account from username
            Account account = await httpService.GetWithParams<Account>(url,
                                                                       new List<KeyValuePair<string, string>>()
                                                                       {
                                                                           keyVal
                                                                       });
            
            return account?.AccTypeId;
        }

        public async Task <bool> CheckAccountExists (string username)
        {
            string url = ApiUri.CheckAccountExists.ToUrl ();

            // gawa tayo kvPair para gawing GET Parameters
            var keyVal = new KeyValuePair<string, string>("username", username.ToBase64());

            // GET account from username
            Account account = await httpService.GetWithParams<Account>(url,
                                                                       new List<KeyValuePair<string, string>>()
                                                                       {
                                                                           keyVal
                                                                       });
            
            return account != null;
        }

        public async Task <Account> GetAccountAsSession (string username)
        {
            string url = ApiUri.GetAccountAsSession.ToUrl();

            // gawa tayo kvPair para gawing GET Parameters
            var keyVal = new KeyValuePair<string, string>("username", username.ToBase64());

            // GET account from username
            Account account = await httpService.GetWithParams<Account>(url,
                                                                       new List<KeyValuePair<string, string>>()
                                                                       {
                                                                           keyVal
                                                                       });
            
            return account;
        }

        #endregion

    }
}


