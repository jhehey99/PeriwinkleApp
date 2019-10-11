using Android.Content;
using Android.OS;
using Android.Support.V4.App;
using Android.Views;
using Android.Views.InputMethods;
using PeriwinkleApp.Android.Source.Views.Interfaces;

namespace PeriwinkleApp.Android.Source.Views.Fragments.Common
{
    public class HideSoftInputFragment : Fragment, View.IOnTouchListener, IHideSoftInput
    {
        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);
            view.SetOnTouchListener (this);
        }

        public bool OnTouch (View v, MotionEvent e)
        {
            HideSoftInput ();
            return true;
        }
		
		public void HideSoftInput ()
		{
			InputMethodManager imm = (InputMethodManager)Activity.GetSystemService(Context.InputMethodService);
			imm.HideSoftInputFromWindow(View.WindowToken, HideSoftInputFlags.NotAlways);
        }
    }
}
