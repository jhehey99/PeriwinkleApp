using System;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using PeriwinkleApp.Core.Sources.Utils;

namespace PeriwinkleApp.Android.Source.ViewHolders
{
	public class CardReactiveViewHolder : RecyclerView.ViewHolder
	{
		public TextView TextTitle { get; private set; }
		public TextView TextMessage { get; private set; }
		public Button ButtonAction { get; private set; }

		public CardReactiveViewHolder(View itemView, Action<int> listener) : base(itemView)
		{
			TextTitle = itemView.FindViewById<TextView>(Resource.Id.card_view_notif_rct_ttl);
			TextMessage = itemView.FindViewById<TextView>(Resource.Id.card_view_notif_rct_msg);
			ButtonAction = itemView.FindViewById <Button> (Resource.Id.card_view_notif_rct_action);
			itemView.Click += (sender, e) => listener(base.LayoutPosition);
		}

		public void AddButtonActionClicked (EventHandler actionClicked)
		{
			ButtonAction.Click += actionClicked;
        }
	}
}
