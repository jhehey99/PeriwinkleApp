using Android.OS;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;
using PeriwinkleApp.Android.Source.Presenters.ClientPresenters;

namespace PeriwinkleApp.Android.Source.Views.Fragments.ClientFragments
{
    public interface IClientHomeHeadView
    {
        void DisplayHeadInfo (string name, string username);
    }

    public class ClientHomeHeadView : Fragment, IClientHomeHeadView
    {
        private TextView txtCliHomeHeadName, txtCliHomeHeadUname;

        private IClientHomeHeadPresenter presenter;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
            presenter = new ClientHomeHeadPresenter (this);
		}
        
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            return inflater.Inflate(Resource.Layout.client_frag_home_head, container, false);
        }

        public override async void OnViewCreated (View view, Bundle savedInstanceState)
        {
            base.OnViewCreated (view, savedInstanceState);

			// Ung Allow Survey
			view.FindViewById<View>(Resource.Id.divider).Visibility = ViewStates.Gone;
			view.FindViewById<LinearLayout>(Resource.Id.ll_allow).Visibility = ViewStates.Gone;

			// Textviews
            txtCliHomeHeadName = view.FindViewById<TextView>(Resource.Id.txt_cli_home_head_name);
            txtCliHomeHeadUname = view.FindViewById<TextView>(Resource.Id.txt_cli_home_head_uname);

			await presenter.LoadLoggedClient();
            presenter.LoadHeadInfo ();
        }

        #region IClientHomeHeadView
        
        public void DisplayHeadInfo (string name, string username)
        {
            txtCliHomeHeadName.Text = name;
            txtCliHomeHeadUname.Text = username;
        }
        
        #endregion

    }
}
