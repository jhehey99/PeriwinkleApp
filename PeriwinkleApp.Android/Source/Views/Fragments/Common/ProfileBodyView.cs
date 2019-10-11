using Android.OS;
using Android.Support.V4.App;
using Android.Views;
using PeriwinkleApp.Android.Source.Views.Fragments.AdminFragments;

namespace PeriwinkleApp.Android.Source.Views.Fragments.Common
{
    public class ProfileBodyView : Fragment
    {
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            return inflater.Inflate(Resource.Layout.profile_body_frag, container, false);
        }

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);

            FragmentTransaction ft = Activity.SupportFragmentManager.BeginTransaction();

            Fragment clientProfileFragment = new AdminClientProfileTabView();

            ft.Replace(Resource.Id.fragment_body_content, clientProfileFragment);
            ft.AddToBackStack(null);

            ft.Commit();
        }
    }
}