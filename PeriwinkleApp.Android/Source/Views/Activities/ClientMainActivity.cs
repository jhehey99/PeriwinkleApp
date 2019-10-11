using Android.App;
using PeriwinkleApp.Android.Source.Cache;
using PeriwinkleApp.Android.Source.Presenters.ClientPresenters;
using PeriwinkleApp.Core.Sources.Models.Domain;

namespace PeriwinkleApp.Android.Source.Views.Activities
{
	public interface IClientMainActivity
	{
		void SetNavHeaderDetails(string name, string email);
	}

    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = false)]
    public class ClientMainActivity : NavigationDrawerActivity, IClientMainActivity
    {
		//		protected override int? ResourceIdFab => Resource.Id.fab;

        protected override int? ResourceIdToolbar => Resource.Id.toolbar;
		protected override int? ResourceIdDrawerLayout => Resource.Id.client_drawer_layout;
		protected override int? ResourceIdNavView => Resource.Id.client_nav_view;
		protected override int? ResourceLayout => Resource.Layout.activity_client_main;
//        protected override int? ResourceIdDefaultItem => Resource.Id.cli_menu_home;
        protected override AccountType? MyAccountType => AccountType.Client;

		private IClientMainPresenter presenter;

        protected override void OnCreateInitialize ()
		{
			presenter = new ClientMainPresenter(this);
			presenter.LoadLoggedClient ();
//			await presenter.LoadSession ();
			presenter.LoadNavHeaderDetails();
        }

		protected override void OnStartInitialize ()
		{
			base.OnStartInitialize ();
			if (CacheProvider.IsSet(CacheKey.BackFragItem))
			{
				int backFragItem = CacheProvider.Get<int>(CacheKey.BackFragItem);
				ResourceIdDefaultItem = backFragItem;
			}
			else
			{
				ResourceIdDefaultItem = Resource.Id.cli_menu_home;
			}
        }

		public void SetNavHeaderDetails (string name, string email)
		{
			TxtNavHeaderName.Text = name;
			TxtNavHeaderEmail.Text = email;
		}
	}
}
