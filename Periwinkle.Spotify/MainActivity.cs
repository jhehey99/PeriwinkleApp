using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Webkit;

namespace Periwinkle.Spotify
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
	{
		private WebView webView;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);

			webView = FindViewById<WebView>(Resource.Id.webview);
			webView.Settings.JavaScriptEnabled = true;
			webView.SetWebViewClient(new SpotifyWebClient());
			webView.LoadUrl("https://open.spotify.com/artist/0ZXi1NG0Wwlaj70Qn25mAr");
        }

		public override bool OnKeyDown(Android.Views.Keycode keyCode, Android.Views.KeyEvent e)
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