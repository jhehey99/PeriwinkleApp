using System.Threading.Tasks;
using PeriwinkleApp.Android.Source.Views.Fragments.AdminFragments;
using PeriwinkleApp.Core.Sources.Models.Domain;
using PeriwinkleApp.Core.Sources.Services;
using PeriwinkleApp.Core.Sources.Services.Interfaces;

namespace PeriwinkleApp.Android.Source.Presenters.AdminPresenters
{
    public interface IAdminConsultantProfileAboutPresenter
    {
        Consultant ConsultantProfile { get; }
        Task GetConsultantProfileAsync (string username);
    }

    public class AdminConsultantProfileAboutPresenter : IAdminConsultantProfileAboutPresenter
    {
        private readonly IAdminConsultantProfileAboutView view;
        private readonly IConsultantService consultantService;
        private Consultant consultant;

        public AdminConsultantProfileAboutPresenter (IAdminConsultantProfileAboutView view)
        {
            this.view = view;
            consultantService = consultantService ?? new ConsultantService ();
        }

        public Consultant ConsultantProfile
        {
            get => consultant;
            protected set
            {
                // tuwing mas-et, update display
                consultant = value;
                view.DisplayConsultantAboutProfile (consultant);
            }
        }
        
        public async Task GetConsultantProfileAsync (string username)
        {
            // walang kukunin
            if (username == null)
                return;

            // pag may nakuha na kanina, parang naka-cache
            if (consultant != null && consultant.Username == username)
                return;

            // sa setter natin i-cacall ung pang display sa view
            ConsultantProfile = await consultantService.GetConsultantByUsername(username);
        }
    }
}
