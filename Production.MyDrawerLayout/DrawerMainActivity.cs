using Android.App;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Support.V4.View;
using Android.Support.V4.Widget;
using Android.Support.V7.App;
using Android.Support.V7.Widget;
using Android.Views;
using ActionBar = Android.Support.V7.App.ActionBar;

namespace Production.MyDrawerLayout
{
    [Activity(Label = "DrawerMainActivity")]
    public class DrawerMainActivity : AppCompatActivity,
                                    NavigationView.IOnNavigationItemSelectedListener
    {
        private DrawerLayout drawerLayout;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.drawer_main);

            // Toolbar
            Toolbar toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);

            // Drawer Layout
            drawerLayout = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            ActionBarDrawerToggle toggle = new ActionBarDrawerToggle(this, drawerLayout, toolbar,
                                                                     Resource.String.navigation_drawer_open,
                                                                     Resource.String.navigation_drawer_close);
            drawerLayout.AddDrawerListener(toggle);
            toggle.SyncState();

            // Navigation View
            NavigationView navigationView = FindViewById<NavigationView>(Resource.Id.nav_view);
            navigationView.SetNavigationItemSelectedListener(this);

            // Action Bar
            ActionBar actionBar = SupportActionBar;
            actionBar.SetDisplayHomeAsUpEnabled(true);
            actionBar.SetHomeAsUpIndicator(Resource.Drawable.ic_menu_manage);
        }

        public bool OnNavigationItemSelected(IMenuItem menuItem)
        {
            // set item as selected to persist highlight
            menuItem.SetChecked(true);

            // close drawer when item is tapped
            drawerLayout.CloseDrawers();

            // Add code here to update the UI based on the item selected
            // For example, swap UI fragments here

            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.home:
                    drawerLayout.OpenDrawer(GravityCompat.Start);
                    return true;
            }

            return base.OnOptionsItemSelected(item);
        }
    }
}