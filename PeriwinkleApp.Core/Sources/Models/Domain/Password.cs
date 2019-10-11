using PeriwinkleApp.Core.Sources.CommonInterfaces;
using PeriwinkleApp.Core.Sources.Extensions;

namespace PeriwinkleApp.Core.Sources.Models.Domain
{
    public class Password : IDebugString
    {
        public int? PsAccountId { get; set; }
        public string PasswordHash { get; set; }
        public string PasswordSalt { get; set; }

        private string jsonString;
        
        public Password ()
        {
            
        }

        public override string ToString ()
        {
            return jsonString ?? (jsonString = this.PrettySerialize ());
        }
        
        public string ToDebug ()
        {
            return jsonString ?? (jsonString = this.PrettySerialize ());
        }
    }
}
