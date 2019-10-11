using System;
using Android;
using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V4.View;
using Android.Support.V4.Widget;
using Android.Support.V7.App;
using Android.Views;

namespace Production.LeftDrawerLayout
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true)]
    public class MainActivity : AppCompatActivity, NavigationView.IOnNavigationItemSelectedListener
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_main);
            Android.Support.V7.Widget.Toolbar toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);

            FloatingActionButton fab = FindViewById<FloatingActionButton>(Resource.Id.fab);
            fab.Click += FabOnClick;

            DrawerLayout drawer = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            ActionBarDrawerToggle toggle = new ActionBarDrawerToggle(this, drawer, toolbar, Resource.String.navigation_drawer_open, Resource.String.navigation_drawer_close);
            drawer.AddDrawerListener(toggle);
            toggle.SyncState();

            NavigationView navigationView = FindViewById<NavigationView>(Resource.Id.nav_view);
            navigationView.SetNavigationItemSelectedListener(this);

            //var ft = SupportFragmentManager.BeginTransaction();
            //ft.Add(Resource.Id.frame_content, new AdminClientsFragment());
            //ft.Add(Resource.Id.frame_content, new AdminConsultantsFragment());
            //ft.Commit();
        }

        public override void OnBackPressed()
        {
            DrawerLayout drawer = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            if(drawer.IsDrawerOpen(GravityCompat.Start))
            {
                drawer.CloseDrawer(GravityCompat.Start);
            }
            else
            {
                base.OnBackPressed();
            }
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu_main, menu);
            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            int id = item.ItemId;
            if (id == Resource.Id.action_settings)
            {
                return true;
            }

            return base.OnOptionsItemSelected(item);
        }

        private void FabOnClick(object sender, EventArgs eventArgs)
        {
            View view = (View) sender;
            Snackbar.Make(view, "Replace with your own action", Snackbar.LengthLong)
                .SetAction("Action", (Android.Views.View.IOnClickListener)null).Show();
        }

        public bool OnNavigationItemSelected(IMenuItem item)
        {
            int id = item.ItemId;
            //var ft = SupportFragmentManager.BeginTransaction();

            switch (id)
            {
                case Resource.Id.nav_admin_home:
                    SupportActionBar.SetTitle(Resource.String.home_title);
                    break;

                case Resource.Id.nav_admin_clients:
                    SupportActionBar.SetTitle(Resource.String.clients_title);
                    //ft.Replace(Resource.Id.frame_content, new AdminClientsFragment()).AddToBackStack(null).Commit();
                    break;

                case Resource.Id.nav_admin_consultants:
                    SupportActionBar.SetTitle(Resource.String.consultants_title);
                    //ft.Replace(Resource.Id.frame_content, new AdminConsultantsFragment()).AddToBackStack(null).Commit();
                    break;

                case Resource.Id.nav_admin_pending:
                    SupportActionBar.SetTitle(Resource.String.pending_title);
                    break;
            }

            /*
            if (id == Resource.Id.nav_camera)
            {
                // Handle the camera action
            }
            else if (id == Resource.Id.nav_gallery)
            {
                SupportActionBar.SetTitle(Resource.String.gallery_title);
            }
            else if (id == Resource.Id.nav_slideshow)
            {
                SupportActionBar.SetTitle(Resource.String.slideshow_title);
            }
            else if (id == Resource.Id.nav_manage)
            {
                SupportActionBar.SetTitle(Resource.String.manage_title);
            }
            else if (id == Resource.Id.nav_share)
            {
                SupportActionBar.SetTitle(Resource.String.share_title);
            }
            else if (id == Resource.Id.nav_send)
            {
                SupportActionBar.SetTitle(Resource.String.send_title);
            }
            */

            DrawerLayout drawer = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            drawer.CloseDrawer(GravityCompat.Start);
            return true;
        }
    }
}

