using System;
using Android.App;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Support.V4.View;
using Android.Support.V4.Widget;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using PeriwinkleApp.Android.Source.Dictionaries;
using PeriwinkleApp.Core.Sources.Models.Domain;
using Toolbar = Android.Support.V7.Widget.Toolbar;
using v4App = Android.Support.V4.App;


namespace PeriwinkleApp.Android.Source.Views.Activities
{
	[Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = false)]
    public abstract class NavigationDrawerActivity : HideSoftInputActivity, NavigationView.IOnNavigationItemSelectedListener
	{
        #region Protected Fields

        protected Toolbar NavToolbar;
		protected FloatingActionButton NavFab;
		protected DrawerLayout NavDrawerLayout;
		protected ActionBarDrawerToggle NavToggle;
		protected NavigationView NavView;
		protected View NavHeaderView;

		#endregion

        #region Toolbar

        protected abstract int? ResourceIdToolbar { get; }
		protected virtual void InitNavToolbar ()
		{
			if (ResourceIdToolbar == null)
				return;

			NavToolbar = FindViewById <Toolbar> ((int) ResourceIdToolbar);
			SetSupportActionBar (NavToolbar);
		}

        #endregion

		#region Floating Action Button

//		protected abstract int? ResourceIdFab { get; }

//        protected virtual void InitNavFab ()
//		{
//			if (ResourceIdFab == null)
//				return;
//
//			NavFab = FindViewById <FloatingActionButton> ((int) ResourceIdFab);
//			NavFab.Click += FabOnClick;
//		}
//
//		private void FabOnClick (object sender, EventArgs e)
//		{
//			View view = (View)sender;
//			Snackbar.Make (view, "Replace with your own action", Snackbar.LengthLong)
//					.SetAction ("Action", (View.IOnClickListener) null).Show ();
//		}

        #endregion

        #region Drawer

        protected abstract int? ResourceIdDrawerLayout { get; }
		protected virtual int NavDrawerOpen { get; } = Resource.String.navigation_drawer_open;
		protected virtual int NavDrawerClose { get; } = Resource.String.navigation_drawer_close;

        protected virtual void InitDrawer ()
		{
			if (ResourceIdDrawerLayout == null || ResourceIdDrawerLayout == null)
				return;

			NavDrawerLayout = FindViewById <DrawerLayout> ((int) ResourceIdDrawerLayout);
			NavToggle = new ActionBarDrawerToggle (this, NavDrawerLayout, NavToolbar, NavDrawerOpen, NavDrawerClose);
			NavDrawerLayout.AddDrawerListener (NavToggle);
			NavToggle.SyncState ();
		}

		protected void CloseNavDrawer ()
		{
			NavDrawerLayout.CloseDrawer (GravityCompat.Start);
        }

        #endregion

        #region Navigation View

        protected abstract int? ResourceIdNavView { get; }

		protected virtual void InitNavigationView ()
		{
			if (ResourceIdNavView == null)
				return;

			NavView = FindViewById <NavigationView> ((int) ResourceIdNavView);
			NavView.SetNavigationItemSelectedListener (this);
		}

        #endregion

        #region Nav Header

		protected TextView TxtNavHeaderName, TxtNavHeaderEmail;
        protected virtual int? ResourceIdHeaderName { get; } = Resource.Id.txt_navHeader_name;
		protected virtual int? ResourceIdHeaderEmail{ get; } = Resource.Id.txt_navHeader_email;

		protected virtual void InitNavHeader ()
		{
			if (ResourceIdHeaderName == null || ResourceIdHeaderEmail == null)
				return;

			NavHeaderView = NavView.GetHeaderView (0);
			TxtNavHeaderName = NavHeaderView.FindViewById<TextView>((int) ResourceIdHeaderName);
			TxtNavHeaderEmail = NavHeaderView.FindViewById<TextView>((int) ResourceIdHeaderEmail);
        }

        #endregion

        #region OnCreate

        protected abstract int? ResourceLayout { get; }

		protected abstract AccountType? MyAccountType { get; }

        protected abstract void OnCreateInitialize ();

		private NavigationItemMap navMap;

		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);

			if (ResourceLayout == null)
				throw new NotImplementedException();

			SetContentView((int)ResourceLayout);

			InitNavToolbar();
//			InitNavFab();
			InitDrawer();
			InitNavigationView();
			InitNavHeader();

			OnCreateInitialize();

			AccountType accountType = MyAccountType.GetValueOrDefault();
			navMap = new NavigationItemMap(accountType);
        }

        #endregion

        #region OnStart

		protected int? ResourceIdDefaultItem { get; set; }

		protected virtual void OnStartInitialize() { }

        protected virtual void SetDefaultDrawerItem ()
		{
			// Set the default navigation drawer item upon startup
			if (ResourceIdDefaultItem == null)
				return;

			IMenuItem item = NavView.Menu.FindItem ((int) ResourceIdDefaultItem);
			NavView.SetCheckedItem ((int) ResourceIdDefaultItem);
			OnNavigationItemSelected (item);
		}

		protected override void OnStart ()
		{
			base.OnStart ();
			OnStartInitialize();
            SetDefaultDrawerItem();
        }

        #endregion

        #region OnBackPressed

        public override void OnBackPressed ()
		{
			if (NavDrawerLayout.IsDrawerOpen(GravityCompat.Start))
			{
				NavDrawerLayout.CloseDrawer(GravityCompat.Start);
			}
			else
			{
				int count = SupportFragmentManager.BackStackEntryCount;

				if (count > 0)
					SupportFragmentManager.PopBackStack();
				else
					base.OnBackPressed();
			}
		}

        #endregion

        #region OnCreateOptionsMenu

		protected virtual int? ResourceMenuMain { get; } = Resource.Menu.menu_main;

		public override bool OnCreateOptionsMenu(IMenu menu)
		{
			if (ResourceMenuMain == null)
				return false;

			MenuInflater.Inflate ((int) ResourceMenuMain, menu);
			return true;
		}

        #endregion

        #region OnOptionsItemSelected

		public override bool OnOptionsItemSelected(IMenuItem item)
		{
			int id = item.ItemId;
			return id == Resource.Id.action_settings || base.OnOptionsItemSelected(item);
		}

        #endregion

        #region NavigationItem

		public bool OnNavigationItemSelected (IMenuItem menuItem)
		{
			int id = menuItem.ItemId;
			return SetNavigationItem (id);
        }

		#endregion

		public bool SetNavigationItem (int id)
		{
			v4App.FragmentTransaction ft = SupportFragmentManager.BeginTransaction();

			(int? titleId, v4App.Fragment fragment) = navMap.GetNavItem(id);

			if (titleId != null)
				SupportActionBar.SetTitle((int)titleId);

			if (fragment != null)
			{
				ft.Replace(Resource.Id.fragment_container, fragment);
				ft.Commit();
			}

			CloseNavDrawer();
			return true;
        }

	}
}
