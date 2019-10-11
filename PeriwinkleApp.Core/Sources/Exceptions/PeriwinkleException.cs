using System;
using PeriwinkleApp.Core.Sources.CommonInterfaces;
using PeriwinkleApp.Core.Sources.Utils;

namespace PeriwinkleApp.Core.Sources.Exceptions
{
    public abstract class PeriwinkleException : Exception, IDebugString
    {
        protected virtual string Tag => "";
        protected virtual string Code => "";

        protected PeriwinkleException (string message) : base (message)
        {
            Logger.Debug (this);
        }

        public override string ToString ()
        {
            return "Problem Occured: " + Message;
        }

        public string ToDebug ()
        {
            return "Exception " + Code + ": " + Tag + "\n\t" + Message;
        }
    }
}
