using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Support.V7.App;
using Android.Support.Design.Widget;
using Android.Support.V4.Widget;


namespace Prod.DrawerLayout.Resources
{
    [Activity(Label = "DrawerActivity", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true)]
    public class DrawerActivity : AppCompatActivity, NavigationView.IOnNavigationItemSelectedListener
    {
        
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.drawer_main);

            // Navigation View
            NavigationView navigationView = FindViewById<NavigationView>(Resource.Id.nav_view);
            navigationView.SetNavigationItemSelectedListener(this);
        }

        public bool OnNavigationItemSelected(IMenuItem menuItem)
        {
            menuItem.SetChecked(true);


            throw new NotImplementedException();
        }
    }
}