using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Views;
using Android.Webkit;

namespace PeriwinkleApp.Android.Source.Views.Fragments.ClientFragments
{
	[Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = false)]
    public class ClientEntertainmentView : AppCompatActivity
	{
		private WebView webView;
		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
			// Set our view from the "main" layout resource
			SetContentView(Resource.Layout.client_frag_entertainment);


			string url = this.Intent.GetStringExtra ("spotifyPlaylist") ?? null;
			if (url == null)
				return;
			
            webView = FindViewById<WebView>(Resource.Id.webview);
			webView.Settings.JavaScriptEnabled = true;
			webView.SetWebViewClient(new SpotifyWebClient());
			webView.LoadUrl(url);
        }

        public override bool OnKeyDown(Keycode keyCode, KeyEvent e)
		{
			if (keyCode == Keycode.Back && webView.CanGoBack())
			{
				webView.GoBack();
				return true;
			}
			return base.OnKeyDown(keyCode, e);
		}
    }

	public class SpotifyWebClient : WebViewClient
	{
		public override bool ShouldOverrideUrlLoading(WebView view, IWebResourceRequest request)
		{
			view.LoadUrl(request.Url.ToString());
			return false;
		}
	}
}
