using System.Threading.Tasks;
using PeriwinkleApp.Core.Sources.Extensions;

namespace PeriwinkleApp.Core.Sources.Services
{
	public interface IConnectionService
	{

	}
	public class ConnectionService : PeriwinkleHttpService, IConnectionService
    {
		private new static string Tag => "ConnectionService";

		private string serverKey = "aksdo90123kok";


        public async Task <bool> CanRequestToServer ()
		{
			//TODO CHECK KUNG NAGANA TO
			string url = ApiUri.CanRequest.ToUrl ();

			string response = await httpService.GetAll <string> (url);

			return response == serverKey;
		}
    }
}
