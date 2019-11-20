using Android.OS;
using PeriwinkleApp.Android.Source.Cache;
using PeriwinkleApp.Android.Source.Factories;
using PeriwinkleApp.Core.Sources.Utils;
using Fragment = Android.Support.V4.App.Fragment;

namespace PeriwinkleApp.Android.Source.Views.Fragments.Common
{
	public class LogoutView : Fragment
	{
		public override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			SessionFactory.ClearSession();
			CacheProvider.Clear();
			Logger.Log("Exiting Application");
			Activity.FinishAffinity();
		}
	}
}