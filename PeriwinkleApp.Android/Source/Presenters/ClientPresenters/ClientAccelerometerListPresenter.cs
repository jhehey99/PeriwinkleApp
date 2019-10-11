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
	public interface IClientAccelerometerListPresenter
	{
		Task GetAllAccelerometerRecords();
		void ViewRecordClicked(object sender, int position);
	}

	public class ClientAccelerometerListPresenter : IClientAccelerometerListPresenter
	{
		private readonly IClientAccelerometerView view;
		private readonly IClientService cliService;
		private ClientSession cliSession;
		private List<AccelerometerRecord> records;
		private string sessionKeyToUse;

		public ClientAccelerometerListPresenter(IClientAccelerometerView view, string sessionKeyToUse = null)
		{
			this.view = view;
			this.sessionKeyToUse = sessionKeyToUse;
			cliService = cliService ?? new ClientService();
			LoadClientSession();
		}

		private void LoadClientSession()
		{
			if (sessionKeyToUse == null)
				sessionKeyToUse = SessionKeys.LoggedClient;

			cliSession = SessionFactory.ReadSession<ClientSession>(sessionKeyToUse);
		}

		public async Task GetAllAccelerometerRecords()
		{
			records = await cliService.GetAccelerometerRecordByClientId(cliSession.ClientId);

			if (records == null)
				return;

			List<BehaviorAdapterModel> dataSet =
				records.Select((t, i) => new BehaviorAdapterModel()
				{
					Filename = t.Filename,
					StartTime = t.StartTime,
					StopTime = t.StopTime,
					Position = i
				}).ToList();

			foreach (BehaviorAdapterModel model in dataSet)
				model.ViewReportClicked += ViewRecordClicked;
			
			//pwedeng foreach nlng dito para sa click event
			view.DisplayRecordFiles(dataSet);
		}

		public void ViewRecordClicked(object sender, int position)
		{
			Logger.Log($"ViewReportClicked - {position}");

			AccelerometerRecord r = records[position];
			view.LaunchViewRecord(r);
		}
	}
}