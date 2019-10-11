using Android.Views;
using Android.Support.V7.Widget;
using PeriwinkleApp.Android.Source.AdapterModels;
using System.Collections.Generic;
using PeriwinkleApp.Android.Source.ViewHolders;

namespace PeriwinkleApp.Android.Source.Adapters
{
    public class AccountRecyclerAdapter : BaseRecyclerAdapter<AccountAdapterModel>
    {
		public AccountRecyclerAdapter () : base (null) { }

        public AccountRecyclerAdapter(List<AccountAdapterModel> dataset = null) : base(dataset) { }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            CardItemViewHolder viewHolder = (CardItemViewHolder) holder;

            // Set the CardView's TextViews
            viewHolder.ImageAvatar.SetImageResource(Resource.Drawable.avatar_male);
            viewHolder.TextName.Text = DataSet[position].Name;
            viewHolder.TextEmail.Text = DataSet[position].Email;
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View itemView = LayoutInflater.From(parent.Context).
                                Inflate(Resource.Layout.card_view_item, parent, false);

            CardItemViewHolder viewHolder = new CardItemViewHolder(itemView, OnClick);
            return viewHolder;
        }
    }
}
