using System;
using System.Collections.Generic;
using PeriwinkleApp.Android.Source.AdapterModels;
using PeriwinkleApp.Android.Source.Factories;
using PeriwinkleApp.Android.Source.Session;
using PeriwinkleApp.Android.Source.Views.Fragments.ConsultantFragments;
using PeriwinkleApp.Core.Sources.Utils;

namespace PeriwinkleApp.Android.Source.Presenters.ConsultantPresenters
{
	public interface IConsultantClientProfileBodyPresenter
	{
		void LoadNotifications ();
	}
	
    public class ConsultantClientProfileBodyPresenter : IConsultantClientProfileBodyPresenter
    {
		private readonly IConsultantClientProfileBodyView view;

		public ConsultantClientProfileBodyPresenter (IConsultantClientProfileBodyView view)
		{
			this.view = view;

			ClientSession cliSessionToView = SessionFactory.ReadSession <ClientSession> (SessionKeys.ViewClient);
		}

		private void ViewClientBehaviorList(object sender, EventArgs e)
        {
			Logger.Log ("ViewClientBehaviorList");
			view.StartViewClientBehaviorList();
		}

        private void ViewClientWeeklyReport(object sender, EventArgs e)
		{
			Logger.Log ("ViewClientWeeklyReport");
			view.StartViewClientWeeklyReport ();
		}

		private void ViewClientSurveyReport (object sender, EventArgs e)
		{
			Logger.Log("ViewClientSurveyReport");
			view.StartViewClientSurveyReport ();
        }

        public void LoadNotifications ()
		{
			// Behavior
			NotificationAdapterModel notifBehaviorList =
				new NotificationAdapterModel ()
				{
					Title = "View Behavior List",
					Message = "View this client's behavior graphs as well as statistics for each graph",
					HasAction = true
				};
			notifBehaviorList.ActionClicked += ViewClientBehaviorList;

            // WeeklyReport
			NotificationAdapterModel notifWeekly =
				new NotificationAdapterModel()
				{
					Title = "View Weekly Reports",
					Message = "View this client's weekly reports and statistics",
					HasAction = true
				};
			notifWeekly.ActionClicked += ViewClientWeeklyReport;

			// SurveyReport
			NotificationAdapterModel notifSurvey =
				new NotificationAdapterModel()
				{
					Title = "View Survey Reports",
					Message = "View this client's survey scores and report",
					HasAction = true
				};
			notifSurvey.ActionClicked += ViewClientSurveyReport;
			
            // add it to the data set
            List <NotificationAdapterModel> dataSet =
				new List <NotificationAdapterModel> ()
				{
					notifBehaviorList,
					notifWeekly,
					notifSurvey
                };
			view.DisplayNotifications (dataSet);
		}

    }
}
