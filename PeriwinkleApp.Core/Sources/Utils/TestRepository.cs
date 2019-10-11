using System;
using PeriwinkleApp.Core.Sources.Exceptions;
using PeriwinkleApp.Core.Sources.Models.Domain;
using PeriwinkleApp.Core.Sources.Services;

namespace PeriwinkleApp.Core.Sources.Utils
{
    public static class TestRepository
    {
        private static readonly Random Getrandom = new Random();

        public static Account GetAccount ()
        {
            string str = randPostName ();
            string estr = randAlpha ();
            return new Account ()
            {
                Username = "user_" + estr,
                FirstName = "first_" + str,
                LastName = "last_" + str,
                Email = "first_last" + estr + "@gmail.com",
                Contact = randContact (),
                Birthday = randBday (),
                Gender = Convert.ToBoolean (Getrandom.Next(2)) ? 'M' : 'F'
            };
        }
        
        public static Consultant GetConsultant ()
        {
            string str = randPostName ();
            string estr = randAlpha ();
            return new Consultant ()
            {
                Username = "con" + estr,
                FirstName = "first_" + str,
                LastName = "last_" + str,
                Email = "first_last" + estr + "@gmail.com",
                Contact = randContact(),
                Birthday = randBday(),
                Gender = Convert.ToBoolean (Getrandom.Next(2)) ? 'M' : 'F',
                License = Getrandom.Next(1000,999999).ToString()
            };
        }

        public static Client GetClient ()
        {
            string str = randPostName ();
            string estr = randAlpha ();
            return new Client ()
            {
                Username = "cli" + estr,
                FirstName = "first_" + str,
                LastName = "last_" + str,
                Email = "first_last" + estr + "@gmail.com",
                Contact = randContact(),
                Birthday = randBday(),
                Gender = Convert.ToBoolean (Getrandom.Next(2)) ? 'M' : 'F',
                Height = Getrandom.Next(100, 250),
                Weight = Getrandom.Next(30, 150)
            }; 
        }
        
        public static Account GetValidAccount ()
        {
            Account account = GetAccount ();
            InputValidationService service = new InputValidationService ();
            
            try
            {
                service.ValidateUsername (account.Username);   
                service.ValidateFirstName (account.FirstName);   
                service.ValidateLastName (account.LastName);   
                service.ValidateEmail (account.Email);   
                service.ValidateContact (account.Contact);   
            }
            catch (InputValidationException e)
            {
                Logger.Debug (e);
                account = null;
                throw;
            }
            
            return account;
        }

        public static Consultant GetValidConsultant ()
        {
            Consultant consultant = GetConsultant ();
            InputValidationService service = new InputValidationService ();
            
            try
            {
                service.ValidateUsername (consultant.Username);   
                service.ValidateFirstName (consultant.FirstName);   
                service.ValidateLastName (consultant.LastName);   
                service.ValidateEmail (consultant.Email);   
                service.ValidateContact (consultant.Contact);   
            }
            catch (InputValidationException e)
            {
                Logger.Debug (e);
                consultant = null;
                throw;
            }
            
            return consultant;
        }

        public static Client GetValidClient ()
        {
            Client client = GetClient ();
            InputValidationService service = new InputValidationService ();
            
            try
            {
                service.ValidateUsername (client.Username);   
                service.ValidateFirstName (client.FirstName);   
                service.ValidateLastName (client.LastName);   
                service.ValidateEmail (client.Email);   
                service.ValidateContact (client.Contact);   
            }
            catch (InputValidationException e)
            {
                Logger.Debug (e);
                client = null;
                throw;
            }
            
            return client;
        }
        
        public static string GetValidPlainPassword ()
        {
            string pass = "Jaspe" + randAlpha (5, 10) + "99";
            InputValidationService service = new InputValidationService ();

            try
            {
                service.ValidatePassword (pass);
            }
            catch (InputValidationException e)
            {
                Logger.Debug (e);
                pass = null;
                throw;
            }

            return pass;
        }
        
        private static string randPostName ()
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
            var spechars = "àáâäãåąčćęèéêëėįìíîïłńòóôöõøùúûüųūÿýżźñçčšžÀÁÂÄÃÅĄĆČĖĘÈÉÊËÌÍÎÏĮŁŃÒÓÔÖÕØÙÚÛÜŲŪŸÝŻŹÑßÇŒÆČŠŽ";
            
            int strSize = Getrandom.Next (5, 20);
            var stringChars = new char[strSize];

            for (int i = 0; i < strSize - 2; i++)
                stringChars [i] = chars [Getrandom.Next (chars.Length)];
            
            for (int i = strSize - 2; i < strSize; i++)
                stringChars [i] = spechars [Getrandom.Next (spechars.Length)];
            
            return new String(stringChars);
        }

        private static string randAlpha (int min = 5, int max = 20)
        {
            var chars = "abcdefghijklmnopqrstuvwxyz";
            
            int strSize = Getrandom.Next (min, max);
            var stringChars = new char[strSize];

            for (int i = 0; i < strSize; i++)
                stringChars [i] = chars [Getrandom.Next (chars.Length)];
            
            return new String(stringChars);
        }
        
        private static string randContact ()
        {
            var chars = "0123456789";
            var stringChars = new char[11];
			stringChars[0] = '0';
			stringChars[1] = '9';

            for (int i = 2; i < stringChars.Length; i++)
                stringChars[i] = chars[Getrandom.Next(chars.Length)];

            return new String(stringChars);
        }

        private static DateTime randBday ()
        {
            var mm = Getrandom.Next(1, 13);
            var dd = Getrandom.Next(1, 28);
            var yyyy = Getrandom.Next(1950, 2018);

            return DateTime.Parse(yyyy.ToString () + "-" + mm.ToString () + "-" + dd.ToString ());
        }

    }
}
