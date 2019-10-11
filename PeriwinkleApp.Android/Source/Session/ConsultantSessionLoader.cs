using System.Threading.Tasks;
using PeriwinkleApp.Android.Source.Factories;
using PeriwinkleApp.Core.Sources.Models.Domain;
using PeriwinkleApp.Core.Sources.Services;
using PeriwinkleApp.Core.Sources.Services.Interfaces;

namespace PeriwinkleApp.Android.Source.Session
{
	public class ConsultantSessionLoader
	{
		private readonly IConsultantService conService;
		public Consultant LoadedConsultant { get; protected set; }

		public ConsultantSessionLoader (IConsultantService conService = null)
		{
			this.conService = conService ?? new ConsultantService ();
		}

		public async Task<bool> LoadConsultantSession ()
		{
			AccountSession session = SessionFactory.ReadSession<AccountSession>(SessionKeys.LoginKey);

			if (session.AccountType != AccountType.Consultant)
				return false;

			// account is consultant, so get its info
			LoadedConsultant = await conService.GetConsultantByUsername (session.Username);

			// add it to consultant session
			ConsultantSession conSession =
				SessionFactory.CreateSession <ConsultantSession> (SessionKeys.LoggedConsultant);

			conSession.AddConsultantSession (LoadedConsultant);

			return LoadedConsultant != null;
		}
    }
}
