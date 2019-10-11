using PeriwinkleApp.Android.Source.AdapterModels;
using PeriwinkleApp.Android.Source.Extensions;
using PeriwinkleApp.Core.Sources.Exceptions;
using PeriwinkleApp.Core.Sources.Models.Domain;
using PeriwinkleApp.Core.Sources.Models.Response;
using PeriwinkleApp.Core.Sources.Services;
using PeriwinkleApp.Core.Sources.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PeriwinkleApp.Android.Source.Views.Fragments.AdminFragments;

namespace PeriwinkleApp.Android.Source.Presenters.AdminPresenters
{
    public interface IAdminPendingPresenter
    {
        Task GetAllPendingConsultants();
        void UpdateRegistration(int position, bool accept);
		string GetNameAt (int position);
	}

    public class AdminPendingPresenter : IAdminPendingPresenter
    {
        private readonly IAdminPendingView view;
        private readonly IConsultantService consultantService;
        private List<AccountAdapterModel> accountDataSet;
        private List<Consultant> consultants;

        public AdminPendingPresenter(IAdminPendingView view)
        {
            this.view = view;
            accountDataSet = null;
            consultantService = consultantService ?? new ConsultantService();
        }

        public async Task GetAllPendingConsultants()
        {
			if (accountDataSet != null)
			{
				view.DisplayPendingConsultantsList(accountDataSet);
				return;
            }

            consultants = await consultantService.GetAllPendingConsultants();
            accountDataSet = consultants.ToListAccountAdapterModel();
            view.DisplayPendingConsultantsList (accountDataSet);
        }

        public async void UpdateRegistration(int position, bool accept)
        {
            Consultant consultant = consultants[position];
            try
            {
                List <ApiResponse> response = await consultantService.ValidatePendingConsultant(consultant.Username, accept);

                if (response.First().Code == ApiResponseCode.UpdateSuccess)
                    view.ShowToastMessage("Operation Completed Successfully!");
            }
            catch (RequestFailedException e)
            {
                view.ShowToastMessage("Problem: An Error Occured! " + e.Message);
            }
        }

		public string GetNameAt (int position)
		{
			return position > accountDataSet.Count ? "" : accountDataSet[position].Name;
		}
	}
}