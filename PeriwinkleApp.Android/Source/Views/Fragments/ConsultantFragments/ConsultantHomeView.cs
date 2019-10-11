using Android.OS;
using Android.Support.V4.App;
using Android.Views;

namespace PeriwinkleApp.Android.Source.Views.Fragments.ConsultantFragments
{
    public class ConsultantHomeView : Fragment
    {
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            return inflater.Inflate(Resource.Layout.consultant_frag_home, container, false);
        }

		public override void OnViewCreated (View view, Bundle savedInstanceState)
		{
			base.OnViewCreated (view, savedInstanceState);

			FragmentTransaction ft = Activity.SupportFragmentManager.BeginTransaction();

			Fragment headFragment = new ConsultantHomeHeadView();
			ft.Replace(Resource.Id.con_home_framehead, headFragment);

			ft.Commit ();
		}
	}
}
