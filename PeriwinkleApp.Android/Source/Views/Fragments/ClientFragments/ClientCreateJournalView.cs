using Android.OS;
using Android.Support.V4.App;
using Android.Views;

namespace PeriwinkleApp.Android.Source.Views.Fragments.ClientFragments
{
	public class ClientCreateJournalView : Fragment
	{
		public override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);
		}

		public override View OnCreateView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			return inflater.Inflate (Resource.Layout.client_frag_journal_create, container, false);
		}
	}
}
