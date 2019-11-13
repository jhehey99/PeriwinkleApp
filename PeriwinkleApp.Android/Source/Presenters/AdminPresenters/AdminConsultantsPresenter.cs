using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PeriwinkleApp.Android.Source.Extensions;
using PeriwinkleApp.Android.Source.Views.Fragments.AdminFragments;
using PeriwinkleApp.Core.Sources.Models.Domain;
using PeriwinkleApp.Core.Sources.Services;
using PeriwinkleApp.Core.Sources.Services.Interfaces;

namespace PeriwinkleApp.Android.Source.Presenters.AdminPresenters
{
    public interface IAdminConsultantsPresenter
    {
        List <Consultant> Consultants { get; }
        Task GetAllConsultants();
    }

    public class AdminConsultantsPresenter : IAdminConsultantsPresenter
    {
        private readonly IAdminConsultantsView view;
        private readonly IConsultantService consultantService;

        private List <Consultant> consultants;
        public List<Consultant> Consultants
        {
            get => consultants ?? (consultants = new List <Consultant> ());

            protected set
            {
                consultants = value;
                view.DisplayConsultantsList (consultants.ToListAccountAdapterModel ());
            }
        }

        public AdminConsultantsPresenter(IAdminConsultantsView view)
        {
            this.view = view;
            consultantService = consultantService ?? new ConsultantService();
        }

        public async Task GetAllConsultants ()
        {
			Consultants = await consultantService.GetAllConsultants ();
		}
	}
}