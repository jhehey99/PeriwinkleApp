using System;
using System.Threading.Tasks;
using PeriwinkleApp.Android.Source.Factories;
using PeriwinkleApp.Android.Source.Session;
using PeriwinkleApp.Android.Source.Views.Fragments.ClientFragments;
using PeriwinkleApp.Core.Sources.Models.Domain;
using PeriwinkleApp.Core.Sources.Services;
using PeriwinkleApp.Core.Sources.Services.Interfaces;

namespace PeriwinkleApp.Android.Source.Presenters.ClientPresenters
{
	public interface IClientMyConsultantPresenter
    {
		Task LoadHeadInfo();
	}

	//TODO GAWIN MO KONG HEAD
    public class ClientMyConsultantPresenter : IClientMyConsultantPresenter
    {
		private readonly IClientMyConsultantHeadView view;
		private readonly IConsultantService conService;

        public ClientMyConsultantPresenter(IClientMyConsultantHeadView view)
		{
			this.view = view;
			conService = conService ?? new ConsultantService();
		}

        public async Task LoadHeadInfo ()
		{
			// get client session need ung username
			ClientSession cliSession = SessionFactory.ReadSession<ClientSession>(SessionKeys.LoggedClient);

			//TODO CUSTOM SESSION EXCEPTION
			// Dapat kasi point na to, naload na ung sesion. pag di pa, baka may connection issue na, so relogin
			if (cliSession == null || !cliSession.IsSet)
				throw new Exception("Session has been lost. Please sign in again to continue...");

			Consultant consultant = await conService.GetConsultantByClientId(cliSession.ClientId);

			string name = $"{consultant?.FirstName} {consultant?.LastName}";

			view.DisplayHeadInfo(name, consultant?.Username);
		}
	}
}
