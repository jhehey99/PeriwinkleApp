using System;
using System.IO;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Preferences;
using Android.Test.Mock;
using Android.Views;
using Android.Widget;
using PeriwinkleApp.Core.Sources.Extensions;
using PeriwinkleApp.Core.Sources.Models.Domain;
using PeriwinkleApp.Core.Sources.Services;
using PeriwinkleApp.Core.Sources.Utils;
using Fragment = Android.Support.V4.App.Fragment;

namespace Production.Journals.Source
{
    public class JournalCreateView : Fragment
    {
        public static readonly int PickImageId = 1000;

        private EditText txtTitle, txtBody;
        private Button btnUpload, btnCreate;
        private ImageView imgPicture;

        public override void OnCreate (Bundle savedInstanceState)
        {
            base.OnCreate (savedInstanceState);
        }

        public override View OnCreateView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            return inflater.Inflate (Resource.Layout.journal_create, container, false);
        }

        public override void OnViewCreated (View view, Bundle savedInstanceState)
        {
            base.OnViewCreated (view, savedInstanceState);

            // Edit Texts
            txtTitle = view.FindViewById<EditText>(Resource.Id.txt_create_journal_title);
            txtBody = view.FindViewById<EditText>(Resource.Id.txt_create_journal_body);

            // Button
            btnUpload = view.FindViewById <Button> (Resource.Id.btn_upload_image);
            btnCreate = view.FindViewById <Button> (Resource.Id.btn_create_journal);
            btnUpload.Click += OnButtonUploadClicked;
            btnCreate.Click += OnButtonCreateClicked;

            // Image View
            imgPicture = view.FindViewById <ImageView> (Resource.Id.img_create_journal);
        }

        private void OnButtonUploadClicked (object sender, EventArgs e)
        {
            Intent intent = new Intent();
            intent.SetType("image/*");
            intent.SetAction(Intent.ActionGetContent);
            StartActivityForResult(Intent.CreateChooser(intent, "Select Picture"), PickImageId);
        }

        

        private void OnButtonCreateClicked (object sender, EventArgs e)
        {
            //TODO: title < 256 chars
            //TODO: body < 65535 chars
            string title = txtTitle.Text;
            string body = txtBody.Text;

            JournalEntry entry = new JournalEntry ()
                                 {
                                     JournalEntryId = 1,
                                     JournalClientId = 1,
                                     Title = title,
                                     Body = body,
                                     DateTimeCreated = DateTime.Now
                                 };

            Logger.Log (entry.PrettySerialize ());
        }

        public override async void OnActivityResult (int requestCode, int resultCode, Intent data)
        {
            if ((requestCode != PickImageId) || (resultCode != (int) Result.Ok) || (data == null))
                return;
            
            Android.Net.Uri uri = data.Data;
            Logger.Log (uri.ToString ());
            imgPicture.SetImageURI(uri);


            var x = Convert.ToBase64String (ConvertImageToByte (uri));
            JournalService service = new JournalService ();
            Image64 img = new Image64 ()
                          {
                              ImageString = x
                          };
            await service.SendImageUri (img);
        }
        
        public byte[] ConvertImageToByte(Android.Net.Uri uri)
        {
            Stream stream = Application.Context.ContentResolver.OpenInputStream(uri);
            byte[] byteArray;
            
            using (var memoryStream = new MemoryStream())
            {
                stream.CopyTo(memoryStream);
                byteArray = memoryStream.ToArray();
            }
            return byteArray;
        }
    }
}
