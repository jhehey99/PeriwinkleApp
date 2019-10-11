using Android.OS;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;
using PeriwinkleApp.Android.Source.Presenters.ClientPresenters;

namespace PeriwinkleApp.Android.Source.Views.Fragments.ClientFragments
{
	public interface IClientMyConsultantHeadView
	{
		void DisplayHeadInfo(string name, string username);
    }


    public class ClientMyConsultantHeadView : Fragment, IClientMyConsultantHeadView
    {
		private TextView txtConHomeHeadName, txtConHomeHeadUname;
		private IClientMyConsultantPresenter presenter;

        public override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			// Create your fragment here
			presenter = new ClientMyConsultantPresenter(this);
		}

		public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			// Use this to return your custom view for this Fragment
			return inflater.Inflate(Resource.Layout.consultant_frag_home_head, container, false);
		}

		public override async void OnViewCreated(View view, Bundle savedInstanceState)
		{
			base.OnViewCreated(view, savedInstanceState);

			txtConHomeHeadName = view.FindViewById<TextView>(Resource.Id.txt_con_home_head_name);
			txtConHomeHeadUname = view.FindViewById<TextView>(Resource.Id.txt_con_home_head_uname);

			await presenter.LoadHeadInfo();
		}

        #region IClientMyConsultantHeadView

        public void DisplayHeadInfo (string name, string username)
		{
			txtConHomeHeadName.Text = name;
			txtConHomeHeadUname.Text = username;
		}
		
		#endregion
    }
}
