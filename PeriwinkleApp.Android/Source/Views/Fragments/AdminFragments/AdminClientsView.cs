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
	public interface IAdminClientsView
	{
		void DisplayClientsList (List <AccountAdapterModel> accountDataSet);
	}

    public class AdminClientsView : RecyclerFragment <AccountRecyclerAdapter, AccountAdapterModel>,
									IAdminClientsView
	{
		private IAdminClientsPresenter presenter;

		private ClientSession viewCliSession;

        protected override void OnCreateInitialize ()
		{
			IsAnimated = true;

			presenter = new AdminClientsPresenter(this);

			// create session sa pagvi-view ng client profile
			viewCliSession = SessionFactory.CreateSession<ClientSession>(SessionKeys.AdminClientsKey);
        }

		protected override int ResourceLayout => Resource.Layout.list_frag_generic;
		protected override int? ResourceIdProgressBar => Resource.Id.list_frag_gen_progress;
        protected override int? ResourceIdLinearLayout => Resource.Id.list_frag_gen_linear;
        protected override int? ResourceIdRecyclerView => Resource.Id.list_frag_gen_recyclerView;

		protected override async void LoadInitialDataSet ()
		{
			// eto ung pagkuha ng clients' list
			await presenter.GetAllClientsAsync();
        }

		protected override void OnItemClick (object sender, int position)
		{
			base.OnItemClick (sender, position);

			// kunin sa presenter kung sinong client
			Client client;

			try
			{
				client = presenter.Clients[position];
			}
			catch (ArgumentOutOfRangeException e)
			{
				Logger.Log(e.Message);
				client = null;
			}

			// pag walang nakuha, return na
			if (client == null)
				return;

			// Add natin sa session ung client profile na cinlick
			viewCliSession.AddClientSession(client);

			// dapat ung SupportFragmentManager gagamitin
			FragmentTransaction ft = Activity.SupportFragmentManager.BeginTransaction();

			// Profile Fragment View, ung may head at body
			Fragment fragment = new ProfileView(AccountType.Client);

			ft.Replace(Resource.Id.fragment_container, fragment);
			//ft.AddToBackStack(null);
			ft.Commit();
        }

		#region IAdminClientsView

        public void DisplayClientsList (List <AccountAdapterModel> accountDataSet)
		{
			UpdateAdapterDataSet (accountDataSet);
			HideProgressBar ();
		}

		#endregion
    }
}
