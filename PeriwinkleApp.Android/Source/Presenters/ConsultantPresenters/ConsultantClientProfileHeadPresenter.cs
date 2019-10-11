using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PeriwinkleApp.Android.Source.Factories;
using PeriwinkleApp.Android.Source.Session;
using PeriwinkleApp.Android.Source.Views.Fragments.ConsultantFragments;
using PeriwinkleApp.Core.Sources.Models.Domain;
using PeriwinkleApp.Core.Sources.Models.Response;
using PeriwinkleApp.Core.Sources.Services;
using PeriwinkleApp.Core.Sources.Services.Interfaces;
using PeriwinkleApp.Core.Sources.Utils;

namespace PeriwinkleApp.Android.Source.Presenters.ConsultantPresenters
{
	public interface IConsultantClientProfileHeadPresenter
	{
		Task LoadClientToView ();
		void LoadHeadInfo ();
		Task AllowClientTakeMbes(bool allow);
	}

    public class ConsultantClientProfileHeadPresenter : IConsultantClientProfileHeadPresenter
	{
		private readonly IConsultantClientProfileHeadView view;

		private readonly IClientService cliService;
		private Client clientToView;

		public ConsultantClientProfileHeadPresenter (IConsultantClientProfileHeadView view)
		{
			this.view = view;
			cliService = cliService ?? new ClientService ();
		}

        public async Task LoadClientToView ()
		{
			ClientSession cliSession = SessionFactory.ReadSession<ClientSession>(SessionKeys.ViewClient);

			// may client session na, so kunin nalang natin ung info nya.
			if (cliSession != null && cliSession.IsSet)
				clientToView = await cliService.GetClientByUsername(cliSession.Username);
		}

		public void LoadHeadInfo ()
		{
			// build ung string
			string name = $"{clientToView?.FirstName} {clientToView?.LastName}";

			// display na ung info
			view.DisplayHeadInfo(name, clientToView?.Username);
		}

		public async Task AllowClientTakeMbes(bool allow)
		{
			clientToView.MbesAllowAttempt = allow;
			List <ApiResponse> responses = await cliService.AllowClientTakeMbes (clientToView);
			ApiResponse response = responses.FirstOrDefault ();
			if(response?.Code != ApiResponseCode.UpdateSuccess)
				Logger.Log (response?.Message);
		}
	}
}
