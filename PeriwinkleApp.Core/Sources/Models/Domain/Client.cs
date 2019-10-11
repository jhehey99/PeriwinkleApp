using Newtonsoft.Json;
using PeriwinkleApp.Core.Sources.Utils;

namespace PeriwinkleApp.Core.Sources.Models.Domain
{
    public class Client : Account
    {
        public int? ClientId { get; set; }
        public float? Height { get; set; }
        public float? Weight { get; set; }
        public int MbesAttemptCount { get; set; }

		[JsonConverter(typeof(BoolConverter))]
        public bool MbesAllowAttempt { get; set; }

        public Client ()
        {
            AccTypeId = AccountType.Client;
        }

        public Client(Account account) : base(account)
        {
            AccTypeId = AccountType.Client;
            Height = null;
            Weight = null;
        }

        public bool Equals (Client other)
        {
            return base.Equals (other) && 
                   ClientId.Equals (other.ClientId) && 
                   Height.Equals (other.Height) && 
                   Weight.Equals (other.Weight) &&
                   MbesAttemptCount.Equals (other.MbesAttemptCount) &&
                   MbesAllowAttempt == other.MbesAllowAttempt;
        }
    }
}
