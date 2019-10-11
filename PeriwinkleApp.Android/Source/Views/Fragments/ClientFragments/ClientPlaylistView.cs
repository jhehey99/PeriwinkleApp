using System.Collections.Generic;
using Android.Content;
using Android.OS;
using PeriwinkleApp.Android.Source.AdapterModels;
using PeriwinkleApp.Android.Source.Adapters;
using PeriwinkleApp.Android.Source.Presenters.ClientPresenters;
using PeriwinkleApp.Android.Source.Views.Fragments.Common;

namespace PeriwinkleApp.Android.Source.Views.Fragments.ClientFragments
{
	public interface IClientPlaylistView
	{
		// load playlist entertainment bla bla
		// load dataset
		void DisplayPlaylists (List<ReactiveAdapterModel> playlistDataSet);
		void LaunchPlaylist (string url);
	}


    public class ClientPlaylistView : RecyclerFragment<ReactiveRecyclerAdapter, ReactiveAdapterModel>,
									  IClientPlaylistView
	{
		private IClientPlaylistPresenter presenter;
		protected override void OnCreateInitialize ()
		{
			IsAnimated = true;
			presenter = new ClientPlaylistPresenter (this);
        }

		protected override int ResourceLayout => Resource.Layout.list_frag_generic;
		protected override int? ResourceIdLinearLayout => Resource.Id.list_frag_gen_linear;
		protected override int? ResourceIdRecyclerView => Resource.Id.list_frag_gen_recyclerView;
		protected override int? ResourceIdProgressBar => Resource.Id.list_frag_gen_progress;

        protected override void LoadInitialDataSet ()
		{
			presenter.LoadPlaylists();
		}

		protected override void OnItemClick (object sender, int position)
		{
			base.OnItemClick (sender, position);
			presenter.LoadPlaylist (position);
		}

		public void DisplayPlaylists (List <ReactiveAdapterModel> playlistDataSet)
		{
			UpdateAdapterDataSet(playlistDataSet);
			HideProgressBar();
		}

		public void LaunchPlaylist (string url)
		{
			Intent intent = new Intent(Context, typeof(ClientEntertainmentView));
			intent.PutExtra ("spotifyPlaylist", url);
			StartActivity (intent);
		}
	}
}
