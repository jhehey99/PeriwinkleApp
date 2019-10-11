using System.Collections.Generic;

namespace PeriwinkleApp.Core.Sources.Extensions
{
    public static partial class ApiUriExtension
    {
        private static readonly string ipAddress;
        private static readonly Dictionary <ApiUri, string> serviceUris;

        static ApiUriExtension ()
		{
			//TODO BAGUHIN MO KO
			ipAddress = "192.168.1.101"; //"192.168.1.101" "api.periwinkle.com"

			// initialize dictionary of service URIs for the Periwinkle API
			serviceUris = new Dictionary <ApiUri, string> ()
            {

            #region Connection Services
				{ ApiUri.CanRequest, "/services/connection/CanRequest.php" },
			#endregion

			#region Account Services
                { ApiUri.LoginAccount, "/services/account/login.php" },
                { ApiUri.RegisterAccount, "/services/account/RegisterAccount.php" },
                { ApiUri.CheckAccountExists, "/services/account/CheckAccountExists.php" },
                { ApiUri.GetAccountTypeByUsername, "/services/account/GetAccountTypeByUsername.php" },
                { ApiUri.GetAccountAsSession, "/services/account/GetAccountAsSession.php" },
            #endregion

            #region Consultant Services
                { ApiUri.RegisterConsultant, "/services/consultant/RegisterConsultant.php" },
                { ApiUri.ValidatePendingConsultant, "/services/consultant/ValidatePendingConsultant.php" },
                { ApiUri.GetAllConsultants, "/services/consultant/GetAllConsultants.php" },
                { ApiUri.GetAllPendingConsultants, "/services/consultant/GetAllPendingConsultants.php" },
                { ApiUri.GetConsultantByUsername, "/services/consultant/GetConsultantByUsername.php" },
                { ApiUri.GetConsultantByClientId, "/services/consultant/GetConsultantByClientId.php" },
            #endregion

            #region Client Services
                { ApiUri.RegisterClient, "/services/client/RegisterClient.php" },
                { ApiUri.RegisterClientToConsultant, "/services/client/RegisterClientToConsultant.php" },
                { ApiUri.UpdateHeightWeight, "/services/client/UpdateHeightWeight.php" },
                { ApiUri.GetAllClients, "/services/client/GetAllClients.php" },
                { ApiUri.GetClientByUsername, "/services/client/GetClientByUsername.php" },
                { ApiUri.GetClientsByConsultantId, "/services/client/GetClientsByConsultantId.php" },
                { ApiUri.GetBehaviorGraphByClientId, "/services/client/GetBehaviorGraphByClientId.php" },
				{ ApiUri.AddMbesAttemptCount, "/services/client/AddMbesAttemptCount.php" },
				{ ApiUri.AddBehaviorGraph, "/services/client/AddBehaviorGraph.php" },
				{ ApiUri.GetAllJournalsByClientId, "/services/client/GetAllJournalsByClientId.php" },
				{ ApiUri.AddJournalEntry, "/services/client/AddJournalEntry.php" },
				{ ApiUri.GetFileContentByFilename, "/services/client/GetFileContentByFilename.php" },
				{ ApiUri.AllowClientTakeMbes, "/services/client/AllowClientTakeMbes.php" },
            #endregion

			#region Journal Services
				{ ApiUri.CreateJournalEntry, "/services/journal/CreateJournalEntry.php" },
			#endregion

			#region Mbes Services
				{ ApiUri.AddClientMbesResponse, "/services/mbes/AddClientMbesResponse.php" },
				{ ApiUri.AddClientMbes, "/services/mbes/AddClientMbes.php" },
				{ ApiUri.GetMbesByClientId, "/services/mbes/GetMbesByClientId.php" },
				{ ApiUri.GetResponseByMbesId, "/services/mbes/GetResponseByMbesId.php" },
			#endregion

			#region Password Services
                { ApiUri.RegisterPassword, "/services/password/RegisterPassword.php" },
                { ApiUri.GetPasswordByUsername, "/services/password/GetPasswordByUsername.php" },
            #endregion

            #region File Services
                { ApiUri.UploadJsonFile, "/services/files/UploadJsonFile.php"},
			#endregion

			#region Accelerometer
                { ApiUri.AddAccelerometerRecord, "/services/accelerometer/AddAccelerometerRecord.php"},
                { ApiUri.GetAccelerometerRecordByClientId, "/services/accelerometer/GetAccelerometerRecordByClientId.php"}
			#endregion	

			};
        }
    }
}
