using System.IO;
using System.Linq;
using Newtonsoft.Json;
using NUnit.Framework;
using PeriwinkleApp.Core.Sources.Models.Domain;
using PeriwinkleApp.Core.Sources.Models.Response;
using PeriwinkleApp.Core.Sources.Services;
using PeriwinkleApp.Core.Sources.Services.Interfaces;
using PeriwinkleApp.Core.Sources.Utils;

namespace PeriwinkleApp.Core.Test.Services
{
    [TestFixture]
    public class ConsultantServiceTest
    {
        private static string Tag => "ConsultantServiceTest";

        [Test]
        public void RegisterConsultantSuccessTest ()
        {
            // get a valid consultant account
            Consultant consultant = TestRepository.GetValidConsultant ();
            IConsultantService conService = new ConsultantService ();

            // must have a valid consultant
            Assert.NotNull (consultant);
            
            Logger.Debug (consultant, true);

            var task = conService.RegisterConsultant (consultant);
            task.Wait();
            var conResponse = task.Result;
            
            // consultant registration should be successful
            Assert.True (conResponse.Select (conApiResponse => conApiResponse.Code == ApiResponseCode.RegisterSuccess).FirstOrDefault());

            // generate and register new random password
            string plainPassword = TestRepository.GetValidPlainPassword ();
            IPasswordService passwordService = new PasswordService ();

            var passTask = passwordService.RegisterPassword (consultant.Username, plainPassword);
            passTask.Wait ();
            var passResponse = passTask.Result;
            
            // registration should be successful
            Assert.True (passResponse.Select (passApiResponse => passApiResponse.Code == ApiResponseCode.RegisterSuccess).FirstOrDefault());
        }

        [Test]
        public void GetAllConsultantsTestSuccessTest ()
        {
            IConsultantService conService = new ConsultantService ();
            var task = conService.GetAllConsultants ();
            task.Wait ();
            var consultants = task.Result;

            Logger.DebugList (consultants);
            
            Assert.That (consultants.First().GetType () == typeof(Consultant));
        }
        
        [Test]
        public void GetAllPendingConsultantsTestSuccessTest ()
        {
            IConsultantService conService = new ConsultantService ();
            var task = conService.GetAllPendingConsultants ();
            task.Wait ();
            var consultants = task.Result;

            Logger.DebugList (consultants);
            
            Assert.That (consultants.First().GetType () == typeof(Consultant));
        }

        [Test]
        public void GetConsultantByUsernameSuccessTest ()
        {
            // deserialize JSON directly from a file
            string filename = @"C:\Users\Jhehey\RiderProjects\PeriwinkleApp\PeriwinkleApp.Core.Test\Files\Json\GetConsultantByUsernameTest.json";
            Consultant realConsultant = JsonConvert.DeserializeObject<Consultant>(File.ReadAllText(filename));
            
            IConsultantService conService = new ConsultantService ();
            var task = conService.GetConsultantByUsername ("conUser_ykUyCM");
            task.Wait ();
            var getConsultant = task.Result;
            
            // dapat equal ung obtained mula sa GET sa nakasave sa file na actual rin naman tlga
            Assert.True (realConsultant.Equals (getConsultant));
        }

        [Test]
        public void GetConsultantByClientIdSuccessTest ()
        {
            // deserialize JSON directly from a file
            string filename = @"C:\Users\Jhehey\RiderProjects\PeriwinkleApp\PeriwinkleApp.Core.Test\Files\Json\GetConsultantByClientIdTest.json";
            Consultant realConsultant = JsonConvert.DeserializeObject<Consultant>(File.ReadAllText(filename));
            
            IConsultantService conService = new ConsultantService ();
            var task = conService.GetConsultantByClientId (30);
            task.Wait ();
            var getConsultant = task.Result;
            
            // dapat equal ung obtained mula sa GET sa nakasave sa file na actual rin naman tlga
            Assert.True (realConsultant.Equals (getConsultant));
        }
    }
}
