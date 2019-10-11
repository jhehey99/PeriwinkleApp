using Android.OS;
using Android.Support.Design.Widget;
using Android.Support.V4.App;
using Android.Support.V4.View;
using Android.Views;
using PeriwinkleApp.Android.Source.Adapters;

namespace PeriwinkleApp.Android.Source.Views.Fragments.AdminFragments
{
    public class AdminClientProfileTabView : Fragment, TabLayout.IOnTabSelectedListener
    {
        private ViewPager viewPager;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            return inflater.Inflate(Resource.Layout.admin_client_profile_tab, container, false);
        }

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);

            TabLayout tabLayout = view.RootView.FindViewById<TabLayout>(Resource.Id.adcli_profile_tabLayout);
            TabItem tabAbout = view.RootView.FindViewById<TabItem>(Resource.Id.adcli_profile_tabAbout);
            TabItem tabActivity = view.RootView.FindViewById<TabItem>(Resource.Id.adcli_profile_tabActivity);
            TabItem tabSurvey = view.RootView.FindViewById<TabItem>(Resource.Id.adcli_profile_tabSurvey);

            viewPager = view.RootView.FindViewById<ViewPager>(Resource.Id.adcli_profile_viewPager);
            AdminClientPageAdapter pageAdapter = new AdminClientPageAdapter(FragmentManager, tabLayout.TabCount);

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
