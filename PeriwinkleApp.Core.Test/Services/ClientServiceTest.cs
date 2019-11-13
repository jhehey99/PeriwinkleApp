using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using NUnit.Framework;
using PeriwinkleApp.Core.Sources.Models.Domain;
using PeriwinkleApp.Core.Sources.Models.Response;
using PeriwinkleApp.Core.Sources.Services;
using PeriwinkleApp.Core.Sources.Services.Interfaces;
using PeriwinkleApp.Core.Sources.Utils;
using PeriwinkleApp.Core.Sources.Extensions;
using PeriwinkleApp.Core.Test.Helpers;
// ReSharper disable InconsistentNaming

namespace PeriwinkleApp.Core.Test.Services
{
    [TestFixture]
    public class ClientServiceTest
    {
        private static string Tag => "ClientServiceTest";

        private static ITestHelper _testHelper;
        private static ITestHelper testHelper => _testHelper ?? (_testHelper = new TestHelper ());
        
        [Test]
        public void RegisterClientSuccessTest ()
        {
            Client client = TestRepository.GetValidClient ();
            IClientService cliService = new ClientService ();

            // must have a valid client
            Assert.NotNull (client);
            
            Logger.Debug (client, true);

            var task = cliService.RegisterClient (client);
            task.Wait ();
            var cliResponse = task.Result;
            
            // client registration should be successful
            Assert.True (cliResponse.Select (cliApiResponse => cliApiResponse.Code == ApiResponseCode.RegisterSuccess).FirstOrDefault());

            // generate and register new random password
            string plainPassword = TestRepository.GetValidPlainPassword ();
            IPasswordService passwordService = new PasswordService ();

            var passTask = passwordService.RegisterPassword (client.Username, plainPassword);
            passTask.Wait ();
            var passResponse = passTask.Result;
            
            // registration should be successful
            Assert.True (passResponse.Select (passApiResponse => passApiResponse.Code == ApiResponseCode.RegisterSuccess).FirstOrDefault());
        }

        [Test]
        public void RegisterClientToConsultantSuccessTest ()
        {
            // consultant registration should be successful
            (Consultant consultant, List<ApiResponse> regConResponses) = testHelper.RegisterConsultant ();
            Assert.True (regConResponses.Select (apiResponse => apiResponse.Code == ApiResponseCode.RegisterSuccess).FirstOrDefault());

            // consultant password registration should be successful
            IEnumerable <ApiResponse> conPassResponses = testHelper.RegisterRandomValidPassword (consultant.Username);
            Assert.True (conPassResponses.Select (passApiResponse => passApiResponse.Code == ApiResponseCode.RegisterSuccess).FirstOrDefault());

            // client registration should be successful
            (Client client, List<ApiResponse> regCliResponses) = testHelper.RegisterConsultantClient (consultant);
            Assert.True (regCliResponses.Select (apiResponse => apiResponse.Code == ApiResponseCode.RegisterSuccess).FirstOrDefault());
            
            // client password registration should be successful
            IEnumerable <ApiResponse> cliPassResponses = testHelper.RegisterRandomValidPassword (client.Username);
            Assert.True (cliPassResponses.Select (passApiResponse => passApiResponse.Code == ApiResponseCode.RegisterSuccess).FirstOrDefault());

            // must have a valid client and consultant
            Assert.NotNull (client);
            Assert.NotNull (consultant);
            
            Logger.Debug (client, true);
            Logger.Debug (consultant, true);
        }

        [Test]
        public void GetAllClientsSuccessTest ()
        {
            IClientService clientService = new ClientService ();

            var task = clientService.GetAllClients ();
            task.Wait ();
            var clients = task.Result;

            Logger.DebugList (clients);
            
            Assert.That (clients.First().GetType () == typeof(Client));
        }

        //[Test]
        public void GetClientByUsernameTest ()
        {
            // deserialize JSON directly from a file
            string filename = @"C:\Users\Jhehey\RiderProjects\PeriwinkleApp\PeriwinkleApp.Core.Test\Files\Json\GetClientByUsername.json";
            Consultant realClient = JsonConvert.DeserializeObject<Consultant>(File.ReadAllText(filename));

            
            IClientService clientService = new ClientService ();

            var task = clientService.GetClientByUsername ("cliUser_vbTMFOKgagBlxWjmtdV");
            task.Wait ();
            var getClient = task.Result;

            Logger.Debug (getClient, true);
            
            Assert.That (getClient.GetType () == typeof(Client));
            Assert.True (realClient.Equals (getClient));
        }

        //[Test]
        public void GetClientsByClientIdTest ()
        {
            IClientService clientService = new ClientService ();
            var task = clientService.GetClientsByConsultantId (22);
            task.Wait ();
            var clients = task.Result;
            
            Logger.DebugList (clients);

            var client25 = clients.FirstOrDefault(c => c.ClientId == 25);
            Assert.NotNull (client25);
            Assert.True (client25.ClientId == 25);
        }

    }
}
