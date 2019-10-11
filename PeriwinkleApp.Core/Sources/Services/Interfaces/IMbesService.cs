using System.Collections.Generic;
using System.Threading.Tasks;
using PeriwinkleApp.Core.Sources.Models.Domain;
using PeriwinkleApp.Core.Sources.Models.Response;

namespace PeriwinkleApp.Core.Sources.Services.Interfaces
{
    public interface IMbesService
	{
		Task <List <ApiResponse>> AddClientMbesResponse (MbesResponse mbesResponse);
		Task<int> AddMbes(Mbes mbes);
		Task<List<Mbes>> GetMbesByClientId(int clientId);
		Task<MbesResponse> GetResponseByMbesId(int mbesId);
	}
}
