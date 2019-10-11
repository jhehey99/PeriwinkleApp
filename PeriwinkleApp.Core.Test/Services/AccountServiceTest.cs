using System.Linq;
using NUnit.Framework;
using PeriwinkleApp.Core.Sources.Models.Domain;
using PeriwinkleApp.Core.Sources.Models.Response;
using PeriwinkleApp.Core.Sources.Services;
using PeriwinkleApp.Core.Sources.Services.Interfaces;
using PeriwinkleApp.Core.Sources.Utils;

namespace PeriwinkleApp.Core.Test.Services
{
    [TestFixture]
    public class AccountServiceTest
    {
        private static string Tag => "AccountServiceTest";

//        [Test]
//        public void GetTestAccountByUsernameSuccessTest ()
//        {
//            // use the "test" account
//            AccountService accountService = new AccountService ();
//            var task = accountService.GetAccountByUsername ("test");
//            task.Wait ();
//            var account = task.Result;
//
//            Logger.Debug (account, true);
//            
//            Assert.That (() => account.AccountId == 1 && 
//                               account.Username == "test" &&
//                               account.FirstName == "test" &&
//                               account.LastName == "test" &&
//                               account.Email == "test@gmail.com" &&
//                               account.AccTypeId == AccountType.Admin);
//        }
//
//        [Test]
//        public void GetAccountByUsernameTest ()
//        {
//            AccountService accountService = new AccountService ();
//            var task = accountService.GetAccountByUsername ("user_ypZEOFQbLvBbBxmJpN");
//            task.Wait ();
//            var account = task.Result;
//
//            Logger.Debug (account, true);
//            
//            Assert.That (() => account.AccountId == 31 && 
//                               account.Username == "user_ypZEOFQbLvBbBxmJpN" &&
//                               account.FirstName == "first_VlGCxtUqCbTĘŃ" &&
//                               account.LastName == "last_VlGCxtUqCbTĘŃ" &&
//                               account.Email == "first_lastypZEOFQbLvBbBxmJpN@gmail.com" &&
//                               account.AccTypeId == AccountType.Admin);
//        }
//        
//        [Test]
//        public void RegisterPasswordForTestAccountTest ()
//        {
//            // use the "test" account
//            AccountService accountService = new AccountService ();
//            var accTask = accountService.GetAccountByUsername ("test");
//            accTask.Wait ();
//            var account = accTask.Result;
//            
//            // register tayo sa password ni test account
//            var passTask = accountService.RegisterPassword (account.Username, "jaspehehey99");
//            passTask.Wait ();
//            var response = passTask.Result;
//            
//            Assert.True (response.Select (r => r.Code == ApiResponseCode.RegisterSuccess).FirstOrDefault());
//            Assert.True (true);
//        }
//
////        [Test]
//        public void RegisterNewAccountWithPasswordSuccessTest ()
//        {
//            // generate and register new random account
//            Account account = TestRepository.GetValidAccount ();
//            IAccountService accountService = new AccountService ();
//            
//            // must have a valid account
//            Assert.NotNull (account);
//            
//            Logger.Debug (account, true, Tag);
//
//            var accTask = accountService.RegisterAccount (account);
//            accTask.Wait ();
//            var accResponse = accTask.Result;
//
//            // account registration should be successful
//            Assert.True (accResponse.Select (accApiResponse => accApiResponse.Code == ApiResponseCode.RegisterSuccess).FirstOrDefault());
//
//            // generate and register new random password
//            string plainPassword = TestRepository.GetValidPlainPassword ();
//            IPasswordService passwordService = new PasswordService ();
//
//            var passTask = accountService.RegisterPassword (account.Username, plainPassword);
//            passTask.Wait ();
//            var passResponse = passTask.Result;
//            
//            // registration should be successful
//            Assert.True (passResponse.Select (passApiResponse => passApiResponse.Code == ApiResponseCode.RegisterSuccess).FirstOrDefault());
//        }
        
    }
}
