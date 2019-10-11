using Android.App;
using Android.OS;
using Android.Support.V7.App;

namespace PeriwinkleApp.Android.Source.Views.Activities
{
	[Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = false)]
    public class ConsultantPendingMainActivity : AppCompatActivity
    {
		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);
			SetContentView (Resource.Layout.consultant_frag_pending_home);
		}

    }
}
