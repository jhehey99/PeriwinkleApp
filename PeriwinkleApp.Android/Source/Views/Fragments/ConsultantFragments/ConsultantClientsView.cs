using System.Collections.Generic;
using Android.Support.V4.App;
using PeriwinkleApp.Android.Source.AdapterModels;
using PeriwinkleApp.Android.Source.Adapters;
using PeriwinkleApp.Android.Source.Presenters.ConsultantPresenters;
using PeriwinkleApp.Android.Source.Session;
using PeriwinkleApp.Android.Source.Views.Fragments.Common;

namespace PeriwinkleApp.Android.Source.Views.Fragments.ConsultantFragments
{
	public interface IConsultantClientsView
	{
		void DisplayMyClientsList(List<AccountAdapterModel> accountDataSet);
		void LaunchClientProfile ();
	}

    public class ConsultantClientsView : RecyclerFragment<AccountRecyclerAdapter, AccountAdapterModel>,
											 IConsultantClientsView
    {
		private IConsultantClientsPresenter presenter;

        protected override void OnCreateInitialize ()
		{
			IsAnimated = true;
			presenter = new ConsultantClientsPresenter(this);
        }

        protected override int ResourceLayout => Resource.Layout.list_frag_generic;
        protected override int? ResourceIdProgressBar => Resource.Id.list_frag_gen_progress;
        protected override int? ResourceIdLinearLayout => Resource.Id.list_frag_gen_linear;
		protected override int? ResourceIdRecyclerView => Resource.Id.list_frag_gen_recyclerView;

		protected override async void LoadInitialDataSet ()
		{
			await presenter.GetAllMyClients ();
		}

		protected override void OnItemClick (object sender, int position)
		{
			base.OnItemClick (sender, position);

			presenter.ViewClientAt (position);
		}


#region IConsultantClientsView

        public void DisplayMyClientsList(List <AccountAdapterModel> accountDataSet)
		{
			UpdateAdapterDataSet(accountDataSet);
			HideProgressBar();
		}

		public void LaunchClientProfile ()
		{
			FragmentTransaction ft = Activity.SupportFragmentManager.BeginTransaction();

			// TODO HEADER AND BODY
			Fragment f = new ConsultantClientProfileView ();
			ft.Replace (Resource.Id.fragment_container, f);
			ft.AddToBackStack (null);
			ft.Commit ();
		}

        #endregion
    }
}
