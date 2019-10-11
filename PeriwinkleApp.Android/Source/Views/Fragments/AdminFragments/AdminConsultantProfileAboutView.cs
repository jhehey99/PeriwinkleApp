using Android.OS;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;
using PeriwinkleApp.Android.Source.Factories;
using PeriwinkleApp.Android.Source.Presenters.AdminPresenters;
using PeriwinkleApp.Android.Source.Session;
using PeriwinkleApp.Core.Sources.Extensions;
using PeriwinkleApp.Core.Sources.Models.Domain;

namespace PeriwinkleApp.Android.Source.Views.Fragments.AdminFragments
{
    public interface IAdminConsultantProfileAboutView
    {
        void DisplayConsultantAboutProfile (Consultant consultant);
    }

    public class AdminConsultantProfileAboutView : Fragment, IAdminConsultantProfileAboutView
    {
        private TextView txtEmail;
        private TextView txtContact;
        private TextView txtGender;
        private TextView txtBirthday;

        private ConsultantSession viewConSession;

        private IAdminConsultantProfileAboutPresenter presenter;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
            presenter = new AdminConsultantProfileAboutPresenter (this);

            // read natin ung session sa pagviview ng client profile
            viewConSession = SessionFactory.ReadSession <ConsultantSession> (SessionKeys.AdminConsultantsKey);      //(AdminConsultantsView.SessionKey);

        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            return inflater.Inflate(Resource.Layout.admin_consultant_profile_about, container, false);
        }

        public override async void OnViewCreated(View view, Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);

            // Set up TextViews
            txtEmail = view.FindViewById<TextView>(Resource.Id.adcon_about_email);
            txtContact = view.FindViewById<TextView>(Resource.Id.adcon_about_contact);
            txtGender = view.FindViewById<TextView>(Resource.Id.adcon_about_gender);
            txtBirthday = view.FindViewById<TextView>(Resource.Id.adcon_about_birthday);

            txtEmail.Text = viewConSession?.Username;
            txtContact.Text = viewConSession?.ConsultantId.ToString();

            // kunin na natin ung consultant profile
            await presenter.GetConsultantProfileAsync(viewConSession?.Username);
        }

        public void DisplayConsultantAboutProfile (Consultant consultant)
        {
            // sa presenter to i-cacall para maset natin sa view

            txtEmail.Text = "Email Address: " + consultant?.Email;
            txtContact.Text = "Contact Number: " + consultant?.Contact;
            txtGender.Text = "Gender: " + consultant?.Gender.GenderString();
            txtBirthday.Text = "Birthday: " + consultant?.Birthday?.ToShortDateString();
        }
    }
}
