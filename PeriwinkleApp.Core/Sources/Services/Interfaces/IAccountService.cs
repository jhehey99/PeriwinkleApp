using System.Collections.Generic;
using System.Threading.Tasks;
using PeriwinkleApp.Core.Sources.Models.Domain;
using PeriwinkleApp.Core.Sources.Models.Response;

namespace PeriwinkleApp.Core.Sources.Services.Interfaces
{
    public interface IAccountService
    {
        Task<List <ApiResponse>> RegisterAccount (Account account);
        Task <AccountType?> GetAccountType (string username);
        Task <bool> CheckAccountExists (string username);
        Task <Account> GetAccountAsSession (string username);
    }
}
