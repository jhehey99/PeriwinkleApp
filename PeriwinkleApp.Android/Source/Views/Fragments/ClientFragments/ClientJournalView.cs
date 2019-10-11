using System;
using Android.Content;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Support.V4.App;
using Android.Views;
using PeriwinkleApp.Android.Source.Views.Fragments.Common;
using PeriwinkleApp.Core.Sources.Models.Domain;

namespace PeriwinkleApp.Android.Source.Views.Fragments.ClientFragments
{
	//TODO TANGAL MO KO
    public class ClientJournalView : Fragment
	{
		private FloatingActionButton journalFab;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            return inflater.Inflate(Resource.Layout.client_frag_journal, container, false);
        }

		public override void OnViewCreated (View view, Bundle savedInstanceState)
		{
			base.OnViewCreated (view, savedInstanceState);

			journalFab = view.FindViewById <FloatingActionButton> ( Resource.Id.journal_fab);
			journalFab.Click += OnJournalFabClicked;
		}

		private void OnJournalFabClicked (object sender, EventArgs e)
		{
//			Intent intent = new Intent (Context, typeof (ClientJournalCreateView));
//			StartActivity (intent);
			FragmentTransaction ft = Activity.SupportFragmentManager.BeginTransaction();
			Fragment fragment = new ClientJournalCreateView ();

			ft.Replace (Resource.Id.fragment_container, fragment);
			ft.AddToBackStack (null);
			ft.Commit();
		}
		
	}
}
