using System.Threading.Tasks;
using PeriwinkleApp.Android.Source.Factories;
using PeriwinkleApp.Android.Source.Session;
using PeriwinkleApp.Android.Source.Views.Activities;
using PeriwinkleApp.Core.Sources.Models.Domain;
using PeriwinkleApp.Core.Sources.Services;
using PeriwinkleApp.Core.Sources.Services.Interfaces;

namespace PeriwinkleApp.Android.Source.Presenters.ConsultantPresenters
{
    public interface IConsultantMainPresenter
	{
		Task LoadSession ();
		void DetermineIsPendingConsultant ();
		void LoadNavHeaderDetails ();
    }

    public class ConsultantMainPresenter : IConsultantMainPresenter
    {
        private readonly IConsultantMainActivity activity;
        private readonly IConsultantService conService;

        private Consultant consultant;

        public ConsultantMainPresenter (IConsultantMainActivity activity)
        {
            this.activity = activity;
            conService = conService ?? new ConsultantService ();
		}

		public async Task LoadSession ()
		{
			ConsultantSession conSession =
				SessionFactory.ReadSession<ConsultantSession>(SessionKeys.LoggedConsultant);

			// may consultant session na, so kunin nalang natin ung info nya.
			if (conSession != null && conSession.IsSet)
				consultant = await conService.GetConsultantByUsername(conSession.Username);

			// i-load muna ung consultant session from account session
            ConsultantSessionLoader conLoader = new ConsultantSessionLoader(conService);
			bool isLoaded = await conLoader.LoadConsultantSession();

			if (isLoaded)
				consultant = conLoader.LoadedConsultant;

            // pag may laman, may na-load kaya !=
//            return consultant != null;
		}

		public void DetermineIsPendingConsultant ()
		{
			if(consultant.IsPending)
				activity.LaunchPendingConsultantActivity ();
		}

		public void LoadNavHeaderDetails ()
        {
//			ConsultantSession conSession =
//				SessionFactory.ReadSession <ConsultantSession> (SessionKeys.LoggedConsultant);
//
//			if (conSession == null || !conSession.IsSet)
//			{
//				ConsultantSessionLoader conLoader= new ConsultantSessionLoader(conService);
//				consultant = await conLoader.LoadConsultantSession();
//            }

            string name = $"{consultant?.FirstName} {consultant?.LastName}";
            
            activity.SetNavHeaderDetails (name, consultant?.Email);
        }

        
    }
}
