using System.Collections.Generic;
using Android.Content;
using Android.Net;
using PeriwinkleApp.Android.Source.AdapterModels;
using PeriwinkleApp.Android.Source.Adapters;
using PeriwinkleApp.Android.Source.Presenters.ClientPresenters;
using PeriwinkleApp.Android.Source.Views.Fragments.Common;

namespace PeriwinkleApp.Android.Source.Views.Fragments.ClientFragments
{
	public interface IClientMyConsultantBodyView
	{
		void DisplayContactMethods (List<ReactiveAdapterModel> dataSet);
		void LaunchTextMessageIntent (string contactNumber);
		void LaunchPhoneCallIntent (string contactNumber);
		void LaunchEmailIntent (string email);
	}

    public class ClientMyConsultantBodyView : RecyclerFragment<ReactiveRecyclerAdapter, ReactiveAdapterModel>,
											  IClientMyConsultantBodyView
	{
		private IClientMyConsultantBodyPresenter presenter;
        protected override void OnCreateInitialize ()
		{
			IsAnimated = true;
			presenter = new ClientMyConsultantBodyPresenter (this);
			// load who this client's consultant is
        }

        protected override int ResourceLayout => Resource.Layout.list_frag_generic;
		protected override int? ResourceIdLinearLayout => Resource.Id.list_frag_gen_linear;
		protected override int? ResourceIdRecyclerView => Resource.Id.list_frag_gen_recyclerView;
		protected override int? ResourceIdProgressBar => Resource.Id.list_frag_gen_progress;

        protected override void LoadInitialDataSet ()
		{
			// data set refers to that list to be displayed in the recycler view
			// which are the card views that allows the client to contact their consultant

			presenter.LoadMyConsultantContactMethods ();
		}

        #region IClientMyConsultantBodyView

        public void DisplayContactMethods (List<ReactiveAdapterModel> contactMethods)
		{
			UpdateAdapterDataSet(contactMethods);
			HideProgressBar();
		}

		public void LaunchTextMessageIntent (string contactNumber)
		{
			Uri textUri = Uri.Parse($"smsto:{contactNumber}");
			Intent textIntent = new Intent (Intent.ActionSendto, textUri);
			textIntent.PutExtra ("sms_body", "Insert your Message");
			
			StartActivity (textIntent);
		}

		public void LaunchPhoneCallIntent (string contactNumber)
		{
			Uri callUri = Uri.FromParts ("tel", contactNumber, null);
			Intent callIntent = new Intent (Intent.ActionDial, callUri);

			StartActivity (callIntent);
		}

		public void LaunchEmailIntent (string email)
		{
			Intent emailIntent = new Intent (Intent.ActionSendto);
			emailIntent.PutExtra(Intent.ExtraEmail, new string[] { email });
			emailIntent.PutExtra(Intent.ExtraSubject, "Subject");
			emailIntent.PutExtra(Intent.ExtraText, "Message");
			emailIntent.SetType ("message/rfc822");

			StartActivity (Intent.CreateChooser(emailIntent, "Send Email Via"));
		}

		#endregion
    }
}
