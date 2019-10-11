using PeriwinkleApp.Core.Sources.CommonInterfaces;
using PeriwinkleApp.Core.Sources.Extensions;

namespace PeriwinkleApp.Core.Sources.Models.Response
{
    public class ApiResponse : IDebugString
    {
        public ApiResponseCode Code { get; }
        public string Subject { get; }
        public string Message { get; }

        private string jsonString;

        public ApiResponse (ApiResponseCode code, string subject, string message)
        {
            Code = code;
            Subject = subject;
            Message = message;
        }

        public string ToDebug ()
        {
            return jsonString ?? (jsonString = this.PrettySerialize ());
        }
        
        public override string ToString ()
        {
            return Subject + " is " + Message;
        }

    }
}
