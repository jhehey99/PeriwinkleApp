using System;
using System.Globalization;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Text;
using Android.Views;
using Android.Views.InputMethods;
using Android.Widget;
using PeriwinkleApp.Android.Source.Presenters.Common;
using PeriwinkleApp.Core.Sources.Models.Domain;
using PeriwinkleApp.Core.Sources.Utils;
using AlertDialog = Android.Support.V7.App.AlertDialog;
using Fragment = Android.Support.V4.App.Fragment;


namespace PeriwinkleApp.Android.Source.Views.Fragments.Common
{
    public interface IRegisterView
    {
        void ShowInvalidMessage (string message);
        void DisplayResponse (string subject, string message);
		void FinishRegister();
    }

    public class RegisterView : HideSoftInputFragment, DatePickerDialog.IOnDateSetListener, IRegisterView
    {
        private EditText txtRegFname,
                         txtRegLname,
                         txtRegUname,
                         txtRegPass,
                         txtRegEmail,
                         txtRegContact,
                         txtRegBday;

        private TextView txtRegErrfname,
                         txtRegErrlname,
                         txtRegErruname,
                         txtRegErrpass,
                         txtRegErremail,
                         txtRegErrcontact;

        private TextView activeTextView;

        private Button btnPickBday,
                       btnRegCreate;

        private Spinner spinGender;

        private IRegisterPresenter presenter;

        private AccountType registerType;

        private DateTime? birthday;

        private bool canRegister;

		private AlertDialog alertDialog;


        public RegisterView (AccountType registerType) : base ()
        {
            this.registerType = registerType;
        }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            presenter = new RegisterPresenter (this);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            return inflater.Inflate(Resource.Layout.register_view, container, false);
        }

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);

            // Edit Texts
            txtRegFname = view.FindViewById<EditText>(Resource.Id.txt_reg_fname);
            txtRegLname = view.FindViewById<EditText>(Resource.Id.txt_reg_lname);
            txtRegUname = view.FindViewById<EditText>(Resource.Id.txt_reg_uname);
            txtRegPass = view.FindViewById<EditText>(Resource.Id.txt_reg_pass);
            txtRegEmail = view.FindViewById<EditText>(Resource.Id.txt_reg_email);
            txtRegContact = view.FindViewById<EditText>(Resource.Id.txt_reg_contact);
            txtRegBday = view.FindViewById<EditText>(Resource.Id.txt_reg_bday);

            // Edit Texts' TextChanged events
            txtRegFname.TextChanged += OnFirstNameTextChanged;
            txtRegLname.TextChanged += OnLastNameTextChanged;
            txtRegUname.TextChanged += OnUsernameTextChanged;
            txtRegPass.TextChanged += OnPasswordTextChanged;
            txtRegEmail.TextChanged += OnEmailTextChanged;
            txtRegContact.TextChanged += OnContactTextChanged;

            // Text Views for the errors
            txtRegErrfname = view.FindViewById<TextView>(Resource.Id.txt_reg_errfname);
            txtRegErrlname = view.FindViewById<TextView>(Resource.Id.txt_reg_errlname);
            txtRegErruname = view.FindViewById<TextView>(Resource.Id.txt_reg_erruname);
            txtRegErrpass = view.FindViewById<TextView>(Resource.Id.txt_reg_errpass);
            txtRegErremail = view.FindViewById<TextView>(Resource.Id.txt_reg_erremail);
            txtRegErrcontact = view.FindViewById<TextView>(Resource.Id.txt_reg_errcontact);

            // Spinner
            spinGender = view.FindViewById<Spinner>(Resource.Id.spin_gender);

            // Buttons
            btnPickBday = view.FindViewById<Button>(Resource.Id.btn_pick_bday);
            btnRegCreate = view.FindViewById<Button>(Resource.Id.btn_reg_create);

            // Buttons' Clicked events
            btnPickBday.Click += OnPickBirthdayClicked;
            btnRegCreate.Click += OnCreateAccountClicked;
            
            //TODO ETO NA NEXT, BDAY AT GENDER
            // TODO THEN UNG REGISTERE BUTTON CLICK
            // for gender, get item first char
        }

        private void HideErrorTextView(TextView txtView)
        {
            // gone muna sya, pag may problem, may throw ng exception ung service
            // saka nya i-show ung error message
            canRegister = true;
            activeTextView = txtView;
            activeTextView.Visibility = ViewStates.Gone;
        }

        #region Text Changed

        private void OnFirstNameTextChanged (object sender, TextChangedEventArgs e)
        {
            HideErrorTextView(txtRegErrfname);
            presenter.ValidateFirstName(e.Text.ToString());
        }

        private void OnLastNameTextChanged(object sender, TextChangedEventArgs e)
        {
            HideErrorTextView(txtRegErrlname);
            presenter.ValidateLastName (e.Text.ToString ());
        }

        private void OnUsernameTextChanged (object sender, TextChangedEventArgs e)
        {
            HideErrorTextView (txtRegErruname);
            presenter.ValidateUsername(e.Text.ToString ());
        }

        private void OnPasswordTextChanged(object sender, TextChangedEventArgs e)
        {
            HideErrorTextView(txtRegErrpass);
            presenter.ValidatePassword (e.Text.ToString ());
        }
        
        private void OnEmailTextChanged(object sender, TextChangedEventArgs e)
        {
            HideErrorTextView(txtRegErremail);
            presenter.ValidateEmail(e.Text.ToString());
        }

        private void OnContactTextChanged(object sender, TextChangedEventArgs e)
        {
            HideErrorTextView(txtRegErrcontact);
            presenter.ValidateContact(e.Text.ToString());
        }

        #endregion

        #region Button Clicks

        private void OnCreateAccountClicked(object sender, EventArgs e)
        {
            if(!canRegister) //TODO Display error rin kasi may problema pa eh
                return;
            
            string genders = "MF";
            char? gender = genders[spinGender.SelectedItemPosition];

            //TODO Kailangan ung accountType mula sa calling home page
//            registerType = AccountType.Consultant;
            
            Account account = new Account()
                              {
                                  Username = txtRegUname.Text,
                                  FirstName = txtRegFname.Text,
                                  LastName = txtRegLname.Text,
                                  Email = txtRegEmail.Text,
                                  Contact = txtRegContact.Text,
                                  Birthday = birthday,
                                  Gender = gender,
                                  AccTypeId = registerType
            };

            Logger.Debug(account);

            // pass string rawPassword
            string plainPassword = txtRegPass.Text;
            // TODO REGISTER NA
            presenter.PerformRegistration(account, plainPassword);
        }

        private void OnPickBirthdayClicked(object sender, EventArgs e)
        {
            var dialog = new DatePickerDialogFragment (Activity, DateTime.Now, this);
            dialog.Show (FragmentManager, null);
        }

        #endregion

        #region IRegisterView

        public void ShowInvalidMessage (string message)
        {
            canRegister = false;
            activeTextView.Text = message;
            activeTextView.Visibility = ViewStates.Visible;
        }

		public void DisplayResponse (string subject, string message)
		{
			//TODO KUNIN MO UNG NASA ADMINPENDINGVIEW.CS
//            AlertDialog.Builder builder = new AlertDialog.Builder(this.Activity);
//            builder.SetCustomTitle(dialogTitleTextView);
//            builder.SetMessage(Resource.String.pending_confirm_msg);

			AlertDialog.Builder builder = new AlertDialog.Builder (this.Context);
			builder.SetTitle (subject);
			builder.SetMessage (message);

			builder.SetPositiveButton ("OK", AlertDialogOkClicked);

			alertDialog = builder.Create ();
			alertDialog.Show ();
			Logger.Log (subject + "\n" + message);
		}

		public void FinishRegister()
		{
			this.Activity.Finish();
		}

		private void AlertDialogOkClicked (object sender, DialogClickEventArgs e)
		{
			alertDialog.Hide ();
		}

#endregion

        #region IOnDateSetListener

        public void OnDateSet(DatePicker view, int year, int month, int dayOfMonth)
        {
            // ung month kasi 0-based
            birthday = new DateTime(year, month + 1, dayOfMonth);
            txtRegBday.Text = birthday.Value.ToShortDateString ();
        }
        
        #endregion

    }
}
