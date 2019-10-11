using Android.App;
using Android.Content;
using Android.Support.V7.App;
using Android.Views;
using Android.Views.InputMethods;
using PeriwinkleApp.Android.Source.Views.Interfaces;

namespace PeriwinkleApp.Android.Source.Views.Activities
{
	[Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = false)]
    public class HideSoftInputActivity : AppCompatActivity, IHideSoftInput
    {
		public override bool OnTouchEvent (MotionEvent e)
		{
			HideSoftInput ();
			return base.OnTouchEvent (e);
		}

		public void HideSoftInput ()
		{
			InputMethodManager imm = (InputMethodManager)GetSystemService(Context.InputMethodService);
			imm.HideSoftInputFromWindow(Window.DecorView.WindowToken, HideSoftInputFlags.NotAlways);
		}
	}
}
