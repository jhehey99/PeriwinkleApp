using Android.OS;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;
using PeriwinkleApp.Android.Source.Presenters.ConsultantPresenters;

namespace PeriwinkleApp.Android.Source.Views.Fragments.ConsultantFragments
{
	public interface IConsultantHomeHeadView
    {
		void DisplayHeadInfo(string name, string username);
	}

	public class ConsultantHomeHeadView : Fragment, IConsultantHomeHeadView
	{
		private TextView txtConHomeHeadName, txtConHomeHeadUname;	
		private IConsultantHomeHeadPresenter presenter;

        public override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			// Create your fragment here
			presenter = new ConsultantHomeHeadPresenter(this);
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

			//TODO ASYNC MO KO
			await presenter.LoadSession ();
			presenter.LoadHeadInfo();
		}

#region IConentHomeHeadView

		public void DisplayHeadInfo(string name, string username)
		{
			txtConHomeHeadName.Text = name;
			txtConHomeHeadUname.Text = username;
		}

#endregion

	}
}
