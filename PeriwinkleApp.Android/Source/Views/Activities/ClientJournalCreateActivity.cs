using System;
using System.IO;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.V7.App;
using Android.Views;
using Android.Views.InputMethods;
using Android.Widget;
using PeriwinkleApp.Android.Source.Cache;
using PeriwinkleApp.Android.Source.Presenters.ClientPresenters;
using PeriwinkleApp.Core.Sources.Models.Domain;
using Uri = Android.Net.Uri;

namespace PeriwinkleApp.Android.Source.Views.Activities
{
	public interface IClientJournalCreateActivity
	{
		void BackToJournalListView ();
	}

	[Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = false)]
    public class ClientJournalCreateActivity : AppCompatActivity, IClientJournalCreateActivity
    {
		public static readonly int PickImageId = 1000;

		private EditText txtTitle, txtBody;
		private Button btnAttach, btnCreate;
		private ImageView imgPicture;

		private IClientJournalCreatePresenter presenter;
		private byte[] imageBytes;


        protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);

			SetContentView (Resource.Layout.client_frag_journal_creates);

			Window.SetSoftInputMode(SoftInput.StateHidden);

			// Edit Texts
			txtTitle = FindViewById<EditText>(Resource.Id.txt_create_journal_title);
			txtBody = FindViewById<EditText>(Resource.Id.txt_create_journal_body);

            // Button
			btnAttach = FindViewById<Button>(Resource.Id.btn_upload_image);
			btnCreate = FindViewById<Button>(Resource.Id.btn_create_journal);
			btnAttach.Click += OnAttachImageClicked;
			btnCreate.Click += OnCreateJournalClicked;

			// Image View
			imgPicture = FindViewById<ImageView>(Resource.Id.img_create_journal);

            presenter = new ClientJournalCreatePresenter (this);

        }

		private void OnAttachImageClicked(object sender, EventArgs e)
		{
			Intent intent = new Intent();
			intent.SetType("image/*");
			intent.SetAction(Intent.ActionGetContent);
			StartActivityForResult(Intent.CreateChooser(intent, "Select Picture"), PickImageId);
		}

        private async void OnCreateJournalClicked (object sender, EventArgs e)
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

			await presenter.AddJournalEntry(journal);
		}

		protected override void OnActivityResult (int requestCode, Result resultCode, Intent data)
		{
            //			base.OnActivityResult (requestCode, resultCode, data);


			if ((requestCode != PickImageId) || (resultCode != Result.Ok) || (data == null))
				return;

			Uri uri = data.Data;
			//			Logger.Log(uri.ToString());
			imgPicture.SetImageURI(uri);

			// convert to bytes array
			imageBytes = ConvertImageToByte(uri);

        }

		public void BackToJournalListView()
		{
            //Resource.Id.cli_menu_journal
			OnBackPressed ();
		}

        public override void OnBackPressed ()
		{
			base.OnBackPressed ();

			CacheProvider.Set<int> (CacheKey.BackFragItem, Resource.Id.cli_menu_journal);
			FinishActivity (1001);
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

        public override bool OnTouchEvent(MotionEvent e)
		{
			InputMethodManager imm = (InputMethodManager)GetSystemService(Context.InputMethodService);
			imm.HideSoftInputFromWindow(Window.DecorView.WindowToken, 0);
			return base.OnTouchEvent(e);
		}

		
    }
}
