using PeriwinkleApp.Android.Source.Cache;
using PeriwinkleApp.Android.Source.Factories;
using PeriwinkleApp.Android.Source.Session;
using PeriwinkleApp.Android.Source.Views.Fragments.Common;
using PeriwinkleApp.Core.Sources.Models.Domain;
using PeriwinkleApp.Core.Sources.Services;
using PeriwinkleApp.Core.Sources.Services.Interfaces;
using PeriwinkleApp.Core.Sources.Utils;
using System.Threading.Tasks;

namespace PeriwinkleApp.Android.Source.Presenters.Common
{
    public interface ILoginPresenter
    {
		Task<bool> PerformLogin (string username, string password);
    }

    public class LoginPresenter : ILoginPresenter
    {
        private readonly ILoginView view;
        private readonly IHashService hashService;
        private readonly IAccountService accService;
        private readonly IPasswordService passService;

        private AccountSession loginSession;

        //LoginSession

        public LoginPresenter (ILoginView view)
        {
            this.view = view;
            hashService = hashService ?? new HashService();
            accService = accService ?? new AccountService ();
            passService = passService ?? new PasswordService();
        }

        public async Task<bool> PerformLogin (string username, string password)
        {
            // Empty ung input so error
            if (username == "" || password == "")
            {
                view.DisplayLoginError (true);
                return false;
            }
            
            // TODO Kung sakali, strip ung special

            // Get Account Session details
            Account loggedAccount = await accService.GetAccountAsSession (username);

            if (loggedAccount == null)
            {
                view.DisplayLoginError (true);
                return false;
            }

            // username exists, check na natin kung tama ung password
            view.DisplayLoginError (false);

            // verify natin ung password na input at ni account
            Password userPassword = await passService.GetPasswordByUsername (username);
            bool validPass = await hashService.VerifyPasswordHashAsync (password, userPassword);

            // pag di valid, show tayo ng error
            if (!validPass)
            {
                view.DisplayLoginError(true);
                return false;
            }

            // create ung session sa nag log in na account
            loginSession = SessionFactory.CreateSession<AccountSession>(SessionKeys.LoginKey);
            loginSession.AddAccountSession(loggedAccount);

			// invalid ung account type
            if (loggedAccount.AccTypeId == null)
				return false;

			//TODO TANGAL MO KO
			Logger.Debug (loggedAccount);

			// user exists and valid na ung password, so login na given ung account type for activity
			view.UserLoginSuccess((AccountType)loggedAccount.AccTypeId);
			return true;
		}
    }
}
