using System;
using System.Collections.Generic;
using Android.Content;
using Android.Support.V4.App;
using PeriwinkleApp.Android.Source.AdapterModels;
using PeriwinkleApp.Android.Source.Adapters;
using PeriwinkleApp.Android.Source.Presenters.ClientPresenters;
using PeriwinkleApp.Android.Source.Session;
using PeriwinkleApp.Android.Source.Views.Activities;
using PeriwinkleApp.Android.Source.Views.Fragments.Common;
using PeriwinkleApp.Core.Sources.Models.Domain;
using PeriwinkleApp.Core.Sources.Utils;

namespace PeriwinkleApp.Android.Source.Views.Fragments.ClientFragments
{
	public interface IClientAccelerometerView
	{
		void DisplayRecordFiles(List<BehaviorAdapterModel> recordDataset);
		void LaunchViewRecord(AccelerometerRecord record);
	}

	public class ClientAccelerometerViewTest : RecyclerFragment<BehaviorRecyclerAdapter, BehaviorAdapterModel>,
												IClientAccelerometerView
	{
		IClientAccelerometerListPresenter presenter;
		private readonly string sessionKeyToUse;

		public ClientAccelerometerViewTest(string sessionKeyToUse = null)
		{
			this.sessionKeyToUse = sessionKeyToUse;
		}

		protected override void OnCreateInitialize()
		{
			IsAnimated = true;
			presenter = new ClientAccelerometerListPresenter(this, sessionKeyToUse);
		}

		protected override int ResourceLayout => Resource.Layout.list_frag_generic;
		protected override int? ResourceIdProgressBar => Resource.Id.list_frag_gen_progress;
		protected override int? ResourceIdLinearLayout => Resource.Id.list_frag_gen_linear;
		protected override int? ResourceIdRecyclerView => Resource.Id.list_frag_gen_recyclerView;
		protected override int? ResourceIdFab => Resource.Id.list_frag_gen_fab;
		protected override async void LoadInitialDataSet()
		{
			await presenter.GetAllAccelerometerRecords();
		}

		protected override void OnFloatingActionButtonClicked(object sender, EventArgs e)
		{
			base.OnFloatingActionButtonClicked(sender, e);

			Intent intent = new Intent(Context, typeof(ClientAccelerometerActivity));
			StartActivityForResult(intent, 2);
		}

		public override void OnActivityResult(int requestCode, int resultCode, Intent data)
		{
			base.OnActivityResult(requestCode, resultCode, data);
			Activity.OnBackPressed();
		}

		public void DisplayRecordFiles(List<BehaviorAdapterModel> recordDataset)
		{
			UpdateAdapterDataSet(recordDataset);
			HideProgressBar();
		}

		public void LaunchViewRecord(AccelerometerRecord record)
		{
			Logger.Log("View Accelerometer record");
			/*
			FragmentTransaction ft = Activity.SupportFragmentManager.BeginTransaction();
			Fragment fragment = new ClientViewBehaviorView(behaviorGraph);

			ft.Replace(Resource.Id.fragment_container, fragment);
			ft.AddToBackStack(null);
			ft.Commit();
			*/
		}

		
	}
}
 