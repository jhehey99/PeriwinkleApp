using System;
using System.Collections.Generic;
using Android.Support.V4.App;
using Android.Views;
using PeriwinkleApp.Android.Source.AdapterModels;
using PeriwinkleApp.Android.Source.Adapters;
using PeriwinkleApp.Android.Source.Presenters.ClientFragments;
using PeriwinkleApp.Android.Source.Presenters.ClientPresenters;
using PeriwinkleApp.Android.Source.Views.Fragments.Common;
using PeriwinkleApp.Core.Sources.Models.Domain;
using PeriwinkleApp.Core.Sources.Utils;

namespace PeriwinkleApp.Android.Source.Views.Fragments.ClientFragments
{
	public interface IClientMbesListView
	{
		void DisplayResponses(List<ResponseAdapterModel> responseDataset);
		void LaunchViewResponse(Mbes mbes);
	}

	public class ClientMbesListView : RecyclerFragment<ResponseRecyclerAdapter, ResponseAdapterModel>,
									  IClientMbesListView
	{
		private IClientMbesListPresenter presenter;

		protected override void OnCreateInitialize()
		{
			IsAnimated = true;
			presenter = new ClientMbesListPresenter(this);
		}

		protected override int ResourceLayout => Resource.Layout.list_frag_generic;
		protected override int? ResourceIdProgressBar => Resource.Id.list_frag_gen_progress;
		protected override int? ResourceIdLinearLayout => Resource.Id.list_frag_gen_linear;
		protected override int? ResourceIdRecyclerView => Resource.Id.list_frag_gen_recyclerView;
		protected override int? ResourceIdFab => Resource.Id.list_frag_gen_fab;

		protected override async void LoadInitialDataSet()
		{
			await presenter.GetAllMbes();
		}

		protected override void OnItemClick(object sender, int position)
		{
			base.OnItemClick(sender, position);
			presenter.ViewMbesClicked(sender, position);
		}

		public void DisplayResponses(List<ResponseAdapterModel> responseDataset)
		{
			UpdateAdapterDataSet(responseDataset);
			HideProgressBar();
		}

		public void LaunchViewResponse(Mbes mbes)
		{
			Logger.Log("LaunchViewResponse");
			//TODO Dito ung Pag view nung journal
			FragmentTransaction ft = Activity.SupportFragmentManager.BeginTransaction();
			Fragment fragment = new ClientViewResponseView(mbes);

			ft.Replace(Resource.Id.fragment_container, fragment);
			ft.AddToBackStack(null);
			ft.Commit();
		}

		protected override void SetViewReferences(View view)
		{
			base.SetViewReferences(view);
			RfFab.Visibility = ViewStates.Gone;
		}
	}
}