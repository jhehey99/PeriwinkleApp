namespace PeriwinkleApp.Core.Sources.Exceptions
{
    public class InputValidationException : PeriwinkleException
    {
        protected override string Tag  => "InputValidationException";
        protected override string Code => "INPUT1001";
        public InputValidationException (string message) : base (message) {}
    }
}
