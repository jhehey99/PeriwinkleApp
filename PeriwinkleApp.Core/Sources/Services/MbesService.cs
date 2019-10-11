using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PeriwinkleApp.Core.Sources.Extensions;
using PeriwinkleApp.Core.Sources.Models.Domain;
using PeriwinkleApp.Core.Sources.Models.Response;
using PeriwinkleApp.Core.Sources.Services.Interfaces;

namespace PeriwinkleApp.Core.Sources.Services
{
    public class MbesService : PeriwinkleHttpService, IMbesService
    {
        private new static string Tag => "MbesService";

		public async Task <List <ApiResponse>> AddClientMbesResponse (MbesResponse mbesResponse)
		{
			string url = ApiUri.AddClientMbesResponse.ToUrl ();

			var response = await httpService.PostReadResponse
							   <IEnumerable<ApiResponse>, MbesResponse>(url, mbesResponse);

			return response.ToList();
		}

		public async Task<int> AddMbes(Mbes mbes)
		{
			string url = ApiUri.AddClientMbes.ToUrl();

			var response = await httpService.PostReadResponse
				<string, Mbes>(url, mbes);

			if (int.TryParse(response, out int mbesId))
				return mbesId;

			return -1;
		}
		
		public async Task<List<Mbes>> GetMbesByClientId(int clientId)
		{
			string url = ApiUri.GetMbesByClientId.ToUrl();

			var keyVal = new KeyValuePair<string, string>("MbesClientId", clientId.ToString());

			return await httpService.GetWithParams<List<Mbes>>(url,
															new List<KeyValuePair<string, string>>()
															{
																keyVal
															});
		}
		
		public async Task<MbesResponse> GetResponseByMbesId(int mbesId)
		{
			string url = ApiUri.GetResponseByMbesId.ToUrl();

			var keyVal = new KeyValuePair<string, string>("MbesId", mbesId.ToString());

			return await httpService.GetWithParams<MbesResponse>(url,
															new List<KeyValuePair<string, string>>()
															{
																keyVal
															});
		}
	}
}
