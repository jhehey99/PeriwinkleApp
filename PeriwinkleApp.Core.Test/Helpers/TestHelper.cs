using System.Collections.Generic;
using PeriwinkleApp.Core.Sources.Models.Domain;
using PeriwinkleApp.Core.Sources.Models.Response;
using PeriwinkleApp.Core.Sources.Services;
using PeriwinkleApp.Core.Sources.Services.Interfaces;
using PeriwinkleApp.Core.Sources.Utils;

namespace PeriwinkleApp.Core.Test.Helpers
{
    public class TestHelper : ITestHelper
    {
        public (Consultant, List <ApiResponse>) RegisterConsultant (Consultant consultant = null)
        {
            consultant = consultant ?? TestRepository.GetValidConsultant ();
            IConsultantService consultantService = new ConsultantService ();

            var task = consultantService.RegisterConsultant (consultant);
            task.Wait ();
            var response = task.Result;

            return (consultant, response);
        }
        
        public (Client, List <ApiResponse>) RegisterConsultantClient (Consultant consultant, Client client = null)
        {
            client = client ?? TestRepository.GetValidClient ();
            IClientService cliService = new ClientService ();
            
            // register na ung client sa given consultant
            var task = cliService.RegisterClientToConsultant (client, consultant);
            task.Wait ();
            var response = task.Result;
            
            return (client, response);
        }
        
        public IEnumerable <ApiResponse> RegisterRandomValidPassword (string username)
        {
            // generate and register new random password
            string plainPassword = TestRepository.GetValidPlainPassword ();
            IPasswordService passwordService = new PasswordService ();

            var passTask = passwordService.RegisterPassword (username, plainPassword);
            passTask.Wait ();
            var passResponse = passTask.Result;

            return passResponse;
        }
    }
}
