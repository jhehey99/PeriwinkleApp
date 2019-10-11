using System.Collections.Generic;
using System.Threading.Tasks;
using PeriwinkleApp.Core.Sources.Models.Domain;
using PeriwinkleApp.Core.Sources.Models.Response;

namespace PeriwinkleApp.Core.Sources.Services.Interfaces
{
    public interface IPasswordService
    {
        Task <List <ApiResponse>> RegisterPassword (string username, string plainPassword);
        Task <Password> GetPasswordByUsername (string username);
    }
}
