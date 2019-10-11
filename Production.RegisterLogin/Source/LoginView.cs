using System;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using Production.RegisterLogin.Source.Presenters.Common;
using Fragment = Android.Support.V4.App.Fragment;
using ViewStates = Android.Views.ViewStates;


namespace Production.RegisterLogin.Source
{
    public interface ILoginView
    {
        void DisplayLoginError (bool show);
        void UserLoginSuccess ();
    }

    public class LoginView : Fragment, ILoginView
    {
        private EditText txt_login_uname,
                         txt_login_pass;

        private TextView txt_login_error;

        private Button btn_login_signin,
                       btn_login_regcon;

        private ILoginPresenter presenter;

        public override void OnCreate (Bundle savedInstanceState)
        {
            base.OnCreate (savedInstanceState);
            presenter = new LoginPresenter (this);
        }

        public override View OnCreateView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            return inflater.Inflate (Resource.Layout.login, container, false);
        }

        public override void OnViewCreated (View view, Bundle savedInstanceState)
        {
            base.OnViewCreated (view, savedInstanceState);

            // Edit Texts
            txt_login_uname = view.FindViewById<EditText>(Resource.Id.txt_login_uname);
            txt_login_pass = view.FindViewById<EditText>(Resource.Id.txt_login_pass);

            // Text View
            txt_login_error = view.FindViewById<TextView>(Resource.Id.txt_login_error);
            txt_login_error.Visibility = ViewStates.Gone;

            // Button
            btn_login_signin = view.FindViewById<Button>(Resource.Id.btn_login_signin);
            btn_login_regcon = view.FindViewById<Button>(Resource.Id.btn_login_regcon);
            
            btn_login_signin.Click += OnSignInClicked;
            btn_login_regcon.Click += OnRegisterConsultantClicked;
        }

        #region Button Clicks

        private void OnSignInClicked(object sender, EventArgs e)
        {
            string username = txt_login_uname.Text;
            string password = txt_login_pass.Text;

            presenter.PerformLogin (username, password);
        }

        private void OnRegisterConsultantClicked (object sender, EventArgs e)
        {
            Intent intent = new Intent (Context, typeof (RegisterConsultantActivity));
            StartActivity (intent);
            Activity.Finish ();
        }

        #endregion

        #region ILoginView

        public void DisplayLoginError (bool show)
        {
            txt_login_error.Visibility = show ? ViewStates.Visible : ViewStates.Gone;
        }

        public void UserLoginSuccess ()
        {
            // punta na sa next activity
            //TODO Palitan ung activity, for test, register activity muna haha
            //TODO ClientMainActivity
            
            Intent intent = new Intent(Context, typeof(RegisterConsultantActivity));
            StartActivity(intent);
            Activity.Finish();
        }

    #endregion
    }
}
