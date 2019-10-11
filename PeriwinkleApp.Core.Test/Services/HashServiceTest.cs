using System.Collections.Generic;
using NUnit.Framework;
using PeriwinkleApp.Core.Sources.Crypto;
using PeriwinkleApp.Core.Sources.Models.Domain;
using PeriwinkleApp.Core.Sources.Services;
using PeriwinkleApp.Core.Sources.Services.Interfaces;
using PeriwinkleApp.Core.Sources.Utils;
using PeriwinkleApp.Core.Test.Utils;

namespace PeriwinkleApp.Core.Test.Services
{
    [TestFixture]
    public class HashServiceTest
    {
        private static string Tag => "HashServiceTest";

        [Test]
        public void GeneratePasswordTest ()
        {
            IHashService hashService = new HashService ();

            Password password = hashService.GenerateHashedPassword ("jaspehehey99");

            // TODO 
            // kunin ung sa passwordservice bla bla
            
            Logger.Debug (password);

            Assert.True (BCrypt.CheckPassword ("jaspehehey99", password.PasswordHash));
            Assert.False (BCrypt.CheckPassword ("jaspehehey9", password.PasswordHash));
            Assert.False (BCrypt.CheckPassword ("jasaehehey99", password.PasswordHash));
            Assert.False (BCrypt.CheckPassword ("Jaspehehey99", password.PasswordHash));
            Assert.False (BCrypt.CheckPassword ("aksdqowieo", password.PasswordHash));
        }

        [Test]
        public void ValidateTestRepositoryPasswordSuccessTest ()
        {
            string plainPassword = null;

            // dapat valid ung password
            Assert.DoesNotThrow (() =>
            {
                plainPassword = TestRepository.GetValidPlainPassword ();
            });
            
            // valid sya kaya hindi sya null
            Assert.NotNull (plainPassword);
        }
        
        
    }
}
