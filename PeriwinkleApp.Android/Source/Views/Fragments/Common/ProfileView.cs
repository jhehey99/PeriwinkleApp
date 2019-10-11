using System.Collections.Generic;
using Android.OS;
using Android.Support.V4.App;
using Android.Views;
using PeriwinkleApp.Android.Source.Dictionaries;
using PeriwinkleApp.Android.Source.Views.Fragments.AdminFragments;
using PeriwinkleApp.Core.Sources.Models.Domain;

namespace PeriwinkleApp.Android.Source.Views.Fragments.Common
{
    public class ProfileView : Fragment
    {
        public AccountType ProfileType { get; set; }

        private readonly IDictionary<AccountType, Fragment> bodyFragmentMap =
            new Dictionary<AccountType, Fragment>()
            {
                { AccountType.Client, new AdminClientProfileTabView() },
                { AccountType.Consultant, new AdminConsultantProfileTabView () }
            };

        public ProfileView (AccountType profileType) : base ()
        {
            ProfileType = profileType;
        }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            return inflater.Inflate(Resource.Layout.profile_frag, container, false);
        }
        
        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);

            FragmentTransaction ft = Activity.SupportFragmentManager.BeginTransaction();

            // Profile Head
            Fragment headFragment = new ProfileHeadView(ProfileType);
            ft.Add(Resource.Id.fragment_profile_head, headFragment);
            ft.AddToBackStack(null);
            
            // Profile Body
            Fragment bodyFragment = bodyFragmentMap.ContainsKey(ProfileType) ? bodyFragmentMap[ProfileType] : null;

            if (bodyFragment != null)
            {
                ft.Add(Resource.Id.fragment_profile_body, bodyFragment);
                ft.AddToBackStack(null);
            }
            
            ft.Commit();
        }
    }
}