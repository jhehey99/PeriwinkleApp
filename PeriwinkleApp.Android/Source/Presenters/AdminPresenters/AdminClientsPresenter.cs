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
                clients = value;
                view.DisplayClientsList(clients.ToListAccountAdapterModel());
            }
        }

        public async Task GetAllClientsAsync()
        {
            Clients = await clientService.GetAllClients();
        }
    }
}