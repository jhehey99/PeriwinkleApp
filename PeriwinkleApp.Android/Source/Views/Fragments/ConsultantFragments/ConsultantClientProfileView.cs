using Android.OS;
using Android.Support.V4.App;
using Android.Views;
using PeriwinkleApp.Android.Source.Views.Fragments.ClientFragments;

namespace PeriwinkleApp.Android.Source.Views.Fragments.ConsultantFragments
{
	public class ConsultantClientProfileView : Fragment
	{
		public override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);
		}

		public override View OnCreateView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			return inflater.Inflate (Resource.Layout.consultant_frag_cliprofile, container, false);
		}
		
		public override void OnViewCreated (View view, Bundle savedInstanceState)
		{
			base.OnViewCreated (view, savedInstanceState);
			FragmentTransaction ft = Activity.SupportFragmentManager.BeginTransaction();

			Fragment headFragment = new ConsultantClientProfileHeadView();
			ft.Replace(Resource.Id.con_cli_home_framehead, headFragment);

			Fragment bodyFragment = new ConsultantClientProfileBodyView();
			ft.Replace(Resource.Id.con_cli_home_framebody, bodyFragment);

			ft.Commit();
        }
	}
}
