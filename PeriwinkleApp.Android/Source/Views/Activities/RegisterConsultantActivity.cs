using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.V7.App;
using Android.Views;
using Android.Views.InputMethods;
using PeriwinkleApp.Android.Source.Views.Fragments.Common;
using PeriwinkleApp.Core.Sources.Models.Domain;
using v4App = Android.Support.V4.App;

namespace PeriwinkleApp.Android.Source.Views.Activities
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = false)]
    public class RegisterConsultantActivity : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Set our view from the "main" layout resource

            SetContentView(Resource.Layout.register_fragment_layout);

            Window.SetSoftInputMode (SoftInput.StateHidden);
            
            v4App.FragmentTransaction ft = SupportFragmentManager.BeginTransaction();
            v4App.Fragment fragment = new RegisterView(AccountType.Consultant);
            
            ft.Replace(Resource.Id.register_frame, fragment);
            ft.Commit();
        }

        public override bool OnTouchEvent (MotionEvent e)
        {
            InputMethodManager imm = (InputMethodManager)GetSystemService(Context.InputMethodService);
            imm.HideSoftInputFromWindow(Window.DecorView.WindowToken, 0);
            return base.OnTouchEvent (e);
        }

        public override void OnBackPressed ()
        {
            base.OnBackPressed ();
            Intent intent = new Intent (this, typeof (LoginActivity));
            intent.SetFlags (ActivityFlags.ClearTop);
            StartActivity (intent);
        }
    }
}
