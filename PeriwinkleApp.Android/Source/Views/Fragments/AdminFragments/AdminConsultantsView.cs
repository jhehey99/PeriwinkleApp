using System;
using System.Collections.Generic;
using Android.Support.V4.App;
using PeriwinkleApp.Android.Source.AdapterModels;
using PeriwinkleApp.Android.Source.Adapters;
using PeriwinkleApp.Android.Source.Factories;
using PeriwinkleApp.Android.Source.Presenters.AdminPresenters;
using PeriwinkleApp.Android.Source.Session;
using PeriwinkleApp.Android.Source.Views.Fragments.Common;
using PeriwinkleApp.Core.Sources.Models.Domain;
using PeriwinkleApp.Core.Sources.Utils;

namespace PeriwinkleApp.Android.Source.Views.Fragments.AdminFragments
{
	public interface IAdminConsultantsView
	{
		void DisplayConsultantsList(List<AccountAdapterModel> accountDataSet);
	}

    public class AdminConsultantsView : RecyclerFragment<AccountRecyclerAdapter, AccountAdapterModel>,
											IAdminConsultantsView
    {
		private IAdminConsultantsPresenter presenter;

		private ConsultantSession viewConSession;

        protected override void OnCreateInitialize ()
		{
			IsAnimated = true;

			presenter = new AdminConsultantsPresenter(this);

			// create session sa pagvi-view ng consultant profile
			viewConSession = SessionFactory.CreateSession<ConsultantSession>(SessionKeys.AdminConsultantsKey);
        }

		protected override int ResourceLayout => Resource.Layout.list_frag_generic;
		protected override int? ResourceIdProgressBar => Resource.Id.list_frag_gen_progress;
		protected override int? ResourceIdLinearLayout => Resource.Id.list_frag_gen_linear;
		protected override int? ResourceIdRecyclerView => Resource.Id.list_frag_gen_recyclerView;

        protected override async void LoadInitialDataSet ()
		{
			// eto ung pagkuha ng consultants' list
			await presenter.GetAllConsultants();
        }

		protected override void OnItemClick (object sender, int position)
		{
			base.OnItemClick (sender, position);

			// kuha sa presenter kung sinong consultant ung cinlick
			Consultant consultant;

			try
			{
				consultant = presenter.Consultants[position];
			}
			catch (ArgumentOutOfRangeException e)
			{
				Logger.Log(e.Message);
				consultant = null;
			}

			// walang nakuha
			if (consultant == null)
				return;

			// gawa tayo session ng i-viview na consultant profile
			viewConSession?.AddConsultantSession(consultant);

			// dapat ung SupportFragmentManager gagamitin
			FragmentTransaction ft = Activity.SupportFragmentManager.BeginTransaction();

			// Profile Fragment View, ung may head at body
			Fragment fragment = new ProfileView(AccountType.Consultant);

			ft.Replace(Resource.Id.fragment_container, fragment);
			ft.AddToBackStack(null);
			ft.Commit();
        }

#region IAdminConsultantsView

		public void DisplayConsultantsList(List<AccountAdapterModel> accountDataSet)
		{
			UpdateAdapterDataSet(accountDataSet);
			HideProgressBar();
        }

        #endregion

    }
}
