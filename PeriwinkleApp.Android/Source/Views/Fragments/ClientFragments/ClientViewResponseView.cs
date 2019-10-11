
using Android.OS;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;
using PeriwinkleApp.Android.Source.Presenters.ClientPresenters;
using PeriwinkleApp.Core.Sources.Models.Domain;
using PeriwinkleApp.Core.Sources.Utils;

namespace PeriwinkleApp.Android.Source.Presenters.ClientFragments
{
	public interface IClientViewResponseView
	{
		void DisplayResponse(MbesResponse response, Score score);
	}

	public class ClientViewResponseView : Fragment, IClientViewResponseView
	{
		private TextView txtDate, txtHeight, txtWeight, txtBMI, txtBinge, txtBulimia, txtAnorexia;

		private readonly Mbes mbes;
		private IClientViewResponsePresenter presenter;
		private MbesResponse response;

		public ClientViewResponseView(Mbes mbes)
		{
			this.mbes = mbes;
		}

		public override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
			presenter = new ClientViewResponsePresenter(this);
		}

		public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			//TODO layout netong view response
			return inflater.Inflate(Resource.Layout.client_view_response_scores, container, false);
		}

		public override async void OnViewCreated(View view, Bundle savedInstanceState)
		{
			base.OnViewCreated(view, savedInstanceState);
			txtDate = view.FindViewById<TextView>(Resource.Id.score_weight);
			txtHeight = view.FindViewById<TextView>(Resource.Id.score_height);
			txtWeight = view.FindViewById<TextView>(Resource.Id.score_weight);
			txtBMI = view.FindViewById<TextView>(Resource.Id.score_bmi);
			txtBinge = view.FindViewById<TextView>(Resource.Id.score_binge);
			txtBulimia = view.FindViewById<TextView>(Resource.Id.score_bulimia);
			txtAnorexia = view.FindViewById<TextView>(Resource.Id.score_anorexia);

			await presenter.LoadResponse(mbes);
		}

		public void DisplayResponse(MbesResponse response, Score score)
		{
			Logger.Log("DISPLAY RESPONSE");

			txtDate.Text = mbes.DateCreated.ToShortDateString();
			txtHeight.Text = mbes.Height.Value.ToString() + "cm";
			txtWeight.Text = mbes.Weight.Value.ToString() + "kg";
			txtBMI.Text = mbes.BMI.Value.ToString("0.00");
			txtBinge.Text = score.BingeScore.ToString("0.00");
			txtBulimia.Text = score.BulimiaScore.ToString("0.00");
			txtAnorexia.Text = score.AnorexiaScore.ToString("0.00");
		}
	}
}