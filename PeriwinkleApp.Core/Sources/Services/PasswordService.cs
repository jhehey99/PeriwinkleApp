using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PeriwinkleApp.Core.Sources.Crypto;
using PeriwinkleApp.Core.Sources.Extensions;
using PeriwinkleApp.Core.Sources.Models.Bridge;
using PeriwinkleApp.Core.Sources.Models.Domain;
using PeriwinkleApp.Core.Sources.Models.Response;
using PeriwinkleApp.Core.Sources.Services.Interfaces;
using PeriwinkleApp.Core.Sources.Utils;

namespace PeriwinkleApp.Core.Sources.Services
{
    public class PasswordService : PeriwinkleHttpService, IPasswordService
    {
        private new static string Tag => "PasswordService";

        public async Task <List <ApiResponse>> RegisterPassword (string username, string plainPassword)
        {
            // we need to use the password service for generating the password
            IHashService hashService = new HashService ();
            
            // get a password model with the HASHED plain password
            Password password = hashService.GenerateHashedPassword (plainPassword);

            // create tayo ng POST Model para i-send ung username at password
            UsernamePassword userPassword = new UsernamePassword ()
            {
                PsUsername = username,
                UPassword = password
            };
            
            string url = ApiUri.RegisterPassword.ToUrl ();

            var response = await httpService.PostReadResponse 
                               <IEnumerable <ApiResponse>, UsernamePassword> (url, userPassword);
            
            return response.ToList ();
        }

        public async Task <Password> GetPasswordByUsername (string username)
        {
            string url = ApiUri.GetPasswordByUsername.ToUrl ();
            
            var keyVal = new KeyValuePair<string, string> ("username", username.ToBase64 ());

            return await httpService.GetWithParams<Password> (url,
                                                                   new List <KeyValuePair <string, string>> ()
                                                                   {
                                                                       keyVal
                                                                   });
        }
    }
}
