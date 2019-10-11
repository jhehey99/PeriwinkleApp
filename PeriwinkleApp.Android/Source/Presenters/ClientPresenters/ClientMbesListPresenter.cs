using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using PeriwinkleApp.Android.Source.AdapterModels;
using PeriwinkleApp.Android.Source.Cache;
using PeriwinkleApp.Android.Source.Factories;
using PeriwinkleApp.Android.Source.Session;
using PeriwinkleApp.Android.Source.Views.Fragments.ClientFragments;
using PeriwinkleApp.Core.Sources.Models.Domain;
using PeriwinkleApp.Core.Sources.Services;
using PeriwinkleApp.Core.Sources.Services.Interfaces;

namespace PeriwinkleApp.Android.Source.Presenters.ClientPresenters
{
	public interface IClientMbesListPresenter
	{
		Task GetAllMbes();
		void ViewMbesClicked(object sender, int position);
	}

	public class ClientMbesListPresenter : IClientMbesListPresenter
	{
		private readonly IClientMbesListView view;
		private readonly IClientService cliService;
		private readonly IMbesService mbesService;
		private ClientSession cliSession;
		private List <Mbes> mbes;
		private Client loggedClient;

        public ClientMbesListPresenter(IClientMbesListView view)
		{
			this.view = view;
			cliService = cliService ?? new ClientService();
			mbesService = mbesService ?? new MbesService();
			LoadClientSession ();
		}

		private void LoadClientSession()
		{
			cliSession = SessionFactory.ReadSession<ClientSession>(SessionKeys.LoggedClient);

			loggedClient = CacheProvider.Get<Client>(CacheKey.LoggedClient);
		}

		public async Task GetAllMbes()
		{
			mbes = await mbesService.GetMbesByClientId(loggedClient.ClientId.Value);

			if (mbes == null)
				return;

			List<ResponseAdapterModel> dataSet =
				mbes.Select((t, i) => new ResponseAdapterModel()
				{
					Date = t.DateCreated.ToLongDateString(),
					BMI = t.BMI.ToString()
				}).ToList();

			foreach (ResponseAdapterModel model in dataSet)
				model.ViewMbeClicked += ViewMbesClicked;

			view.DisplayResponses(dataSet);
		}

		public void ViewMbesClicked(object sender, int position)
		{
			Mbes m = mbes[position];
			view.LaunchViewResponse(m);
		}
	}
}