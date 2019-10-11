using Android.App;
using Android.OS;
using Android.Support.V7.App;
using PeriwinkleApp.Android.Source.Views.Fragments.ClientFragments;
using v4App = Android.Support.V4.App;

namespace PeriwinkleApp.Android.Source.Views.Activities
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = false)]
    public class CheckListMainActivity : AppCompatActivity
    {
        protected override void OnCreate (Bundle savedInstanceState)
        {
            base.OnCreate (savedInstanceState);

			//TODO: NEW RESOURCE LAYOUT FOR CHECKLIST WITH A FRAGMENT CONTAINER

            SetContentView(Resource.Layout.client_checklist_activity);

            v4App.FragmentTransaction ft = SupportFragmentManager.BeginTransaction();
            v4App.Fragment fragment = new ClientCheckListInstructionView();

            ft.Replace(Resource.Id.fragment_container, fragment);
            ft.Commit();
        }
    }
}
