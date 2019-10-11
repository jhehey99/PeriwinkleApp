using System;
using Newtonsoft.Json;
using PeriwinkleApp.Core.Sources.Utils;

namespace PeriwinkleApp.Core.Sources.Models.Domain
{
    public class Consultant : Account
    {
        public int? ConsultantId { get; set; }
        public string License { get; set; }
		public DateTime ApplicationDate { get; set; }

		[JsonConverter(typeof(BoolConverter))]
        public bool IsPending { get; set; }
		
        public Consultant ()
        {
            AccTypeId = AccountType.Consultant;
			IsPending = true;
		}
		
        public Consultant (Account account) : base(account)
        {
            AccTypeId = AccountType.Consultant;
			IsPending = true;
            License = "";
        }
        
        public bool Equals (Consultant other)
        {
			//TODO UPDATE MO TO
            return base.Equals (other) &&
                   ConsultantId.Equals (other.ConsultantId) &&
                   string.Equals (License, other.License);
        }
    }
}
