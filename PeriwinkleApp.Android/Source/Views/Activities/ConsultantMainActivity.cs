using Android.App;
using Android.Content;
using PeriwinkleApp.Android.Source.Presenters.ConsultantPresenters;
using PeriwinkleApp.Android.Source.Views.Fragments.ConsultantFragments;
using PeriwinkleApp.Core.Sources.Models.Domain;

namespace PeriwinkleApp.Android.Source.Views.Activities
{
	public interface IConsultantMainActivity
	{
		void SetNavHeaderDetails(string name, string email);
		void LaunchPendingConsultantActivity ();
	}

    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = false)]
    public class ConsultantMainActivity : NavigationDrawerActivity, IConsultantMainActivity
    {
		//		protected override int? ResourceIdFab => Resource.Id.fab;

        protected override int? ResourceIdToolbar => Resource.Id.toolbar;
		protected override int? ResourceIdDrawerLayout => Resource.Id.consul_drawer_layout;
		protected override int? ResourceIdNavView => Resource.Id.consul_nav_view;
		protected override int? ResourceLayout => Resource.Layout.activity_consultant_main;
		protected override AccountType? MyAccountType => AccountType.Consultant;

        private IConsultantMainPresenter presenter;

        protected override async void OnCreateInitialize ()
		{
			// Access ung Login Session
			// TODO Mula sa presenter nito, kukunin natin ung session
			presenter = new ConsultantMainPresenter(this);

			// ensure that the logged consultant session is loaded
			await presenter.LoadSession ();

			// pag pending consultant, launch ung other activity, then mag finish tong activity na to.
			presenter.DetermineIsPendingConsultant ();

			// Legit consultant na pag umabot dito
            presenter.LoadNavHeaderDetails();

            //			await presenter.LoadNavHeaderDetails();
            //TODO DETERMINE IF PENDING PALANG THEN IBANG VIEW ILOAD SA KANYA
        }

		protected override void OnStartInitialize ()
		{
			ResourceIdDefaultItem = Resource.Id.con_menu_home;
        }

        #region IConsultantMainActivity

        public void SetNavHeaderDetails (string name, string email)
		{
            // TODO: DETERMINE KUNG REGISTERED CONSULTANT NA 
            // TODO: OR PENDING PALANG. PAG PENDING PALANG MAG LAGAY NALANG TAYO NG DISPLAY
            // TODO: INDICATING NA PENDING SYA AND WALA PA SYA KAYANG GAWIN AND WAIT FOR EMAIL
			TxtNavHeaderName.Text = name;
			TxtNavHeaderEmail.Text = email;
        }

		public void LaunchPendingConsultantActivity ()
		{
			//TODO PALITAN MAIN ACTIVITY
			Intent intent = new Intent(ApplicationContext, typeof(ConsultantPendingMainActivity));
			StartActivity (intent);
			Finish ();
//			throw new System.NotImplementedException ();
		}

		#endregion
    }
}
