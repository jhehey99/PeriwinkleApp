using PeriwinkleApp.Core.Sources.Models.Domain;
using PeriwinkleApp.Core.Sources.Services;
using PeriwinkleApp.Core.Sources.Services.Interfaces;

namespace Production.RegisterLogin.Source.Presenters.Common
{
    public interface ILoginPresenter
    {
        void PerformLogin (string username, string password);
    }

    public class LoginPresenter : ILoginPresenter
    {
        private readonly ILoginView view;
        private readonly IHashService hashService;
        private readonly IAccountService accService;
        private readonly IPasswordService passService;

        //LoginSession

        public LoginPresenter (ILoginView view)
        {
            this.view = view;
            hashService = hashService ?? new HashService();
            accService = accService ?? new AccountService ();
            passService = passService ?? new PasswordService();
        }

        public async void PerformLogin (string username, string password)
        {
            // check if username exists
            // f = show invalid
            // t => get salt and hash of username
            // use encryption to check
            // f = show invalid
            
            // Empty
            if (username == "" || password == "")
            {
                view.DisplayLoginError (true);
                return;
            }

            // TODO Kung sakali, strip ung special
            // dapat malaman kung anong klaseng account
            AccountType? type = await accService.GetAccountType (username);
            
            if (type == null)
            {
                view.DisplayLoginError (true);
                return;
            }

            // username exists, check na natin kung tama ung password
            view.DisplayLoginError (false);

            Password userPassword = await passService.GetPasswordByUsername (username);

            bool validPass = hashService.VerifyPasswordHash (password, userPassword);

            if (!validPass)
            {
                view.DisplayLoginError(true);
                return;
            }
            
            // kailangan malaman kung anong type ng user sya pala
            // user exists and valid na ung password na binigay so login na
            
            // TODO Create dito ung Login Session
            view.UserLoginSuccess ();

            //loginSession = SessionFactory.CreateSession <LoginSession> (SessionKeys.LoginKey);
            // loginSession.Login(username, type);
        }
    }
}
