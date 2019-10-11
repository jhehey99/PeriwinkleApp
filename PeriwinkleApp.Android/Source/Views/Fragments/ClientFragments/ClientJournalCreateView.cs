using System;
using System.IO;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Provider;
using Android.Views;
using Android.Widget;
using PeriwinkleApp.Android.Source.Presenters.ClientPresenters;
using PeriwinkleApp.Android.Source.Views.Activities;
using PeriwinkleApp.Android.Source.Views.Fragments.Common;
using PeriwinkleApp.Core.Sources.Models.Domain;
using Fragment = Android.Support.V4.App.Fragment;
using FragmentTransaction = Android.Support.V4.App.FragmentTransaction;
using Uri = Android.Net.Uri;

namespace PeriwinkleApp.Android.Source.Views.Fragments.ClientFragments
{
    public class ClientJournalCreateView : HideSoftInputFragment, IClientJournalCreateActivity
    {
		public static readonly int PickImageId = 1000;

        private EditText txtTitle, txtBody;
		private Button btnUpload, btnCreate;
		private ImageView imgPicture;

        private IClientJournalCreatePresenter presenter;
		private byte[] imageBytes;

		public ClientJournalCreateView() { }

        public ClientJournalCreateView (byte[] imagesBytes)
		{
			this.imageBytes = imagesBytes;
		}

        public override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);
			presenter = new ClientJournalCreatePresenter (this);
			imageBytes = null;
		}

		public override View OnCreateView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			return inflater.Inflate (Resource.Layout.client_frag_journal_create, container, false);
		}

		public override void OnViewCreated (View view, Bundle savedInstanceState)
		{
			base.OnViewCreated (view, savedInstanceState);


			// Edit Texts
			txtTitle = view.FindViewById<EditText>(Resource.Id.txt_create_journal_title);
			txtBody = view.FindViewById<EditText>(Resource.Id.txt_create_journal_body);

			// Button
			btnUpload = view.FindViewById<Button>(Resource.Id.btn_upload_image);
			btnCreate = view.FindViewById<Button>(Resource.Id.btn_create_journal);
			btnUpload.Click += OnButtonUploadClicked;
			btnCreate.Click += OnButtonCreateClicked;

			// Image View
			imgPicture = view.FindViewById<ImageView>(Resource.Id.img_create_journal);
        }

		private void OnButtonUploadClicked(object sender, EventArgs e)
		{
            Intent intent = new Intent();
			intent.SetType("image/*");
			intent.SetAction(Intent.ActionGetContent);
			StartActivityForResult(Intent.CreateChooser(intent, "Select Picture"), PickImageId);
		}

        private async void OnButtonCreateClicked (object sender, EventArgs e)
		{
			//TODO: title < 256 chars
			//TODO: body < 65535 chars
			string title = txtTitle.Text;
			string body = txtBody.Text;

			// create journal entry model
			//TODO SA PRESENTER I-SET UNG CLIENT ID
			JournalEntry journal = new JournalEntry()
								 {
									 Title = title,
									 Body = body,
									 DateTimeCreated = DateTime.Now,
									 ImageBytes = imageBytes
								 };

			await presenter.AddJournalEntry (journal);


			
//            ClientSession cliSession = SessionFactory.ReadSession<ClientSession>(SessionKeys.LoggedClient);
//
//			if (cliSession == null)
//				throw new Exception("Client session problem");
//
//			int? clientid = cliSession?.ClientId;
//
//            
//
//			Logger.Log(journal.PrettySerialize());
//
//			JournalService service = new JournalService();
//			if(image != null)
//				await service.SendImageUri(image);
//
//			await service.CreateEntry (journal);
		}
		
        private Image64 image;

		public override void OnActivityResult (int requestCode, int resultCode, Intent data)
		{
//			base.OnActivityResult (requestCode, resultCode, data);

			if ((requestCode != PickImageId) || (resultCode != (int)Result.Ok) || (data == null))
				return;

			Uri uri = data.Data;
			//			Logger.Log(uri.ToString());
			imgPicture.SetImageURI(uri);

			// convert to bytes array
			imageBytes = ConvertImageToByte (uri);

			FragmentTransaction ft = Activity.SupportFragmentManager.BeginTransaction();
			Fragment fragment = new ClientJournalCreateView(imageBytes);

			ft.Replace(Resource.Id.fragment_container, fragment);
			ft.AddToBackStack(null);
			ft.Commit();


            //			var imgString = Convert.ToBase64String(ConvertImageToByte(uri));
            //			image = new Image64() { ImageString = imgString };
        }
		
		public byte[] ConvertImageToByte(Uri uri)
		{
			Stream stream = Application.Context.ContentResolver.OpenInputStream(uri);
			byte[] byteArray;

			using (MemoryStream memoryStream = new MemoryStream())
			{
				stream.CopyTo(memoryStream);
				byteArray = memoryStream.ToArray();
			}
			return byteArray;
		}

		public void BackToJournalListView ()
		{
			throw new NotImplementedException ();
		}
	}
}
