using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PeriwinkleApp.Android.Source.AdapterModels;
using PeriwinkleApp.Android.Source.Factories;
using PeriwinkleApp.Android.Source.Session;
using PeriwinkleApp.Android.Source.Views.Fragments.ClientFragments;
using PeriwinkleApp.Core.Sources.Models.Domain;
using PeriwinkleApp.Core.Sources.Services;
using PeriwinkleApp.Core.Sources.Services.Interfaces;
using PeriwinkleApp.Core.Sources.Utils;

namespace PeriwinkleApp.Android.Source.Presenters.ClientPresenters
{
	public interface IClientBehaviorListPresenter
	{
		Task GetAllBehaviorData ();
		void ViewReportClicked (object sender, int position);
	}

	public class ClientBehaviorListPresenter : IClientBehaviorListPresenter
	{
		private readonly IClientBehaviorView view;
		private readonly IClientService cliService;
		private ClientSession cliSession;
		private List <BehaviorGraph> graphs;
		private string sessionKeyToUse;

        public ClientBehaviorListPresenter (IClientBehaviorView view, string sessionKeyToUse = null)
		{
			this.view = view;
			this.sessionKeyToUse = sessionKeyToUse;
            cliService = cliService ?? new ClientService ();
			LoadClientSession ();
		}

		private void LoadClientSession()
		{
			if (sessionKeyToUse == null)
				sessionKeyToUse = SessionKeys.LoggedClient;

			cliSession = SessionFactory.ReadSession<ClientSession>(sessionKeyToUse);
			//			if (cliSession != null && cliSession.IsSet)
			//				client = await cliService.GetClientByUsername(cliSession.Username);
		}

        public async Task GetAllBehaviorData ()
		{
			//TODO kasama statistics sa behavior graph na makukuha
			graphs = await cliService.GetBehaviorGraphByClientId (cliSession.ClientId);

			// pag walang laman, avoid na magdisplay, TODO OR lagay na no items yet bla bla chu chu
			if (graphs == null)
				return;

			List <BehaviorAdapterModel> dataSet =
				graphs.Select ((t, i) => new BehaviorAdapterModel ()
										 {
											 Filename = t.Filename,
											 StartTime = t.StartTime,
											 StopTime = t.StopTime,
											 Position = i
										 }).ToList ();

			foreach (BehaviorAdapterModel model in dataSet)
				model.ViewReportClicked += ViewReportClicked;
			
			//pwedeng foreach nlng dito para sa click event
			view.DisplayBehaviorFiles (dataSet);
		}

		public void ViewReportClicked (object sender, int position)
		{
			Logger.Log($"ViewReportClicked - {position}");

            BehaviorGraph bg = graphs[position];
			view.LaunchViewBehavior (bg);
		}
	}
}
