using Android.OS;
using Android.Views;
using Android.Widget;
using Android.Support.V4.App;
using PeriwinkleApp.Android.Source.Session;
using PeriwinkleApp.Android.Source.Factories;
using PeriwinkleApp.Core.Sources.Models.Domain;
using PeriwinkleApp.Android.Source.Presenters.AdminPresenters;
using PeriwinkleApp.Core.Sources.Extensions;

namespace PeriwinkleApp.Android.Source.Views.Fragments.AdminFragments
{
    public interface IAdminClientProfileAboutView
    {
        void DisplayClientAboutProfile (Client client);
    }

    public class AdminClientProfileAboutView : Fragment, IAdminClientProfileAboutView
    {
        private TextView txtEmail;
        private TextView txtContact;
        private TextView txtGender;
        private TextView txtBirthday;

        private ClientSession viewCliSession;

        private IAdminClientProfileAboutPresenter presenter;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
            presenter = new AdminClientProfileAboutPresenter (this);

            // read natin ung session sa pagviview ng client profile
            viewCliSession = SessionFactory.ReadSession <ClientSession> (SessionKeys.AdminClientsKey); //(AdminClientsView.SessionKey);

        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            return inflater.Inflate(Resource.Layout.admin_client_profile_about, container, false);
        }

        public override async void OnViewCreated(View view, Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);

            // Set up TextViews
            txtEmail = view.FindViewById<TextView>(Resource.Id.adcli_about_email);
            txtContact = view.FindViewById<TextView>(Resource.Id.adcli_about_contact);
            txtGender = view.FindViewById<TextView>(Resource.Id.adcli_about_gender);
            txtBirthday = view.FindViewById<TextView>(Resource.Id.adcli_about_birthday);

            txtEmail.Text = viewCliSession?.Username;
            txtContact.Text = viewCliSession?.ClientId.ToString ();

            // kunin na natin ung client profile
            await presenter.GetClientProfileAsync (viewCliSession?.Username);
        }

        public void DisplayClientAboutProfile (Client client)
        {
            // sa presenter to i-cacall para maset natin sa view
            
            txtEmail.Text = "Email Address: " + client?.Email;
            txtContact.Text = "Contact Number: " + client?.Contact;
            txtGender.Text = "Gender: " + client?.Gender.GenderString ();
            txtBirthday.Text = "Birthday: " + client?.Birthday?.ToShortDateString ();
        } 
    }
}