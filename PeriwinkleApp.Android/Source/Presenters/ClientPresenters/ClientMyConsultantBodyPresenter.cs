using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PeriwinkleApp.Android.Source.AdapterModels;
using PeriwinkleApp.Android.Source.Factories;
using PeriwinkleApp.Android.Source.Session;
using PeriwinkleApp.Android.Source.Views.Fragments.ClientFragments;
using PeriwinkleApp.Core.Sources.Models.Domain;
using PeriwinkleApp.Core.Sources.Services;
using PeriwinkleApp.Core.Sources.Services.Interfaces;

namespace PeriwinkleApp.Android.Source.Presenters.ClientPresenters
{
	public interface IClientMyConsultantBodyPresenter
	{
//		Task<bool> LoadMyConsultant ();
		void LoadMyConsultantContactMethods ();

	}

    public class ClientMyConsultantBodyPresenter : IClientMyConsultantBodyPresenter
	{
		private readonly IClientMyConsultantBodyView view;
		private readonly IConsultantService conService;

        private Consultant MyConsultant;

        public ClientMyConsultantBodyPresenter (IClientMyConsultantBodyView view)
		{
			this.view = view;
			conService = conService ?? new ConsultantService();
        }

        private async Task<bool> LoadMyConsultant ()
		{
			// get client session need ung username
			ClientSession cliSession = SessionFactory.ReadSession<ClientSession>(SessionKeys.LoggedClient);

			//TODO CUSTOM SESSION EXCEPTION
			// Dapat kasi point na to, naload na ung sesion. pag di pa, baka may connection issue na, so relogin
			if (cliSession == null || !cliSession.IsSet)
				throw new Exception("Session has been lost. Please sign in again to continue...");

			MyConsultant = await conService.GetConsultantByClientId(cliSession.ClientId);

			// if there is a consultant, true
			return MyConsultant != null;
		}

		public async void LoadMyConsultantContactMethods()
		{
			if (!await LoadMyConsultant ())
				return;

			string myConFullName = $"{MyConsultant?.FirstName} {MyConsultant?.LastName}";

			string textMessage = $"Text your consultant, {myConFullName} at {MyConsultant?.Contact}";
			ReactiveAdapterModel textModel = new ReactiveAdapterModel ()
											 {
												 Title = "Send a Text Message",
												 Message = textMessage
											 };
			textModel.ActionClicked += OnTextActionClicked;


            string callMessage = $"Call your consultant, {myConFullName} at {MyConsultant?.Contact}";
			ReactiveAdapterModel callModel = new ReactiveAdapterModel ()
											 {
												 Title = "Make a Phone Call",
												 Message = callMessage
											 };
			callModel.ActionClicked += OnCallActionClicked;


            string emailMessage = $"Email your consultant, {myConFullName} at {MyConsultant?.Email}";
			ReactiveAdapterModel emailModel = new ReactiveAdapterModel ()
											  {
												  Title = "Send an Email",
												  Message = emailMessage
											  };
			emailModel.ActionClicked += OnEmailActionClicked;

            List<ReactiveAdapterModel> dataSet = new List <ReactiveAdapterModel> ()
												  {
													  textModel, callModel, emailModel
												  };
			
			// TODO ACTION CLICKED

			view.DisplayContactMethods(dataSet);
		}

		private void OnTextActionClicked (object sender, EventArgs e)
		{
			view.LaunchTextMessageIntent (MyConsultant?.Contact);
		}

		private void OnCallActionClicked (object sender, EventArgs e)
		{
			view.LaunchPhoneCallIntent (MyConsultant?.Contact);
		}

		private void OnEmailActionClicked (object sender, EventArgs e)
		{
			view.LaunchEmailIntent (MyConsultant?.Email);
		}

    }
}
