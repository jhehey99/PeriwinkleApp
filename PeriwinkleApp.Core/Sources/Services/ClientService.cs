using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PeriwinkleApp.Core.Sources.Extensions;
using PeriwinkleApp.Core.Sources.Models.Bridge;
using PeriwinkleApp.Core.Sources.Models.Domain;
using PeriwinkleApp.Core.Sources.Models.Response;
using PeriwinkleApp.Core.Sources.Services.Interfaces;

namespace PeriwinkleApp.Core.Sources.Services
{
    public class ClientService : PeriwinkleHttpService, IClientService 
    {
        private new static string Tag => "ClientService";
        
        public async Task <List <ApiResponse>> RegisterClient (Client client)
        {
            string url = ApiUri.RegisterClient.ToUrl ();

            var response = await httpService.PostReadResponse
                               <IEnumerable <ApiResponse>, Client> (url, client);

            return response.ToList ();
        }

        public async Task <List <ApiResponse>> RegisterClientToConsultant (Client client, Consultant consultant)
        {
            string url = ApiUri.RegisterClientToConsultant.ToUrl ();

            var response = await httpService.PostReadResponse
                               <IEnumerable <ApiResponse>, ConsultantClient>
                               (url,
                                new ConsultantClient ()
                                {
                                    CcConsultant = consultant,
                                    CcClient = client
                                });
            
            return response.ToList ();
        }

        public async Task <List <ApiResponse>> UpdateHeightWeight (Client client)
        {
            string url = ApiUri.UpdateHeightWeight.ToUrl ();

            var response = await httpService.PostReadResponse
                               <IEnumerable <ApiResponse>, Client> (url, client);

            return response.ToList ();
        }

		public async Task <List <ApiResponse>> AddMbesAttemptCount (Client client)
		{
			string url = ApiUri.AddMbesAttemptCount.ToUrl ();

			var response = await httpService.PostReadResponse
							   <IEnumerable <ApiResponse>, Client> (url, client);

			return response.ToList ();
		}

        public async Task <List <Client>> GetAllClients ()
        {
            string url = ApiUri.GetAllClients.ToUrl ();
            return await httpService.GetAll<List <Client>> (url);
        }

        public async Task <Client> GetClientByUsername (string username)
		{
            string url = ApiUri.GetClientByUsername.ToUrl ();
            
            var keyVal = new KeyValuePair<string, string> ("username", username.ToBase64 ());

            return await httpService.GetWithParams<Client> (url,
                                                            new List <KeyValuePair <string, string>> ()
                                                            {
                                                                keyVal
                                                            });
        }

        public async Task <List <Client>> GetClientsByConsultantId (int? consultantId)
        {
            string url = ApiUri.GetClientsByConsultantId.ToUrl ();
            
            var keyVal = new KeyValuePair<string, string> ("consultantId", consultantId.ToString ().ToBase64 ());

            return await httpService.GetWithParams<List <Client>> (url,
                                                            new List <KeyValuePair <string, string>> ()
                                                            {
                                                                keyVal
                                                            });
        }
		
		public async Task <List <ApiResponse>> AddBehaviorGraphs (BehaviorGraph behaviorGraph)
		{
			string url = ApiUri.AddBehaviorGraph.ToUrl ();

			string filename = behaviorGraph.Filename;

			IFileService fileService = new FileService(FileDirectory.Graph);
			string content = await fileService.ReadToEndAsStringAsync (filename);
			byte[] bytesContent = content.ToBytesArray ();

			var response = await httpService.PostMultipartFormDataContent
							   <IEnumerable <ApiResponse>, BehaviorGraph> (url, behaviorGraph, bytesContent, filename);
			
			return response.ToList ();
		}

		public async Task <List <ApiResponse>> AddJournalEntry (JournalEntry journalEntry)
		{
			string url = ApiUri.AddJournalEntry.ToUrl ();

			var response = await httpService.PostMultipartFormDataContent
							   <IEnumerable <ApiResponse>, JournalEntry> (url, journalEntry, journalEntry.ImageBytes, journalEntry.ImageFileName);

			return response.ToList ();
		}

		public async Task<List<BehaviorGraph>> GetBehaviorGraphByClientId(int? clientId)
		{
			string url = ApiUri.GetBehaviorGraphByClientId.ToUrl();

			var keyVal = new KeyValuePair<string, string>("clientId", clientId.ToString().ToBase64());

            return await httpService.GetWithParams<List<BehaviorGraph>>(url,
																		new List<KeyValuePair<string, string>>()
																		{
																			keyVal
																		});
		}

        public async Task <List <JournalEntry>> GetAllJournalsByClientId(int? clientId)
		{
			string url = ApiUri.GetAllJournalsByClientId.ToUrl ();

			var keyVal = new KeyValuePair<string, string>("clientId", clientId.ToString().ToBase64());

			return await httpService.GetWithParams<List<JournalEntry>>(url,
																		new List<KeyValuePair<string, string>>()
																		{
																			keyVal
																		});
        }

		public async Task<string> GetFileContentByFilename(string filename, string directory)
		{
			string url = ApiUri.GetFileContentByFilename.ToUrl();

			var keyVal1 = new KeyValuePair<string, string>("filename", filename.ToBase64());
			var keyVal2 = new KeyValuePair<string, string>("directory", directory.ToBase64());

			return await httpService.GetWithParams<string>(url,
														   new List<KeyValuePair<string, string>>()
														   {
															   keyVal1, keyVal2
														   });
		}

		public async Task <List <ApiResponse>> AllowClientTakeMbes (Client client)
		{
			string url = ApiUri.AllowClientTakeMbes.ToUrl();

			var response = await httpService.PostReadResponse
							   <IEnumerable <ApiResponse>, Client> (url, client);
			
			return response.ToList();
        }

		public async Task<List<ApiResponse>> AddAccelerometerRecord(AccelerometerRecord record)
		{
			string url = ApiUri.AddAccelerometerRecord.ToUrl();

			string filename = record.Filename;

			IFileService fileService = new FileService(FileDirectory.Accelerometer);
			string content = await fileService.ReadToEndAsStringAsync(filename);
			byte[] bytesContent = content.ToBytesArray();

			var response = await httpService.PostMultipartFormDataContent
							   <IEnumerable<ApiResponse>, AccelerometerRecord>(url, record, bytesContent, filename);

			return response.ToList();
		}

		public async Task<List<AccelerometerRecord>> GetAccelerometerRecordByClientId(int? clientId)
		{
			string url = ApiUri.GetAccelerometerRecordByClientId.ToUrl();

			var keyVal = new KeyValuePair<string, string>("clientId", clientId.ToString().ToBase64());

			return await httpService.GetWithParams<List<AccelerometerRecord>>(url,
																		new List<KeyValuePair<string, string>>()
																		{
																			keyVal
																		});
		}
	}
}
