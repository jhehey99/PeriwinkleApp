using System;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;

namespace PeriwinkleApp.Android.Source.ViewHolders
{
	public class CardNotificationViewHolder : RecyclerView.ViewHolder
    {
		public TextView TextTitle { get; private set; }
		public TextView TextMessage { get; private set; }
		public Button ButtonAction { get; private set; }

		public CardNotificationViewHolder(View itemView, Action<int> listener) : base(itemView)
		{
			TextTitle = itemView.FindViewById<TextView>(Resource.Id.notif_title);
			TextMessage = itemView.FindViewById<TextView>(Resource.Id.notif_msg);
			ButtonAction = itemView.FindViewById<Button>(Resource.Id.notif_action);
			itemView.Click += (sender, e) => listener(base.LayoutPosition);
		}

		public void AddButtonActionClicked(EventHandler actionClicked)
		{
			ButtonAction.Click += actionClicked;
		}
		
		public void SetHasAction (bool hasAction)
		{
			ButtonAction.Visibility = hasAction ? ViewStates.Visible : ViewStates.Gone;
        }

    }
}
