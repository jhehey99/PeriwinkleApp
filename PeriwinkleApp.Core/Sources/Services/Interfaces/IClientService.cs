using System.Collections.Generic;
using System.Threading.Tasks;
using PeriwinkleApp.Core.Sources.Models.Domain;
using PeriwinkleApp.Core.Sources.Models.Response;

namespace PeriwinkleApp.Core.Sources.Services.Interfaces
{
    public interface IClientService
    {
        Task <List <ApiResponse>> RegisterClient (Client client);
        Task <List <ApiResponse>> RegisterClientToConsultant (Client client, Consultant consultant);
        Task <List <ApiResponse>> UpdateHeightWeight (Client client);
		Task <List <ApiResponse>> AddMbesAttemptCount (Client client);
        Task <List <Client>> GetAllClients ();
		Task <Client> GetClientByUsername (string username);
        Task <List<Client>> GetClientsByConsultantId (int? consultantId);


		Task <List <ApiResponse>> AddBehaviorGraphs (BehaviorGraph behaviorGraph);
		Task <List <BehaviorGraph>> GetBehaviorGraphByClientId (int? clientId);

		
        // task addjournal
		Task <List <ApiResponse>> AddJournalEntry (JournalEntry journalEntry);
        Task <List <JournalEntry>> GetAllJournalsByClientId(int? clientId);


		Task <string> GetFileContentByFilename (string filename, string directory);

		Task <List <ApiResponse>> AllowClientTakeMbes (Client client);

		Task<List<ApiResponse>> AddAccelerometerRecord(AccelerometerRecord record);
		Task<List<AccelerometerRecord>> GetAccelerometerRecordByClientId(int? clientId);

		Task<List<ApiResponse>> AddSensorRecord(SensorRecord record);
		Task<List<SensorRecord>> GetSensorRecordByClientId(int? clientId);
	}
}
