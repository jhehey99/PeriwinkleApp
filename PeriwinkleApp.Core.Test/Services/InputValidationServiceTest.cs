using System;
using System.Collections.Generic;
using NUnit.Framework;
using PeriwinkleApp.Core.Sources.Services;
using PeriwinkleApp.Core.Sources.Services.Interfaces;
using PeriwinkleApp.Core.Sources.Utils;
using PeriwinkleApp.Core.Test.Utils;

namespace PeriwinkleApp.Core.Test.Services
{
    [TestFixture]
    public class InputValidationServiceTest
    {
        [Test]
        public void UsernameTest ()
        {
            
            Assert.True (true);
        }

        [Test]
        public void ValidUsernamesTest ()
        {
            IInputValidationService service = new InputValidationService ();
            List <string> validUsernames = FileUtils.FileToStringList (TestFiles.ValidUsernames);

            foreach (string username in validUsernames)
            {
                Assert.DoesNotThrow (() =>
                {
                    service.ValidateUsername (username);
                });
            }
        }
        
        [Test]
        public void ValidPasswordsTest ()
        {
            IInputValidationService service = new InputValidationService ();
            List <string> validPasswords = FileUtils.FileToStringList (TestFiles.ValidPasswords);

            foreach (string password in validPasswords)
            {
                Assert.DoesNotThrow (() =>
                {
                    service.ValidatePassword (password);
                });
            }
        }

        [Test]
        public void ValidFirstNamesTest ()
        {
            IInputValidationService service = new InputValidationService ();
            List <string> validFirstNames = FileUtils.FileToStringList (TestFiles.ValidFirstNames);

            foreach (string firstName in validFirstNames)
            {
                Assert.DoesNotThrow (() =>
                {
                    service.ValidateFirstName (firstName);
                });
            }
        }
        
        [Test]
        public void ValidLastNamesTest ()
        {
            IInputValidationService service = new InputValidationService ();
            List <string> validLastNames = FileUtils.FileToStringList (TestFiles.ValidLastNames);

            foreach (string lastName in validLastNames)
            {
                Assert.DoesNotThrow (() =>
                {
                    service.ValidateLastName (lastName);
                });
            }
        }
        
        [Test]
        public void ValidContactsTest ()
        {
            IInputValidationService service = new InputValidationService ();
            List <string> validContacts = FileUtils.FileToStringList (TestFiles.ValidContacts);

            foreach (string contact in validContacts)
            {
                Assert.DoesNotThrow (() =>
                {
                    service.ValidateContact (contact);
                });
            }
        }
        
        [Test]
        public void ValidEmailsTest ()
        {
            IInputValidationService service = new InputValidationService ();
            List <string> validEmails = FileUtils.FileToStringList (TestFiles.ValidEmails);

            foreach (string email in validEmails)
            {
                Assert.DoesNotThrow (() =>
                {
                    service.ValidateEmail (email);
                });
            }
        }
    }
}
