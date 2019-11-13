using System;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using PeriwinkleApp.Android.Source.Presenters.Common;
using PeriwinkleApp.Android.Source.Views.Activities;
using PeriwinkleApp.Core.Sources.Models.Domain;
using Fragment = Android.Support.V4.App.Fragment;
using ViewStates = Android.Views.ViewStates;
using Android.App;


namespace PeriwinkleApp.Android.Source.Views.Fragments.Common
{
    public interface ILoginView
    {
        void DisplayLoginError (bool show);
        void UserLoginSuccess (AccountType type);
    }

    public class LoginView : Fragment, ILoginView
    {
        private EditText txtLoginUname,
                         txtLoginPass;

        private TextView txtLoginError;

        private Button btnLoginSignin,
                       btnLoginRegcon;

        private ILoginPresenter presenter;

		private AlertDialog loading_alert;

		bool submitted = false;

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
            txtLoginUname = view.FindViewById<EditText>(Resource.Id.txt_login_uname);
            txtLoginPass = view.FindViewById<EditText>(Resource.Id.txt_login_pass);

            // Text View
            txtLoginError = view.FindViewById<TextView>(Resource.Id.txt_login_error);
            txtLoginError.Visibility = ViewStates.Gone;

            // Button
            btnLoginSignin = view.FindViewById<Button>(Resource.Id.btn_login_signin);
            btnLoginRegcon = view.FindViewById<Button>(Resource.Id.btn_login_regcon);
            
            btnLoginSignin.Click += OnSignInClicked;
            btnLoginRegcon.Click += OnRegisterConsultantClicked;

			// Loading
			AlertDialog.Builder builder = new AlertDialog.Builder(this.Context);
			builder.SetView(Resource.Layout.alert_loading);
			builder.SetTitle("Logging in");
			builder.SetMessage("Please wait, logging in...");
			loading_alert = builder.Create();
		}

        #region Button Clicks

        private async void OnSignInClicked(object sender, EventArgs e)
        {
			if (submitted)
				return;

			submitted = true;
			loading_alert.Show();

			string username = txtLoginUname.Text;
            string password = txtLoginPass.Text;

            await presenter.PerformLogin (username, password);
			loading_alert.Dismiss();
			submitted = false;
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
            txtLoginError.Visibility = show ? ViewStates.Visible : ViewStates.Gone;
        }

        public void UserLoginSuccess (AccountType type)
        {
            // punta na sa next activity
            //TODO Palitan ung activity, for test, register activity muna haha
            //TODO ClientMainActivity

            Intent intent = null;
            
            switch (type)
            {
                case AccountType.Admin:
					intent = new Intent(Context, typeof(AdminMainActivity));
					break;

                case AccountType.Client:
                    intent = new Intent(Context, typeof(ClientMainActivity));
                    break;

				case AccountType.Consultant:
                    intent = new Intent(Context, typeof(ConsultantMainActivity));
                    break;
                    //TODO NOT IMPLEMENTED TO HA
                default: throw new ArgumentOutOfRangeException (nameof (type), type, null);
            }
            
            StartActivity(intent);
            Activity.Finish();
        }

    #endregion
    }
}
