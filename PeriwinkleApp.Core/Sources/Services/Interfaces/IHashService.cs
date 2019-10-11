using PeriwinkleApp.Core.Sources.Models.Domain;

namespace PeriwinkleApp.Core.Sources.Services.Interfaces
{
    public interface IHashService
    {
        Password GenerateHashedPassword (string plainPassword);
        bool VerifyPasswordHash (string inputPassword, Password userPassword);
    }
}
