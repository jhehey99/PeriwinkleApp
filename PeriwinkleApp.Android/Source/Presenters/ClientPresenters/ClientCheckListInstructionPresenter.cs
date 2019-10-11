using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Android.Content;
using PeriwinkleApp.Android.Source.Cache;
using PeriwinkleApp.Android.Source.Factories;
using PeriwinkleApp.Android.Source.Services;
using PeriwinkleApp.Android.Source.Session;
using PeriwinkleApp.Android.Source.Views.Fragments.ClientFragments;
using PeriwinkleApp.Core.Sources.Models.Domain;
using PeriwinkleApp.Core.Sources.Services;
using PeriwinkleApp.Core.Sources.Services.Interfaces;
using PeriwinkleApp.Core.Sources.Utils;

namespace PeriwinkleApp.Android.Source.Presenters.ClientPresenters
{
    public interface IClientCheckListInstructionPresenter
    {
        void LoadInstructions ();
        Task UpdateClientHeightWeight (string heightStr, string weightStr);
    }

    public class ClientCheckListInstructionPresenter : IClientCheckListInstructionPresenter
    {
        private readonly IClientCheckListInstructionView view;
        private readonly IMbesAssetService mbesService;
        private readonly IClientService cliService;
		private Client client;

		public ClientCheckListInstructionPresenter (IClientCheckListInstructionView view, Context context)
        {
            this.view = view;
            mbesService = mbesService ?? new MbesAssetService(context);
            cliService = cliService ?? new ClientService ();
        }

        public void LoadInstructions ()
        {
            IList <string> instructions = mbesService.GetInstructions ();
            view.DisplayInstructions (instructions);
        }

        public async Task UpdateClientHeightWeight (string heightStr, string weightStr)
        {
            // di pwede null
            bool isNull = heightStr == null || weightStr == null;

            // check natin kung valid float
            bool validHeight = float.TryParse (heightStr, out float height);
            bool validWeight = float.TryParse (weightStr, out float weight);

            // display invalid or error message
            if (isNull || !validHeight || !validWeight)
                return; 
            
            /* Valid ung input so continue na. */

            // kunin ung logged client, para sa kanya i-update ung height and weight
            ClientSession cliSession = SessionFactory.ReadSession <ClientSession> (SessionKeys.LoggedClient);

            if (cliSession == null || !cliSession.IsSet)
                return;

            // gawa tayo client model, client id lang kailangan i-send sa http
            Client client = new Client()
                            {
                                ClientId = cliSession.ClientId,
                                Height = height,
                                Weight = weight
                            };

            await cliService.UpdateHeightWeight (client);

			if (!CacheProvider.IsSet(CacheKey.LoggedClient))
			{
				// get client info given session username
				cliSession = SessionFactory.ReadSession<ClientSession>(SessionKeys.LoggedClient);

				//TOdo show error, session has been lost login again
				if (cliSession == null || !cliSession.IsSet)
					return;

				client = await cliService.GetClientByUsername(cliSession.Username);
			}
			else
			{
				client = CacheProvider.Get<Client>(CacheKey.LoggedClient);
			}

			client.Height = height;
			client.Weight = weight;
			CacheProvider.Set(CacheKey.LoggedClient, client);
		}
	}
}
