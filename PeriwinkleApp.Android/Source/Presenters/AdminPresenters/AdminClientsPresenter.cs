using PeriwinkleApp.Android.Source.Extensions;
using PeriwinkleApp.Core.Sources.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PeriwinkleApp.Android.Source.Views.Fragments.AdminFragments;
using PeriwinkleApp.Core.Sources.Services.Interfaces;
using PeriwinkleApp.Core.Sources.Models.Domain;

namespace PeriwinkleApp.Android.Source.Presenters.AdminPresenters
{
    public interface IAdminClientsPresenter
    {
        List <Client> Clients { get; }
        Task GetAllClientsAsync();
    }

    public class AdminClientsPresenter : IAdminClientsPresenter
    {
        private readonly IAdminClientsView view;
        private readonly IClientService clientService;

        private List<Client> clients;

        public AdminClientsPresenter(IAdminClientsView view)
        {
            this.view = view;
            clientService = clientService ?? new ClientService();
        }

        public List<Client> Clients
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

        public async Task GetAllClientsAsync()
        {
            // kunin natin ung list ng clients
            Clients = await clientService.GetAllClients();
        }
    }
}