using System;
using System.Collections.Generic;
using Android.Content;
using Android.Support.V4.App;
using PeriwinkleApp.Android.Source.AdapterModels;
using PeriwinkleApp.Android.Source.Adapters;
using PeriwinkleApp.Android.Source.Presenters.ClientPresenters;
using PeriwinkleApp.Android.Source.Views.Activities;
using PeriwinkleApp.Android.Source.Views.Fragments.Common;
using PeriwinkleApp.Core.Sources.Models.Domain;
using PeriwinkleApp.Core.Sources.Utils;

namespace PeriwinkleApp.Android.Source.Views.Fragments.ClientFragments
{
	public interface IClientViewRecordsListView
	{
		void DisplayRecords(List<SensorRecordAdapterModel> dataset);
		void LaunchViewRecord(SensorRecord record);
	}

	public class ClientViewRecordsListView : RecyclerFragment<SensorRecordRecyclerAdapter, SensorRecordAdapterModel>,
											 IClientViewRecordsListView
	{
		private IClientViewRecordsListPresenter presenter;
		private readonly string sessionKeyToUse;

		public ClientViewRecordsListView(string sessionKeyToUse = null)
		{
			this.sessionKeyToUse = sessionKeyToUse;
		}

		protected override void OnCreateInitialize()
		{
			IsAnimated = true;
			presenter = new ClientViewRecordsListPresenter(this, sessionKeyToUse);
		}

		protected override int ResourceLayout => Resource.Layout.list_frag_generic;
		protected override int? ResourceIdProgressBar => Resource.Id.list_frag_gen_progress;
		protected override int? ResourceIdLinearLayout => Resource.Id.list_frag_gen_linear;
		protected override int? ResourceIdRecyclerView => Resource.Id.list_frag_gen_recyclerView;
		protected override int? ResourceIdFab => Resource.Id.list_frag_gen_fab;

		protected override async void LoadInitialDataSet()
		{
			await presenter.GetAllRecords();
		}

		protected override void OnItemClick(object sender, int position)
		{
			base.OnItemClick(sender, position);
			presenter.ViewRecordClicked(sender, position);
		}

		protected override void OnFloatingActionButtonClicked(object sender, EventArgs e)
		{
			base.OnFloatingActionButtonClicked(sender, e);
			Logger.Log("ClientCreateRecordActivity");

			Intent intent = new Intent(Context, typeof(ClientNewRecordActivity));
			Activity.StartActivityForResult(intent, 1001);
		}

		public void DisplayRecords(List<SensorRecordAdapterModel> dataset)
		{
			UpdateAdapterDataSet(dataset);
			HideProgressBar();
		}

		public void LaunchViewRecord(SensorRecord record)
		{
			Logger.Log("LaunchViewRecord");
			FragmentTransaction ft = Activity.SupportFragmentManager.BeginTransaction();
			Fragment fragment = new ClientViewSensorRecordView(record);

			ft.Replace(Resource.Id.fragment_container, fragment);
			ft.AddToBackStack(null);
			ft.Commit();
		}
	}
}
 