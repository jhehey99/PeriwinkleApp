using System;
using System.Collections.Generic;
using PeriwinkleApp.Core.Sources.Models.Domain;
using PeriwinkleApp.Core.Sources.Models.Response;
using PeriwinkleApp.Core.Sources.Services;
using PeriwinkleApp.Core.Sources.Services.Interfaces;
using PeriwinkleApp.Core.Sources.Utils;

namespace PeriwinkleApp.Core.Test.Usage
{
    public class ConsultantUsage
    {
        private static readonly PasswordUsage passwordUsage;

        static ConsultantUsage ()
        {
            passwordUsage = new PasswordUsage ();
        }
        
        // lahat valid dapat
        public (Consultant, List<ApiResponse>) RegisterPendingConsultantWithPassword (Consultant consultant = null)
        {
            consultant = consultant ?? TestRepository.GetValidConsultant ();
            IConsultantService consultantService = new ConsultantService ();

            var task = consultantService.RegisterConsultant (consultant);
            task.Wait ();
            var conResponse = task.Result;

            passwordUsage.RegisterPassword (consultant.Username);
            
            return (consultant, conResponse);
        }

        public (Consultant, List<ApiResponse>) RegisterConsultantWithPassword (Consultant consultant = null, bool accept = false)
        {
            consultant = consultant ?? TestRepository.GetValidConsultant ();
            IConsultantService consultantService = new ConsultantService ();

            var task = consultantService.RegisterConsultant (consultant);
            task.Wait ();
            var conResponse = task.Result;

            passwordUsage.RegisterPassword (consultant.Username);
            
            // ACCEPT NATIN
            // consultantService.AcceptConsultantByUsername(consultant.Username);

            return (consultant, conResponse);
        }
        
        
    }
}
