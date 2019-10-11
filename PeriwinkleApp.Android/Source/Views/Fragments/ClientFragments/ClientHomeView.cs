using System.Threading.Tasks;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Views;
using PeriwinkleApp.Android.Source.Presenters.ClientPresenters;
using Fragment = Android.Support.V4.App.Fragment;
using FragmentTransaction = Android.Support.V4.App.FragmentTransaction;

namespace PeriwinkleApp.Android.Source.Views.Fragments.ClientFragments
{
    public class ClientHomeView : Fragment
    {
		private IClientHomePresenter presenter;

        public override async void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
			presenter = new ClientHomePresenter ();

			bool isLoaded = await presenter.LoadLoggedClient ();

			if(!isLoaded)
				CantLoad ();
		}

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            return inflater.Inflate(Resource.Layout.client_frag_home, container, false);
        }

        public override void OnViewCreated (View view, Bundle savedInstanceState)
        {
            base.OnViewCreated (view, savedInstanceState);
            FragmentTransaction ft = Activity.SupportFragmentManager.BeginTransaction();

            Fragment headFragment = new ClientHomeHeadView ();
            ft.Replace (Resource.Id.cli_home_framehead, headFragment);

			Fragment bodyFragment = new ClientHomeBodyView();
			ft.Replace(Resource.Id.cli_home_framebody, bodyFragment);
			
			ft.Commit();
        }

		public void CantLoad ()
		{
			Snackbar.Make(View, "Unable to Load Account Information", Snackbar.LengthLong).Show();
			Activity.Finish();
		}
	}
}
