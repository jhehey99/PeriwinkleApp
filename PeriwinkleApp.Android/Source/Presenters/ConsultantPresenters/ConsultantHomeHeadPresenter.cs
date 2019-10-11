using System;
using System.Threading.Tasks;
using PeriwinkleApp.Android.Source.Cache;
using PeriwinkleApp.Android.Source.Factories;
using PeriwinkleApp.Android.Source.Session;
using PeriwinkleApp.Android.Source.Views.Fragments.ConsultantFragments;
using PeriwinkleApp.Core.Sources.Models.Domain;
using PeriwinkleApp.Core.Sources.Services;
using PeriwinkleApp.Core.Sources.Services.Interfaces;

namespace PeriwinkleApp.Android.Source.Presenters.ConsultantPresenters
{
	public interface IConsultantHomeHeadPresenter
	{
		Task LoadSession ();
        void LoadHeadInfo();
    }

    public class ConsultantHomeHeadPresenter : IConsultantHomeHeadPresenter
	{
		private readonly IConsultantHomeHeadView view;
		private readonly IConsultantService conService;

		private Consultant consultant;

		public ConsultantHomeHeadPresenter(IConsultantHomeHeadView view)
		{
			this.view = view;
			conService = conService ?? new ConsultantService ();
		}

		public async Task LoadSession ()
		{
			if (CacheProvider.IsSet(CacheKey.LoggedConsultant))
			{
				consultant = CacheProvider.Get <Consultant> (CacheKey.LoggedConsultant);
				return;
			}

            ConsultantSession conSession =
				SessionFactory.ReadSession<ConsultantSession>(SessionKeys.LoggedConsultant);

			// may consultant session na, so kunin nalang natin ung info nya.
			if (conSession != null && conSession.IsSet)
				consultant = await conService.GetConsultantByUsername(conSession.Username);

			// i-load muna ung consultant session from account session
			ConsultantSessionLoader conLoader = new ConsultantSessionLoader(conService);
			bool isLoaded = await conLoader.LoadConsultantSession();

			if (isLoaded)
			{
				consultant = conLoader.LoadedConsultant;
				CacheProvider.Set (CacheKey.LoggedConsultant, consultant);
            }
        }

		public void LoadHeadInfo ()
		{
//			ClientSession cliSession = SessionFactory.ReadSession<ClientSession>(SessionKeys.LoggedClient);
//
//			//TODO CUSTOM SESSION EXCEPTION
//			// Dapat kasi point na to, naload na ung sesion. pag di pa, baka may connection issue na, so relogin
//			if (cliSession == null || !cliSession.IsSet)
//				throw new Exception("Session has been lost. Please sign in again to continue...");
//
//			Consultant consultant = await conService.GetConsultantByClientId (cliSession.ClientId);
//
//			string name = $"{consultant?.FirstName} {consultant?.LastName}";
//
//			view.DisplayHeadInfo(name, consultant?.Username);



			// get consultant session need ung username
//			ConsultantSession conSession =
//				SessionFactory.ReadSession <ConsultantSession> (SessionKeys.LoggedConsultant);
//
//
//			// pag walang naread 
//			if (conSession == null || !conSession.IsSet)
//			{
//				ConsultantSessionLoader conLoader = new ConsultantSessionLoader(conService);
//				consultant = await conLoader.LoadConsultantSession();
//			}
//			else
//			{
//                // pag may naread na consultant session, get na info ni consultant
//                consultant = await conService.GetConsultantByUsername(conSession.Username);
//			}

			string name = $"{consultant?.FirstName} {consultant?.LastName}";

			view.DisplayHeadInfo(name, consultant?.Username);
        }
    }
}
