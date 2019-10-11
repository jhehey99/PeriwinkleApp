using System;
using System.Collections.Generic;
using System.Linq;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using PeriwinkleApp.Android.Source.AdapterModels;
using PeriwinkleApp.Android.Source.Adapters;
using PeriwinkleApp.Android.Source.Presenters.ClientPresenters;
using PeriwinkleApp.Android.Source.Views.Fragments.Common;
using PeriwinkleApp.Core.Sources.Utils;

namespace PeriwinkleApp.Android.Source.Views.Fragments.ClientFragments
{
    public interface IClientCheckListQuestionView
    {
        void DisplayQuestions (List <QuestionAdapterModel> questionDataSet);
    }

    public class ClientCheckListQuestionView : RecyclerFragment<QuestionRecyclerAdapter, QuestionAdapterModel>, 
											   IClientCheckListQuestionView
    {
        private Button btnSubmit;

        private IClientCheckListQuestionPresenter presenter;

		bool submitted = false;
		private AlertDialog loading_alert;

		protected override void OnCreateInitialize()
        {
            presenter = new ClientCheckListQuestionPresenter(this, Context);
        }

        protected override int ResourceLayout => Resource.Layout.client_checklist_question;
        protected override int? ResourceIdLinearLayout => Resource.Id.cli_chk_qtn_linear;
        protected override int? ResourceIdRecyclerView => Resource.Id.cli_chk_qtn_recyclerView;

        protected override void SetViewReferences (View view)
        {
            btnSubmit = view.FindViewById <Button> (Resource.Id.btn_cli_qtn_submit);
            btnSubmit.Click += OnSubmitClicked;

			// Loading
			AlertDialog.Builder builder = new AlertDialog.Builder(this.Context);
			builder.SetView(Resource.Layout.alert_loading);
			builder.SetTitle("Submitting");
			builder.SetMessage("Please wait, submitting response...");
			loading_alert = builder.Create();
		}
        protected override void LoadInitialDataSet()
        {
            presenter.LoadQuestions();
        }

        private async void OnSubmitClicked (object sender, EventArgs e)
        {
			// para di maulit
			if (submitted)
				return;

			submitted = true;
			loading_alert.Show();

			List <int> scaleValueList = RfRecyclerAdapter.GetScaleValueList ();
            Logger.LogList (scaleValueList.Select (v => v.ToString()).ToList ());

			await presenter.AddClientMbesResponse (scaleValueList);

			loading_alert.Dismiss();
			submitted = false;

			// nasubmit na so close na tong activity na to
			Activity.Finish ();
		}

        #region IClientCheckListQuestionView

        public void DisplayQuestions(List<QuestionAdapterModel> questionDataSet)
        {
            UpdateAdapterDataSet (questionDataSet);
            HideProgressBar ();
        }

        #endregion
    }
}
