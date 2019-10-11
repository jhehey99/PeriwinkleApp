using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Android.App;
using PeriwinkleApp.Android.Source.Extensions;
using PeriwinkleApp.Android.Source.Views.Fragments.AdminFragments;
using PeriwinkleApp.Core.Sources.Models.Domain;
using PeriwinkleApp.Core.Sources.Services;
using PeriwinkleApp.Core.Sources.Services.Interfaces;

namespace PeriwinkleApp.Android.Source.Presenters.AdminPresenters
{
    public interface IAdminConsultantProfileClientsPresenter
    {
        List <Client> Clients { get; }
        Task <Consultant> GetConsultantViewed (string username);
        Task GetClientsOfConsultantAsync (Consultant consultant);
    }

    public class AdminConsultantProfileClientsPresenter : IAdminConsultantProfileClientsPresenter
    {
        private readonly IAdminConsultantProfileClientsView view;
        private readonly IClientService clientService;
        private readonly IConsultantService consultantService;

        private List <Client> clients;
        public AdminConsultantProfileClientsPresenter (IAdminConsultantProfileClientsView view)
        {
            this.view = view;
            clientService = clientService ?? new ClientService();
            consultantService = consultantService ?? new ConsultantService();
        }

        public List <Client> Clients
        {
            get => clients ?? (clients = new List<Client>());
            protected set
            {
                // pag magkaiba ng count palitan na or pag may atleast isang item na magkaiba, palitan narin.  TODO Optimize
                bool shouldDisplay = (Clients.Count != value.Count) ||
                                     value.Where((t, i) => !Clients[i].Equals(t)).Any();

                if (!shouldDisplay) return;

                // update ung value and display ulit 
                clients = value;

                view.DisplayClientsList(clients.ToListAccountAdapterModel());
            }
        }

        public async Task <Consultant> GetConsultantViewed (string username)
        {
            return await consultantService.GetConsultantByUsername (username);
        }

        public async Task GetClientsOfConsultantAsync (Consultant consultant)
        {
            List <Client> conClients = await clientService.GetClientsByConsultantId (consultant.ConsultantId);
            if(conClients == null)
                return;
            
            Clients = conClients;
        }
    }
}
