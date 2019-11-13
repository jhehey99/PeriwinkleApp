using System.Threading;
using System.Threading.Tasks;
using Android.Util;
using PeriwinkleApp.Android.Source.Cache;
using PeriwinkleApp.Android.Source.Factories;
using PeriwinkleApp.Android.Source.Session;
using PeriwinkleApp.Android.Source.Views.Activities;
using PeriwinkleApp.Core.Sources.Models.Domain;
using PeriwinkleApp.Core.Sources.Services;
using PeriwinkleApp.Core.Sources.Services.Interfaces;

namespace PeriwinkleApp.Android.Source.Presenters.AdminPresenters
{
	public interface IAdminMainPresenter
	{
		void LoadLoggedAdmin();
		void LoadNavHeaderDetails();
	}
	public class AdminMainPresenter : IAdminMainPresenter
	{
		private readonly IAdminMainActivity activity;
		private readonly IAccountService accService;

		private Account admin;

		public AdminMainPresenter(IAdminMainActivity activity)
		{
			this.activity = activity;
			accService = accService ?? new AccountService();
		}

		public void LoadLoggedAdmin()
		{
			//start progress loading
			AccountSession session = SessionFactory.ReadSession<AccountSession>(SessionKeys.LoginKey);

			if (session.AccountType != AccountType.Admin)
				return;

			admin = new Account()
			{
				AccountId = session?.AccountId,
				AccTypeId = session?.AccountType,
				Username = session?.Username,
			};
		}
		
		public void LoadNavHeaderDetails()
		{
			string username = admin?.Username;
			if (username == null)
				username = "Admin";
			activity.SetNavHeaderDetails(username, "Admin");
		}
	}

}