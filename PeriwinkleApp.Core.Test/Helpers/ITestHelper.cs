using System.Collections.Generic;
using PeriwinkleApp.Core.Sources.Models.Domain;
using PeriwinkleApp.Core.Sources.Models.Response;

namespace PeriwinkleApp.Core.Test.Helpers
{
    public interface ITestHelper
    {
        (Consultant, List <ApiResponse>) RegisterConsultant (Consultant consultant = null);
        (Client, List <ApiResponse>) RegisterConsultantClient (Consultant consultant, Client client = null);
        IEnumerable <ApiResponse> RegisterRandomValidPassword (string username);

        

    }
}
