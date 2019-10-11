using System;
using Android.Widget;
using Android.Views;
using Android.Support.V7.Widget;

namespace PeriwinkleApp.Android.Source.ViewHolders
{
    public class CardItemViewHolder : RecyclerView.ViewHolder
    {
        public ImageView ImageAvatar { get; private set; }
        public TextView TextName { get; private set; }
        public TextView TextEmail { get; private set; }

        public CardItemViewHolder(View itemView, Action<int> listener) : base(itemView)
        {
            ImageAvatar = itemView.FindViewById<ImageView>(Resource.Id.card_view_item_image);
            TextName = itemView.FindViewById<TextView>(Resource.Id.card_view_item_cap1);
            TextEmail = itemView.FindViewById<TextView>(Resource.Id.card_view_item_cap2);
            itemView.Click += (sender, e) => listener(base.LayoutPosition);
        }
    }
}
