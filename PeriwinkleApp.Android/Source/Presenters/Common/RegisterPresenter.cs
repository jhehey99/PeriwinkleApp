﻿using System.Collections.Generic;
using System.Linq;
using PeriwinkleApp.Android.Source.Factories;
using PeriwinkleApp.Android.Source.Session;
using PeriwinkleApp.Android.Source.Views.Fragments.Common;
using PeriwinkleApp.Core.Sources.Exceptions;
using PeriwinkleApp.Core.Sources.Models.Domain;
using PeriwinkleApp.Core.Sources.Models.Response;
using PeriwinkleApp.Core.Sources.Services;
using PeriwinkleApp.Core.Sources.Services.Interfaces;

namespace PeriwinkleApp.Android.Source.Presenters.Common
{
    public interface IRegisterPresenter
    {
        void PerformRegistration(Account account, string plainPassword);
        void ValidateFirstName (string firstname);
        void ValidateLastName (string lastname);
        void ValidateUsername (string username);
        void ValidatePassword (string password);
        void ValidateEmail (string email);
        void ValidateContact (string contact);
    }

    public class RegisterPresenter : IRegisterPresenter
    {
        private readonly IRegisterView view;
        private readonly IInputValidationService inputService;
        private readonly IClientService cliService;
        private readonly IConsultantService conService;
        private readonly IPasswordService passService;

        public RegisterPresenter (IRegisterView view)
        {
            this.view = view;

            inputService = inputService ?? new InputValidationService();
            conService = conService ?? new ConsultantService();
            cliService = cliService ?? new ClientService();
            passService = passService ?? new PasswordService();
        }

        public async void PerformRegistration(Account account, string plainPassword)
        {
            // TODO
            // Check muna tru service kung existing
            // i believe nagrereturn sya ng errors pag existing
            // un nalang kunin, i-try catch natin

            // PASSWORD Pa pala
            // password service
            List<ApiResponse> responses = null;
            List<ApiResponse> passResponses = null;

            if (account.AccTypeId == AccountType.Consultant)
            {
                responses = await conService.RegisterConsultant(new Consultant(account));
            }
            else if (account.AccTypeId == AccountType.Client)
            {
                AccountSession conAccount = SessionFactory.ReadSession <AccountSession> (SessionKeys.LoginKey);
                if (conAccount.AccountType != AccountType.Consultant)
                    return;

                Consultant consultant = new Consultant()
                                        {
                                            AccountId = conAccount.AccountId,
                                            Username = conAccount.Username,
                                            AccTypeId = conAccount.AccountType
                                        };
                responses = await cliService.RegisterClientToConsultant (new Client (account), consultant);
            }
            
            if (responses == null)
            {
				//TODO UNG DISPLAY KO GAGO KA BA, GAWIN MO NA JHON PAUL JASPE UNG DISPLAY NETO OI MAMA MO
                view.DisplayResponse ("Error Occured", "There was an error processing the request");
                return;
            }

            passResponses = await passService.RegisterPassword(account.Username, plainPassword);

			if (passResponses == null)
			{
				view.DisplayResponse("Error Occured", "There was an error processing the request");
				return;
			}

            bool success = responses.Select (resp => resp.Code == ApiResponseCode.RegisterSuccess).FirstOrDefault () && 
                            passResponses.Select(resp => resp.Code == ApiResponseCode.RegisterSuccess).FirstOrDefault();
            
            if (success)
            {
                ApiResponse response = responses.FirstOrDefault ();
                view.DisplayResponse (response?.Subject, response?.ToString ());
				view.FinishRegister();
			}
            else
            {
                string subject = "Registration Not Successful";
                string message = "";

                for (int i = 0; i < responses.Count; i++)
                    message += (i + 1) + ") " + responses[i] + "\n";
                
                view.DisplayResponse (subject, message);
            }

        }

        public void ValidateFirstName (string firstname)
        {
            try
            {
                inputService.ValidateFirstName(firstname);
            }
            catch (InputValidationException e)
            {
                view.ShowInvalidMessage(e.Message);
            }
        }

        public void ValidateLastName (string lastname)
        {
            try
            {
                inputService.ValidateLastName(lastname);
            }
            catch (InputValidationException e)
            {
                view.ShowInvalidMessage(e.Message);
            }
        }
        
        public void ValidateUsername (string username)
        {
            try
            {
                inputService.ValidateUsername (username);
            }
            catch (InputValidationException e)
            {
                view.ShowInvalidMessage(e.Message);
            }
        }

        public void ValidatePassword (string password)
        {
            try
            {
                inputService.ValidatePassword(password);
            }
            catch (InputValidationException e)
            {
                view.ShowInvalidMessage(e.Message);
            }
        }

        public void ValidateEmail (string email)
        {
            try
            {
                inputService.ValidateEmail(email);
            }
            catch (InputValidationException e)
            {
                view.ShowInvalidMessage(e.Message);
            }
        }

        public void ValidateContact (string contact)
        {
            try
            {
                inputService.ValidateContact(contact);
            }
            catch (InputValidationException e)
            {
                view.ShowInvalidMessage(e.Message);
            }
        }
    }
}
