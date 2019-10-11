using PeriwinkleApp.Core.Sources.Services;
using PeriwinkleApp.Core.Sources.Services.Interfaces;
using PeriwinkleApp.Core.Sources.Utils;

namespace PeriwinkleApp.Core.Test.Usage
{
    public class PasswordUsage
    {
        public void RegisterPassword (string username)
        {
            // generate and register new random password
            string plainPassword = TestRepository.GetValidPlainPassword ();
            IPasswordService passwordService = new PasswordService ();

            var passTask = passwordService.RegisterPassword (username, plainPassword);
            passTask.Wait ();
            var passResponse = passTask.Result;
        }
    }
}
