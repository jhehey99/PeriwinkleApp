using System;
using Android.App;
using Android.OS;
using Android.Text;
using Android.Views;
using Android.Widget;
using PeriwinkleApp.Core.Sources.Models.Domain;
using PeriwinkleApp.Core.Sources.Utils;
using Production.RegisterLogin.Source.Presenters.Common;
using AlertDialog = Android.Support.V7.App.AlertDialog;
using Fragment = Android.Support.V4.App.Fragment;


namespace Production.RegisterLogin.Source
{
    public interface IRegisterView
    {
        void ShowInvalidMessage (string message);
        void DisplayResponse (string subject, string message);
    }

    public class RegisterView : Fragment, DatePickerDialog.IOnDateSetListener, IRegisterView
    {
        private EditText txt_reg_fname,
                         txt_reg_lname,
                         txt_reg_uname,
                         txt_reg_pass,
                         txt_reg_email,
                         txt_reg_contact,
                         txt_reg_bday;

        private TextView txt_reg_errfname,
                         txt_reg_errlname,
                         txt_reg_erruname,
                         txt_reg_errpass,
                         txt_reg_erremail,
                         txt_reg_errcontact;

        private TextView activeTextView;

        private Button btn_pick_bday,
                       btn_reg_create;

        private Spinner spin_gender;

        private IRegisterPresenter presenter;

        private AccountType registerType;

        private DateTime? birthday;

        private bool canRegister;

        public RegisterView (AccountType registerType) : base ()
        {
            this.registerType = registerType;
        }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            presenter = new RegisterPresenter (this);
            //TODO I NEED TO KNOW THE ACCOUNT TYPE TO BE REGISTERED
            //TODO: COMING FROM THE CALLING ACTIVITY OR FRAGMENT NALANG
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            return inflater.Inflate(Resource.Layout.register, container, false);
        }

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);

            // get references to the edit texts
            // Edit Texts
            txt_reg_fname = view.FindViewById<EditText>(Resource.Id.txt_reg_fname);
            txt_reg_lname = view.FindViewById<EditText>(Resource.Id.txt_reg_lname);
            txt_reg_uname = view.FindViewById<EditText>(Resource.Id.txt_reg_uname);
            txt_reg_pass = view.FindViewById<EditText>(Resource.Id.txt_reg_pass);
            txt_reg_email = view.FindViewById<EditText>(Resource.Id.txt_reg_email);
            txt_reg_contact = view.FindViewById<EditText>(Resource.Id.txt_reg_contact);
            txt_reg_bday = view.FindViewById<EditText>(Resource.Id.txt_reg_bday);

            // Edit Texts' TextChanged events
            txt_reg_fname.TextChanged += OnFirstNameTextChanged;
            txt_reg_lname.TextChanged += OnLastNameTextChanged;
            txt_reg_uname.TextChanged += OnUsernameTextChanged;
            txt_reg_pass.TextChanged += OnPasswordTextChanged;
            txt_reg_email.TextChanged += OnEmailTextChanged;
            txt_reg_contact.TextChanged += OnContactTextChanged;

            // Text Views for the errors
            txt_reg_errfname = view.FindViewById<TextView>(Resource.Id.txt_reg_errfname);
            txt_reg_errlname = view.FindViewById<TextView>(Resource.Id.txt_reg_errlname);
            txt_reg_erruname = view.FindViewById<TextView>(Resource.Id.txt_reg_erruname);
            txt_reg_errpass = view.FindViewById<TextView>(Resource.Id.txt_reg_errpass);
            txt_reg_erremail = view.FindViewById<TextView>(Resource.Id.txt_reg_erremail);
            txt_reg_errcontact = view.FindViewById<TextView>(Resource.Id.txt_reg_errcontact);

            // Spinner
            spin_gender = view.FindViewById<Spinner>(Resource.Id.spin_gender);

            // Buttons
            btn_pick_bday = view.FindViewById<Button>(Resource.Id.btn_pick_bday);
            btn_reg_create = view.FindViewById<Button>(Resource.Id.btn_reg_create);

            // Buttons' Clicked events
            btn_pick_bday.Click += OnPickBirthdayClicked;
            btn_reg_create.Click += OnCreateAccountClicked;

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
            HideErrorTextView(txt_reg_errfname);
            presenter.ValidateFirstName(e.Text.ToString());
        }

        private void OnLastNameTextChanged(object sender, TextChangedEventArgs e)
        {
            HideErrorTextView(txt_reg_errlname);
            presenter.ValidateLastName (e.Text.ToString ());
        }

        private void OnUsernameTextChanged (object sender, TextChangedEventArgs e)
        {
            HideErrorTextView (txt_reg_erruname);
            presenter.ValidateUsername(e.Text.ToString ());
        }

        private void OnPasswordTextChanged(object sender, TextChangedEventArgs e)
        {
            HideErrorTextView(txt_reg_errpass);
            presenter.ValidatePassword (e.Text.ToString ());
        }
        
        private void OnEmailTextChanged(object sender, TextChangedEventArgs e)
        {
            HideErrorTextView(txt_reg_erremail);
            presenter.ValidateEmail(e.Text.ToString());
        }

        private void OnContactTextChanged(object sender, TextChangedEventArgs e)
        {
            HideErrorTextView(txt_reg_errcontact);
            presenter.ValidateContact(e.Text.ToString());
        }

    #endregion

    #region Button Clicks

        private void OnCreateAccountClicked(object sender, EventArgs e)
        {
            if(!canRegister) //TODO Display error rin kasi may problema pa eh
                return;
            
            string genders = "MF";
            char? gender = genders[spin_gender.SelectedItemPosition];

            //TODO Kailangan ung accountType mula sa calling home page
            registerType = AccountType.Consultant;
            
            Account account = new Account()
                              {
                                  Username = txt_reg_uname.Text,
                                  FirstName = txt_reg_fname.Text,
                                  LastName = txt_reg_lname.Text,
                                  Email = txt_reg_email.Text,
                                  Contact = txt_reg_contact.Text,
                                  Birthday = birthday,
                                  Gender = gender,
                                  AccTypeId = registerType
            };

            Logger.Debug(account);

            // pass string rawPassword
            string plainPassword = txt_reg_pass.Text;
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
            Logger.Log (subject + "\n" + message);
        }
        
        #endregion

        #region IOnDateSetListener

        public void OnDateSet(DatePicker view, int year, int month, int dayOfMonth)
        {
            string date = year.ToString () + "-" + month.ToString () + "-" + dayOfMonth.ToString ();
            birthday = DateTime.Parse(date);
            txt_reg_bday.Text = date;
        }
        
    #endregion

    }
}
