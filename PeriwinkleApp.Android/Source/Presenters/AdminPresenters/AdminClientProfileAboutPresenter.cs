using System.Threading.Tasks;
using PeriwinkleApp.Android.Source.Views.Fragments.AdminFragments;
using PeriwinkleApp.Core.Sources.Models.Domain;
using PeriwinkleApp.Core.Sources.Services;
using PeriwinkleApp.Core.Sources.Services.Interfaces;

namespace PeriwinkleApp.Android.Source.Presenters.AdminPresenters
{
    public interface IAdminClientProfileAboutPresenter
    {
        Client ClientProfile { get; }
        Task GetClientProfileAsync (string username);
    }

    public class AdminClientProfileAboutPresenter : IAdminClientProfileAboutPresenter
    {
        private readonly IAdminClientProfileAboutView view;
        private readonly IClientService clientService;
        private Client client;
       
        public AdminClientProfileAboutPresenter(IAdminClientProfileAboutView view)
        {
            this.view = view;
            clientService = clientService ?? new ClientService();
        }
        public Client ClientProfile
        {
            get => client;
            protected set
            {
                // tuwing mase-set, ma-u-update ung display
                client = value;
                view.DisplayClientAboutProfile(client);
            }
        }

        public async Task GetClientProfileAsync (string username)
        {
            // walang kukunin
            if (username == null)
                return;

            // pag may nakuha na kanina, parang naka-cache
            if (client != null && client.Username == username)
                return;
            
            // sa setter natin i-cacall ung pang display sa view
            ClientProfile = await clientService.GetClientByUsername (username);
        }
    }
}