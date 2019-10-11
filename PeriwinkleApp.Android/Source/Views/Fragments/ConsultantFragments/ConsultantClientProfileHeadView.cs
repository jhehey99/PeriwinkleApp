using System;
using Android.OS;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;
using PeriwinkleApp.Android.Source.Presenters.ConsultantPresenters;

namespace PeriwinkleApp.Android.Source.Views.Fragments.ConsultantFragments
{
	public interface IConsultantClientProfileHeadView
	{
		void DisplayHeadInfo (string name, string username);
	}

    public class ConsultantClientProfileHeadView : Fragment, 
												   CompoundButton.IOnCheckedChangeListener,
												   IConsultantClientProfileHeadView
    {
		private TextView txtHeadName, txtHeadUname;
		private Switch swAllow;
		private IConsultantClientProfileHeadPresenter presenter;

		public override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			// Create your fragment here
			presenter = new ConsultantClientProfileHeadPresenter(this);
		}

		public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			// Use this to return your custom view for this Fragment
			return inflater.Inflate(Resource.Layout.client_frag_home_head, container, false);
		}

		public override async void OnViewCreated(View view, Bundle savedInstanceState)
		{
			base.OnViewCreated(view, savedInstanceState);

			// Ung Allow Survey
			view.FindViewById <View> (Resource.Id.divider).Visibility = ViewStates.Visible;
			view.FindViewById<LinearLayout>(Resource.Id.ll_allow).Visibility = ViewStates.Visible;

			// Text Views
            txtHeadName = view.FindViewById<TextView>(Resource.Id.txt_cli_home_head_name);
			txtHeadUname = view.FindViewById<TextView>(Resource.Id.txt_cli_home_head_uname);

			// Switch para sa allow survey
			swAllow = view.FindViewById <Switch> (Resource.Id.sw_allow);
			swAllow.SetOnCheckedChangeListener (this);
			
			// nasa Session.ViewClient na ung client na i-viview, i-get nalang natin ung information nya 
			await presenter.LoadClientToView ();
			presenter.LoadHeadInfo();
		}
		
//		private async void OnAllowClientTakeMbes (object sender, EventArgs e)
//		{
//			bool allow = true;
//
//			await presenter.AllowClientTakeMbes (allow);
//
//            // presenter.
//            // display client may now be able to take the mbes again
//        }

        #region IConsultantClientProfileHeadView

        public void DisplayHeadInfo (string name, string username)
		{
			txtHeadName.Text = name;
			txtHeadUname.Text = username;
		}

        #endregion

		public async void OnCheckedChanged (CompoundButton buttonView, bool isChecked)
		{
			await presenter.AllowClientTakeMbes(isChecked);
		}
	}
}
