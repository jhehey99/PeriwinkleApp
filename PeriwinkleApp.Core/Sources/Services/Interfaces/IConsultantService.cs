using System.Collections.Generic;
using System.Threading.Tasks;
using PeriwinkleApp.Core.Sources.Models.Domain;
using PeriwinkleApp.Core.Sources.Models.Response;

namespace PeriwinkleApp.Core.Sources.Services.Interfaces
{
    public interface IConsultantService
    {
        Task <List <ApiResponse>> RegisterConsultant (Consultant consultant);
        Task <List <ApiResponse>> ValidatePendingConsultant (string username, bool accept);
        Task <List <Consultant>> GetAllConsultants ();
        Task <List <Consultant>> GetAllPendingConsultants ();
        Task <Consultant> GetConsultantByUsername (string username);
        Task <Consultant> GetConsultantByClientId (int? clientId);
    }
}
