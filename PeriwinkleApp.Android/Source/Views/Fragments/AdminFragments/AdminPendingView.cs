using System.Collections.Generic;
using Android.Content;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using PeriwinkleApp.Android.Source.AdapterModels;
using PeriwinkleApp.Android.Source.Adapters;
using PeriwinkleApp.Android.Source.Presenters.AdminPresenters;
using PeriwinkleApp.Android.Source.Views.Fragments.Common;

namespace PeriwinkleApp.Android.Source.Views.Fragments.AdminFragments
{
	public interface IAdminPendingView
	{
		void ShowToastMessage(string message);
		void DisplayPendingConsultantsList (List <AccountAdapterModel> accountDataSet);

	}

    public class AdminPendingView : RecyclerFragment<AccountRecyclerAdapter, AccountAdapterModel>,
										IAdminPendingView
    {
		private IAdminPendingPresenter presenter;

		private TextView dialogTitleTextView;

		private AlertDialog alertDialog;

		private int pendingPosition;

        protected override void OnCreateInitialize ()
		{
			IsAnimated = true;
			presenter = new AdminPendingPresenter(this);
        }

		protected override int ResourceLayout => Resource.Layout.list_frag_generic;
		protected override int? ResourceIdProgressBar => Resource.Id.list_frag_gen_progress;
		protected override int? ResourceIdLinearLayout => Resource.Id.list_frag_gen_linear;
		protected override int? ResourceIdRecyclerView => Resource.Id.list_frag_gen_recyclerView;

        protected override async void LoadInitialDataSet ()
		{
			//TODO ILOAD ULET AFTER NG REJECT OR ACCEPT
			await presenter.GetAllPendingConsultants();
        }

		protected override void OnItemClick (object sender, int position)
		{
			base.OnItemClick (sender, position);

			pendingPosition = position;

            // Dialog title, contain name of clicked item
            View dialogTitleView = LayoutInflater.Inflate(Resource.Layout.textview_dialog_title, null);
			dialogTitleTextView = dialogTitleView.FindViewById<TextView>(Resource.Id.dialog_title_textview);
			dialogTitleTextView.Text = presenter.GetNameAt (position);
			
			// Confirmation dialog
			AlertDialog.Builder builder = new AlertDialog.Builder(this.Activity);
			builder.SetCustomTitle(dialogTitleTextView);
			builder.SetMessage(Resource.String.pending_confirm_msg);

			// Accept Button
			string positiveText = Resources.GetString(Resource.String.pending_accept);
			builder.SetPositiveButton(positiveText, AlertDialogAcceptClicked);

			// Reject Button
			string negativeText = Resources.GetString(Resource.String.pending_reject);
			builder.SetNegativeButton(negativeText, AlertDialogRejectClicked);

			// Cancel Button
			string neutralText = Resources.GetString(Resource.String.pending_cancel);
			builder.SetNeutralButton(neutralText, AlertDialogCancelClicked);

			// Create and show the dialog
			alertDialog = builder.Create();
			alertDialog.Show();
        }

		private async void AlertDialogAcceptClicked(object sender, DialogClickEventArgs e)
		{
			// accept natin ung consultant registration
			presenter.UpdateRegistration(pendingPosition, true);

			// refresh natin ung dataSet by getting ulit ung pending consultants
			await presenter.GetAllPendingConsultants();
		}

		private async void AlertDialogRejectClicked(object sender, DialogClickEventArgs e)
		{
			// reject natin ung consultant registration
			presenter.UpdateRegistration(pendingPosition, false);

			// refresh natin ung dataSet by getting ulit ung pending consultants
			await presenter.GetAllPendingConsultants();
		}
		
		private void AlertDialogCancelClicked(object sender, DialogClickEventArgs e)
		{
			alertDialog.Cancel();
		}

		public void ShowToastMessage(string message)
		{
			Toast.MakeText(this.Context, message, ToastLength.Short).Show();
		}

		public void DisplayPendingConsultantsList (List <AccountAdapterModel> accountDataSet)
		{
			UpdateAdapterDataSet(accountDataSet);
			HideProgressBar();
        }

    }
}
