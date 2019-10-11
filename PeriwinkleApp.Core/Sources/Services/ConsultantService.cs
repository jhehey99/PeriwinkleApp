using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PeriwinkleApp.Core.Sources.Extensions;
using PeriwinkleApp.Core.Sources.Models.Domain;
using PeriwinkleApp.Core.Sources.Models.Response;
using PeriwinkleApp.Core.Sources.Services.Interfaces;
using PeriwinkleApp.Core.Sources.Utils;

namespace PeriwinkleApp.Core.Sources.Services
{
    public class ConsultantService : PeriwinkleHttpService, IConsultantService
    {
        public new static string Tag => "ConsultantService";
        public async Task <List <ApiResponse>> RegisterConsultant (Consultant consultant)
        {
            string url = ApiUri.RegisterConsultant.ToUrl ();
            
            var response = await httpService.PostReadResponse
                               <IEnumerable <ApiResponse>, Consultant> (url, consultant);
            
            return response.ToList ();
        }

        public async Task <List <ApiResponse>> ValidatePendingConsultant (string username, bool accept)
        {
            string url = ApiUri.ValidatePendingConsultant.ToUrl ();
            
            var keyVal1 = new KeyValuePair<string, string> ("username", username.ToBase64 ());
            var keyVal2 = new KeyValuePair<string, string> ("accept", accept.ToString().ToBase64 ());

            return await httpService.GetWithParams<List <ApiResponse>> (url,
                                                                new List <KeyValuePair <string, string>> ()
                                                                {
                                                                    keyVal1, keyVal2
                                                                });
        }

        public async Task <List <Consultant>> GetAllConsultants ()
        {
            string url = ApiUri.GetAllConsultants.ToUrl ();
            
            var consultants = await httpService.GetAll<List <Consultant>> (url);
            
            return consultants;
        }

        public async Task <List <Consultant>> GetAllPendingConsultants ()
        {
            string url = ApiUri.GetAllPendingConsultants.ToUrl ();
            
            var consultants = await httpService.GetAll<List <Consultant>> (url);
            
            return consultants;
        }

        public async Task <Consultant> GetConsultantByUsername (string username)
        {
            string url = ApiUri.GetConsultantByUsername.ToUrl ();
            
            var keyVal = new KeyValuePair<string, string> ("username", username.ToBase64 ());
            
            return await httpService.GetWithParams<Consultant> (url,
                                                            new List <KeyValuePair <string, string>> ()
                                                            {
                                                                keyVal
                                                            });
        }

        public async Task <Consultant> GetConsultantByClientId (int? clientId)
        {
            string url = ApiUri.GetConsultantByClientId.ToUrl ();
            
            var keyVal = new KeyValuePair<string, string> ("clientId", clientId.ToString ().ToBase64 ());
            
           return await httpService.GetWithParams<Consultant> (url,
                                                                   new List <KeyValuePair <string, string>> ()
                                                                   {
                                                                       keyVal
                                                                   });
        }
    }
}
