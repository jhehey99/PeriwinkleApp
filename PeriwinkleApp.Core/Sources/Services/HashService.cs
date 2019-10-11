using PeriwinkleApp.Core.Sources.Crypto;
using PeriwinkleApp.Core.Sources.Models.Domain;
using PeriwinkleApp.Core.Sources.Services.Interfaces;

namespace PeriwinkleApp.Core.Sources.Services
{
    public class HashService : IHashService
    {
        public Password GenerateHashedPassword (string plainPassword)
        {
            string salt = BCrypt.GenerateSalt (12);
            
            string hashed = BCrypt.HashPassword (plainPassword, salt);

            return new Password ()
            {
                PasswordHash = hashed,
                PasswordSalt = salt
            };
        }

        public bool VerifyPasswordHash (string inputPassword, Password userPassword)
        {
            // userPassword is ung makukuha mula sa database
            string inputHashed = BCrypt.HashPassword (inputPassword, userPassword.PasswordSalt);

            return inputHashed == userPassword.PasswordHash;
        }
    }
}
