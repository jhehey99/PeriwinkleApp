namespace PeriwinkleApp.Core.Sources.Extensions
{
    public enum ApiUri
    {
    #region Connection Services
		CanRequest,
	#endregion

    #region Account Services
        LoginAccount,
        RegisterAccount,
        CheckAccountExists,
        GetAccountTypeByUsername,
        GetAccountAsSession,
    #endregion

    #region Consultant Services
        RegisterConsultant,
        ValidatePendingConsultant,
        GetAllConsultants,
        GetAllPendingConsultants,
        GetConsultantByUsername,
        GetConsultantByClientId,
    #endregion

    #region Client Services
        RegisterClient,
        RegisterClientToConsultant,
        UpdateHeightWeight,
        GetAllClients,
        GetClientByUsername,
        GetClientsByConsultantId,
		GetBehaviorGraphByClientId,
		AddMbesAttemptCount,
		AddBehaviorGraph,
		GetAllJournalsByClientId,
		AddJournalEntry,
		GetFileContentByFilename,
		AllowClientTakeMbes,
    #endregion

    #region Journal Service
        CreateJournalEntry,
		#endregion

	#region Mbes Service
		AddClientMbesResponse,
		AddClientMbes,
		GetMbesByClientId,
		GetResponseByMbesId,
	#endregion

	#region Password Services
		RegisterPassword,
		GetPasswordByUsername,
	#endregion

	#region Files Services
		UploadJsonFile,
    #endregion
	
	#region Accelerometer
        AddAccelerometerRecord,
		GetAccelerometerRecordByClientId,
		#endregion

		#region SensorRecord
		AddSensorRecord,
		GetSensorRecordByClientId

		#endregion
	}
}
