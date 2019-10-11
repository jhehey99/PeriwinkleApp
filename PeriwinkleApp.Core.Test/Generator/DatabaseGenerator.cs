using System.Linq;
using NUnit.Framework;
using PeriwinkleApp.Core.Sources.Models.Domain;
using PeriwinkleApp.Core.Sources.Models.Response;
using PeriwinkleApp.Core.Sources.Services;
using PeriwinkleApp.Core.Sources.Services.Interfaces;
using PeriwinkleApp.Core.Sources.Utils;

namespace PeriwinkleApp.Core.Test.Generator
{
    [TestFixture]
    public class DatabaseGenerator
    {
		private IConsultantService conService;
		private IClientService cliService;
		private IPasswordService passService;
		string plainPassword = "123";

        
		int count = 6;

        [Test]
		public void GeneratePendingConsultants ()
		{
			InitServices();

			for (int i = 0; i < count; i++)
			{
				string username = RegisterPendingConsultant ().Username;
                RegisterPassword (username);
			}
		}
		
        [Test]
		public void GenerateAcceptedConsultantsAndClients ()
		{
			InitServices();

			// 2 consultants
			// 5 clients each consultant
			
			for (int i = 0; i < 2; i++)
			{
				// Consultant
				Consultant con = RegisterPendingConsultant ();
				string username = con.Username;
				RegisterPassword(username);
				AcceptConsultant (username);

				for (int j = 0; j < 5; j++)
				{
					// Client
					Client cli = RegisterClient (con);
					username = cli.Username;
					RegisterPassword (username);
				}
			}
        }




#region Generator Functions

        public void InitServices()
		{
			conService = new ConsultantService();
			cliService = new ClientService();
			passService = new PasswordService();
		}

		public Consultant RegisterPendingConsultant()
		{
			// consultant
			Consultant consultant = TestRepository.GetValidConsultant();
			var task = conService.RegisterConsultant(consultant);
			task.Wait();
			var responses = task.Result;

			Assert.True(responses.Select(r => r.Code == ApiResponseCode.RegisterSuccess).FirstOrDefault());
			return consultant;
		}

		public void RegisterPassword(string username)
		{
			var task = passService.RegisterPassword(username, plainPassword);
			task.Wait();
			var responses = task.Result;

			Assert.True(responses.Select(r => r.Code == ApiResponseCode.RegisterSuccess).FirstOrDefault());
		}

		public void AcceptConsultant(string username)
		{
			var task = conService.ValidatePendingConsultant(username, true);
			task.Wait();
			var responses = task.Result;

			Assert.True(responses.Select(r => r.Code == ApiResponseCode.UpdateSuccess).FirstOrDefault());
		}

		public Client RegisterClient(Consultant consultant)
		{
			Client client = TestRepository.GetValidClient();
			var task = cliService.RegisterClientToConsultant(client, consultant);
			task.Wait();
			var responses = task.Result;

			Assert.True(responses.Select(r => r.Code == ApiResponseCode.RegisterSuccess).FirstOrDefault());
			return client;
		}

#endregion
    }
}
