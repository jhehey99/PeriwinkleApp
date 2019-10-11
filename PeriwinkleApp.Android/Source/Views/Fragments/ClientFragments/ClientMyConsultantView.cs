using Android.OS;
using Android.Support.V4.App;
using Android.Views;
using PeriwinkleApp.Android.Source.Views.Fragments.ConsultantFragments;

namespace PeriwinkleApp.Android.Source.Views.Fragments.ClientFragments
{
    public class ClientMyConsultantView : Fragment
    {
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            return inflater.Inflate(Resource.Layout.client_frag_myconsultant, container, false);
        }

		public override void OnViewCreated (View view, Bundle savedInstanceState)
		{
			base.OnViewCreated (view, savedInstanceState);

			FragmentTransaction ft = Activity.SupportFragmentManager.BeginTransaction();

			Fragment headFragment = new ClientMyConsultantHeadView();
			ft.Replace(Resource.Id.cli_mycon_framehead, headFragment);

			//TODO BODYFRAGMENT
			Fragment bodyFragment = new ClientMyConsultantBodyView ();
			ft.Replace (Resource.Id.cli_mycon_framebody, bodyFragment);

			ft.Commit();
        }
    }
}
