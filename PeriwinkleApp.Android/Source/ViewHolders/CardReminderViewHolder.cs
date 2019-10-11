using System;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;

namespace PeriwinkleApp.Android.Source.ViewHolders
{
	public class CardReminderViewHolder : RecyclerView.ViewHolder
	{
		public TextView TextTitle { get; private set; }
		public TextView TextMessage { get; private set; }

		public CardReminderViewHolder (View itemView, Action <int> listener) : base (itemView)
		{
			TextTitle = itemView.FindViewById <TextView> (Resource.Id.card_view_notif_rmn_ttl);
			TextMessage = itemView.FindViewById <TextView> (Resource.Id.card_view_notif_rmn_msg);
			itemView.Click += (sender, e) => listener (base.LayoutPosition);
		}
	}
}
