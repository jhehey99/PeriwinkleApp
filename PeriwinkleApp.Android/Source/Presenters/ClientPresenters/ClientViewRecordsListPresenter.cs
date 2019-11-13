using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PeriwinkleApp.Android.Source.AdapterModels;
using PeriwinkleApp.Android.Source.Cache;
using PeriwinkleApp.Android.Source.Factories;
using PeriwinkleApp.Android.Source.Session;
using PeriwinkleApp.Android.Source.Views.Fragments.ClientFragments;
using PeriwinkleApp.Core.Sources.Models.Domain;
using PeriwinkleApp.Core.Sources.Services;
using PeriwinkleApp.Core.Sources.Services.Interfaces;
using PeriwinkleApp.Core.Sources.Utils;

namespace PeriwinkleApp.Android.Source.Presenters.ClientPresenters
{
	public interface IClientViewRecordsListPresenter
	{
		Task GetAllRecords();
		void ViewRecordClicked(object sender, int position);
	}

	public class ClientViewRecordsListPresenter : IClientViewRecordsListPresenter
	{
		private readonly IClientViewRecordsListView view;
		private readonly IClientService cliService;
		private ClientSession cliSession;
		private List<SensorRecord> records;
		private Client loggedClient;

		public ClientViewRecordsListPresenter(IClientViewRecordsListView view)
		{
			this.view = view;
			cliService = cliService ?? new ClientService();
			LoadClientSession();
		}

		private void LoadClientSession()
		{
			cliSession = SessionFactory.ReadSession<ClientSession>(SessionKeys.LoggedClient);

			loggedClient = CacheProvider.Get<Client>(CacheKey.LoggedClient);
		}

		public async Task GetAllRecords()
		{
			records = await cliService.GetSensorRecordByClientId(loggedClient.ClientId);
			
			if (records == null)
				return;

			List<SensorRecordAdapterModel> dataSet =
				records.Select((t, i) => new SensorRecordAdapterModel()
				{
					RecordType = t.RecordType.Value,
					StartTime = t.StartTime,
					StopTime = t.StopTime,
					Position = i
				}).ToList();

			foreach (SensorRecordAdapterModel model in dataSet)
				model.ViewReportClicked += ViewRecordClicked;

			view.DisplayRecords(dataSet);
		}

		public void ViewRecordClicked(object sender, int position)
		{
			Logger.Log($"ViewRecordClicked - {position}");

			SensorRecord record = records[position];
			view.LaunchViewRecord(record);
		}
	}
}