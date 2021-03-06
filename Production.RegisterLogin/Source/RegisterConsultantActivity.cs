﻿using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Views;
using Android.Views.InputMethods;
using Android.Widget;
using PeriwinkleApp.Core.Sources.Models.Domain;
using v4App = Android.Support.V4.App;

namespace Production.RegisterLogin.Source
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = false)]
    public class RegisterConsultantActivity : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Set our view from the "main" layout resource
            //TODO PALITAN UNG LAYOUT
            SetContentView(Resource.Layout.activity_main);

            Window.SetSoftInputMode (SoftInput.StateHidden);
            
            v4App.FragmentTransaction ft = SupportFragmentManager.BeginTransaction();
            v4App.Fragment fragment = new RegisterView(AccountType.Consultant);
            
            ft.Replace(Resource.Id.fragment_container, fragment);
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
