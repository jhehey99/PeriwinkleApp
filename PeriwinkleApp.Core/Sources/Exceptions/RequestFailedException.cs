namespace PeriwinkleApp.Core.Sources.Exceptions
{
    public class RequestFailedException : PeriwinkleException
    {
        protected override string Tag => "RequestFailedException";
        protected override string Code => "HTTP1000";
        public RequestFailedException (string message) : base (message) {}
    }
}
