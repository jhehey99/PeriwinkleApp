using Android.App;
using PeriwinkleApp.Android.Source.Cache;
using PeriwinkleApp.Android.Source.Presenters.AdminPresenters;
using PeriwinkleApp.Core.Sources.Models.Domain;


namespace PeriwinkleApp.Android.Source.Views.Activities
{
	public interface IAdminMainActivity
	{
		void SetNavHeaderDetails(string name, string email);
	}

	[Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = false)]
	public class AdminMainActivity : NavigationDrawerActivity, IAdminMainActivity
	{
		protected override int? ResourceIdToolbar => Resource.Id.toolbar;
		protected override int? ResourceIdDrawerLayout => Resource.Id.admin_drawer_layout;
		protected override int? ResourceIdNavView => Resource.Id.admin_nav_view;
		protected override int? ResourceLayout => Resource.Layout.activity_admin_main;
		protected override AccountType? MyAccountType => AccountType.Admin;

		private IAdminMainPresenter presenter;

		protected override void OnCreateInitialize()
		{
			presenter = new AdminMainPresenter(this);
			presenter.LoadLoggedAdmin();
			presenter.LoadNavHeaderDetails();
		}

		protected override void OnStartInitialize()
		{
			base.OnStartInitialize();
			if (CacheProvider.IsSet(CacheKey.BackFragItem))
			{
				int backFragItem = CacheProvider.Get<int>(CacheKey.BackFragItem);
				ResourceIdDefaultItem = backFragItem;
			}
			else
			{
				ResourceIdDefaultItem = Resource.Id.menu_nav_home;
			}
		}


		public void SetNavHeaderDetails(string name, string email)
		{
			TxtNavHeaderName.Text = name;
			TxtNavHeaderEmail.Text = email;
		}
	}
}