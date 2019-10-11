using System.Linq;
using System.Text.RegularExpressions;
using Java.Lang;
using PeriwinkleApp.Core.Sources.Exceptions;
using PeriwinkleApp.Core.Sources.Services.Interfaces;
using String = System.String;

namespace PeriwinkleApp.Core.Sources.Services
{
    public class InputValidationService : IInputValidationService
    {
    #region Public Methods

        // chars min 1, max 32, is a Word
        public void ValidateUsername (string username, int min = 1, int max = 32)
        {
			string pattern = @"^[a-z?A-Z?0-9?]{" + min + "," + max + "}$";

			var ex = Match (pattern, username, "Username must be alphanumeric only.");
            if (ex != null) throw ex;
        }
        
        // chars min 1, max 32, Contains at least 1 digit, 1 Uppercase, is a Word
        public void ValidatePassword (string password, int min = 1, int max = 32)
        {
			string pattern = @"^[a-z?A-Z?0-9?]{" + min + "," + max + "}$";

			var ex = Match (pattern, password, "Password must be alphanumeric only.");
            if (ex != null) throw ex;
        }
        
        // word, min 1, max 64
        public void ValidateFirstName (string fname, int min = 1, int max = 64)
        {
            string pattern = @"^[a-zA-ZàáâäãåąčćęèéêëėįìíîïłńòóôöõøùúûüųūÿýżźñçčšžÀÁÂÄÃÅĄĆČĖĘÈÉÊËÌÍÎÏĮŁŃÒÓÔÖÕØÙÚÛÜŲŪŸÝŻŹÑßÇŒÆČŠŽ ,.'_-]{"+
                        min + "," + max + "}$";

            var ex = Match (pattern, fname, "First Name must be alphabetical characters only.");
            if (ex != null) throw ex;
        }

        // word, min 1, max 48
        public void ValidateLastName (string lname, int min = 1, int max = 48)
        {
            string pattern = @"^[a-zA-ZàáâäãåąčćęèéêëėįìíîïłńòóôöõøùúûüųūÿýżźñçčšžÀÁÂÄÃÅĄĆČĖĘÈÉÊËÌÍÎÏĮŁŃÒÓÔÖÕØÙÚÛÜŲŪŸÝŻŹÑßÇŒÆČŠŽ ,.'_-]{"+
                             min + "," + max + "}$";

            var ex = Match (pattern, lname, "Last Name must be alphabetical characters only.");
            if (ex != null) throw ex;
        }

        // Digits, start with 09 + 9 digits
        public void ValidateContact (string contact)
        {
            const string pattern = @"^(09[\d]{9,9})$";
            
            var ex = Match (pattern, contact, "Contact Number must follow the format 09xxxxxxxxx");
            if (ex != null) throw ex;
        }
        
        // 128 max, may @ then .
        public void ValidateEmail (string email)
        {
            const string pattern = @"^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[A-Za-z0-9.-]+.[A-Za-z0-9]{2,}$";
            
            if(String.IsNullOrEmpty (email) || email.Length > 128)
                throw new InputValidationException ("Invalid Email Address");
            
            var ex = Match (pattern, email, "Invalid Email Address");
            if (ex != null) throw ex;
        }

    #endregion

    #region Private Methods

        private PeriwinkleException Match (string pattern, string input, string errMsg)
        {
            Regex regex = new Regex (pattern);

            // Match = no exception, No Match = exception
            return regex.IsMatch (input) ? null : new InputValidationException (errMsg);
        }
        
    #endregion

    }
}
