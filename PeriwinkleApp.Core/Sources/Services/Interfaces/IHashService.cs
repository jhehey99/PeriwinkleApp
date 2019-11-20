using PeriwinkleApp.Core.Sources.Models.Domain;
using System.Threading.Tasks;

namespace PeriwinkleApp.Core.Sources.Services.Interfaces
{
    public interface IHashService
    {
        Password GenerateHashedPassword (string plainPassword);
        bool VerifyPasswordHash (string inputPassword, Password userPassword);
        Task<bool> VerifyPasswordHashAsync (string inputPassword, Password userPassword);
	}
}
