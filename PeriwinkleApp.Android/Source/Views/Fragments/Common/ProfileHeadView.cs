using System.Collections.Generic;
using Android.OS;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;
using PeriwinkleApp.Android.Source.Factories;
using PeriwinkleApp.Android.Source.Session;
using PeriwinkleApp.Android.Source.Views.Fragments.AdminFragments;
using PeriwinkleApp.Core.Sources.Models.Domain;

namespace PeriwinkleApp.Android.Source.Views.Fragments.Common
{
    public class ProfileHeadView : Fragment
    {
        private TextView txtProfileName, txtProfileUsername;

        private readonly AccountSession accountSession;

        private readonly IDictionary <AccountType, string> profileHeadSessionMap =
            new Dictionary <AccountType, string> ()
            {
                { AccountType.Client, SessionKeys.AdminClientsKey },
                { AccountType.Consultant, SessionKeys.AdminConsultantsKey }
            };

        public ProfileHeadView (AccountType type)
        {
            string key = profileHeadSessionMap.ContainsKey (type) ? profileHeadSessionMap[type] : null;

            // accountSession is null, when key is null
            accountSession = SessionFactory.ReadSession <AccountSession> (key);
        }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment

            return inflater.Inflate(Resource.Layout.profile_head_frag, container, false);
        }

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);

            // Find the TextViews
            txtProfileName = view.RootView.FindViewById<TextView>(Resource.Id.txtProfileName);
            txtProfileUsername = view.RootView.FindViewById<TextView>(Resource.Id.txtProfileUserName);

            // TODO: TRY Gawan ng default value pag walang nakaset
            // TODO: Like, N/A or something

            // check muna kung null, then check kung naset
            if (accountSession == null || !accountSession.IsSet)
                return;

            // Set TextViews' Texts
            txtProfileName.Text = accountSession.Username;
            txtProfileUsername.Text = accountSession.AccountId.ToString ();
            // TODO: Account Type text
        }

    }
}