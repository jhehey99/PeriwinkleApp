using System.Collections.Generic;
using PeriwinkleApp.Android.Source.AdapterModels;
using PeriwinkleApp.Android.Source.Views.Fragments.ClientFragments;

namespace PeriwinkleApp.Android.Source.Presenters.ClientPresenters
{
	public interface IClientPlaylistPresenter
	{
		void LoadPlaylists ();
		void LoadPlaylist (int position);
	}

    public class ClientPlaylistPresenter : IClientPlaylistPresenter
	{
		private IClientPlaylistView view;
		private List <string> urls;

        public ClientPlaylistPresenter (IClientPlaylistView view)
		{
			this.view = view;
			urls = new List <string> ()
				   {
					   "https://open.spotify.com/playlist/37i9dQZF1DXcZQSjptOQtk",
                       "https://open.spotify.com/playlist/37i9dQZF1DX4WYpdgoIcn6",
                       "https://open.spotify.com/artist/7xbVj2U2bY22gyZnh04TlN",
                       "https://open.spotify.com/playlist/37i9dQZF1DXci7j0DJQgGp",
                   };

		}

		public void LoadPlaylists ()
		{
			ReactiveAdapterModel model1 = new ReactiveAdapterModel() { Title = "Top Hit Philippines" };
			ReactiveAdapterModel model2 = new ReactiveAdapterModel() { Title = "Chill Hits" };
			ReactiveAdapterModel model3 = new ReactiveAdapterModel() { Title = "Relaxing Music Therapy" };
			ReactiveAdapterModel model4 = new ReactiveAdapterModel() { Title = "Hanging Out and Relaxing" };

			List<ReactiveAdapterModel> dataSet = new List<ReactiveAdapterModel>() { model1, model2, model3, model4 };
			view.DisplayPlaylists (dataSet);
		}

		public void LoadPlaylist (int position)
		{
			if (position > urls.Count)
				return;

			string url = urls[position];
			view.LaunchPlaylist (url);
		}
	}
}
