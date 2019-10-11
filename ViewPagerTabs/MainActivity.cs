using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Support.V7.Widget;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V4.View;
using v4App = Android.Support.V4.App;

namespace ViewPagerTabs
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity, TabLayout.IOnTabSelectedListener
    {
        ViewPager viewPager;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);

            Toolbar toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
            toolbar.SetTitle(Resource.String.app_name);

            TabLayout tabLayout = FindViewById<TabLayout>(Resource.Id.tablayout);
            TabItem tabChats = FindViewById<TabItem>(Resource.Id.tabChats);
            TabItem tabStatus = FindViewById<TabItem>(Resource.Id.tabStatus);
            TabItem tabCalls = FindViewById<TabItem>(Resource.Id.tabCalls);

            viewPager = FindViewById<ViewPager>(Resource.Id.viewPager);
            PageAdapter pageAdapter = new PageAdapter(SupportFragmentManager, tabLayout.TabCount);
            viewPager.Adapter = pageAdapter;
            viewPager.AddOnPageChangeListener(new TabLayout.TabLayoutOnPageChangeListener(tabLayout));

            tabLayout.AddOnTabSelectedListener(this);
        }

        public void OnTabReselected(TabLayout.Tab tab)
        {
            
        }

        public void OnTabSelected(TabLayout.Tab tab)
        {
            viewPager.SetCurrentItem(tab.Position, true);
        }

        public void OnTabUnselected(TabLayout.Tab tab)
        {
            
        }
    }
}