using System;
using System.Diagnostics.CodeAnalysis;
using PeriwinkleApp.Core.Sources.CommonInterfaces;
using PeriwinkleApp.Core.Sources.Extensions;

namespace PeriwinkleApp.Core.Sources.Models.Domain
{
    public class Account : IDebugString, IEquatable <Account>
    {
        public int? AccountId { get; set; }
        public string Username { get; set; }
        public string FirstName  { get; set; }
        public string LastName  { get; set; }
        public string Email     { get; set; }
        public string Contact   { get; set; }
        public DateTime? Birthday  { get; set; }
        public char?   Gender    { get; set; }
        public AccountType? AccTypeId { get; set; }
        public DateTime? DateRegistered { get; set; }
        
        private string jsonString;

        public Account ()
        {
            
        }

        public Account (Account account)
        {
            FirstName = account.FirstName;
            LastName = account.LastName;
            Username = account.Username;
            Email = account.Email;
            Contact = account.Contact;
            Birthday = account.Birthday;
            Gender = account.Gender;
            AccTypeId = account.AccTypeId;
            DateRegistered = account.DateRegistered;
        }
        
        public virtual string ToDebug ()
        {
            return jsonString ?? (jsonString = this.PrettySerialize ());
        }

        public bool Equals (Account other)
        {
            if (ReferenceEquals (other, null))
                return false;

            if (ReferenceEquals (this, other))
                return true;

            return AccountId.Equals (other.AccountId) &&
                   string.Equals (Username, other.Username) &&
                   string.Equals (FirstName, other.FirstName) &&
                   string.Equals (LastName, other.LastName) &&
                   string.Equals (Email, other.Email) &&
                   string.Equals (Contact, other.Contact) &&
                   Birthday.Equals (other.Birthday) &&
                   Gender.Equals (other.Gender) &&
                   AccTypeId.Equals (other.AccTypeId) &&
                   DateRegistered.Equals (other.DateRegistered);
        }

    }
}
