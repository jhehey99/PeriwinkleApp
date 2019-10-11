using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PeriwinkleApp.Core.Sources.Extensions;
using PeriwinkleApp.Core.Sources.Models.Domain;
using PeriwinkleApp.Core.Sources.Models.Response;

namespace PeriwinkleApp.Core.Sources.Services
{
    public class JournalService : PeriwinkleHttpService
    {
        private new static string Tag => "JournalService";

        public async Task <List <ApiResponse>> CreateEntry (JournalEntry entry)
        {
            string url = ApiUri.CreateJournalEntry.ToUrl ();

            var response = await httpService.PostReadResponse
                               <IEnumerable <ApiResponse>, JournalEntry> (url, entry);

            return response.ToList ();
        }

        public async Task <List <ApiResponse>> SendImageUri (Image64 image)
        {
            string url = ApiUri.UploadJsonFile.ToUrl();

            var response = await httpService.PostReadResponse
                               <IEnumerable<ApiResponse>, Image64>(url, image);

            return response.ToList();
        }

        

    }
}
