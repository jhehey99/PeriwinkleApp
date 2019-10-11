using System;
using Android.Content;
using Android.OS;
using Android.Support.V4.App;
using Android.Views;
using Android.Views.InputMethods;
using PeriwinkleApp.Android.Source.Views.Fragments.Common;
using PeriwinkleApp.Core.Sources.Models.Domain;
using v4App = Android.Support.V4.App;

namespace PeriwinkleApp.Android.Source.Views.Fragments.ConsultantFragments
{
    public class ConsultantNewClientView : Fragment
    {
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
            v4App.FragmentTransaction ft = Activity.SupportFragmentManager.BeginTransaction();
            v4App.Fragment fragment = new RegisterView(AccountType.Client);
            
            ft.Replace(Resource.Id.register_frame, fragment);
            ft.Commit();
        }
        
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            return inflater.Inflate(Resource.Layout.register_fragment_layout, container, false);
        }
    }
}
