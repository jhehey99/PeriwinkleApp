using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PeriwinkleApp.Android.Source.Extensions;
using PeriwinkleApp.Android.Source.Factories;
using PeriwinkleApp.Android.Source.Session;
using PeriwinkleApp.Android.Source.Views.Fragments.ConsultantFragments;
using PeriwinkleApp.Core.Sources.Models.Domain;
using PeriwinkleApp.Core.Sources.Services;
using PeriwinkleApp.Core.Sources.Services.Interfaces;

namespace PeriwinkleApp.Android.Source.Presenters.ConsultantPresenters
{
    public interface IConsultantClientsPresenter
    {
        List <Client> Clients { get; }
        Task GetAllMyClients();

		void ViewClientAt (int position);

	}

    public class ConsultantClientsPresenter : IConsultantClientsPresenter
    {
        private readonly IConsultantClientsView view;
        private readonly IClientService cliService;

        // session ng nag login

        private List <Client> clients;

        public List <Client> Clients
        {
            get => clients ?? (clients = new List <Client> ());
            protected set
            {
                // pag magkaiba ng count palitan na or pag may atleast isang item na magkaiba, palitan narin.  TODO Optimize
                bool shouldDisplay = (Clients.Count != value.Count) ||
                                     value.Where((t, i) => !Clients[i].Equals(t)).Any();

                if (!shouldDisplay) return;

                // update ung list and display ulit
                clients = value;
                view.DisplayMyClientsList(clients.ToListAccountAdapterModel());
            }
        }

        public ConsultantClientsPresenter (IConsultantClientsView view)
        {
            this.view = view;
            cliService = cliService ?? new ClientService ();
        }

        public async Task GetAllMyClients ()
        {
            // kailangan malaman kung sinong consultant id

            ConsultantSession conSession =
                SessionFactory.ReadSession <ConsultantSession> (SessionKeys.LoggedConsultant);

            if (conSession == null || !conSession.IsSet)
                return;

            Clients = await cliService.GetClientsByConsultantId (conSession.ConsultantId);
        }

		public void ViewClientAt (int position)
		{
			if (position > Clients.Count)
				throw new IndexOutOfRangeException ($"index = {position}");

			// Create ng session for viewing that specific client
			ClientSession viewClientSession = SessionFactory.CreateSession <ClientSession> (SessionKeys.ViewClient);
			viewClientSession.AddClientSession (Clients[position]);
			
			// Launch ung fragment para ma view ung profile
			view.LaunchClientProfile ();
		}
	}
}
