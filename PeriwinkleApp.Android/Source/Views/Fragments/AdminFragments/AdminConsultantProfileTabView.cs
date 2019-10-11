using Android.OS;
using Android.Support.V4.App;
using Android.Support.Design.Widget;
using Android.Support.V4.View;
using Android.Views;
using PeriwinkleApp.Android.Source.Adapters;

namespace PeriwinkleApp.Android.Source.Views.Fragments.AdminFragments
{
    public class AdminConsultantProfileTabView : Fragment, TabLayout.IOnTabSelectedListener
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
            return inflater.Inflate(Resource.Layout.admin_consultant_profile_tab, container, false);
        }

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);

            TabLayout tabLayout = view.RootView.FindViewById<TabLayout>(Resource.Id.adcon_profile_tabLayout);
            TabItem tabAbout = view.RootView.FindViewById<TabItem>(Resource.Id.adcon_profile_tabAbout);
            TabItem tabClients = view.RootView.FindViewById<TabItem>(Resource.Id.adcon_profile_tabClients);

            viewPager = view.RootView.FindViewById<ViewPager>(Resource.Id.adcon_profile_viewPager);
            AdminConsultantPageAdapter pageAdapter = new AdminConsultantPageAdapter(FragmentManager, tabLayout.TabCount);

            viewPager.Adapter = pageAdapter;
            viewPager.AddOnPageChangeListener(new TabLayout.TabLayoutOnPageChangeListener(tabLayout));

            tabLayout.AddOnTabSelectedListener(this);
        }

        public void OnTabReselected (TabLayout.Tab tab)
        {
            
        }

        public void OnTabSelected (TabLayout.Tab tab)
        {
            viewPager.SetCurrentItem(tab.Position, true);
        }

        public void OnTabUnselected (TabLayout.Tab tab)
        {
            
        }
    }
}
