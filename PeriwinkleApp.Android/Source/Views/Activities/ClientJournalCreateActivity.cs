using System;
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

namespace PeriwinkleApp.Android.Source.Views.Activities
{
	public interface IClientJournalCreateActivity
	{
		void BackToJournalListView ();
	}

	[Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = false)]
    public class ClientJournalCreateActivity : AppCompatActivity, IClientJournalCreateActivity
    {
		private EditText txtTitle, txtBody;
		private Button btnCreate;

		private IClientJournalCreatePresenter presenter;

        protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);

			SetContentView (Resource.Layout.client_frag_journal_creates);

			Window.SetSoftInputMode(SoftInput.StateHidden);

			// Edit Texts
			txtTitle = FindViewById<EditText>(Resource.Id.txt_create_journal_title);
			txtBody = FindViewById<EditText>(Resource.Id.txt_create_journal_body);

            // Button
			btnCreate = FindViewById<Button>(Resource.Id.btn_create_journal);
			btnCreate.Click += OnCreateJournalClicked;

            presenter = new ClientJournalCreatePresenter (this);
        }

        private async void OnCreateJournalClicked (object sender, EventArgs e)
		{
			string title = txtTitle.Text;
			string body = txtBody.Text;

			JournalEntry journal = new JournalEntry()
								   {
									   Title = title,
									   Body = body,
									   DateTimeCreated = DateTime.Now
								   };

			await presenter.AddJournalEntry(journal);
		}

		public void BackToJournalListView()
		{
			OnBackPressed ();
		}
		
        public override void OnBackPressed ()
		{
			base.OnBackPressed ();

			CacheProvider.Set<int> (CacheKey.BackFragItem, Resource.Id.cli_menu_journal);
			FinishActivity (1001);
		}

        public override bool OnTouchEvent(MotionEvent e)
		{
			InputMethodManager imm = (InputMethodManager)GetSystemService(Context.InputMethodService);
			imm.HideSoftInputFromWindow(Window.DecorView.WindowToken, 0);
			return base.OnTouchEvent(e);
		}
    }
}
