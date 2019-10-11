
using Android.Support.V7.Widget;
using Android.Views;
using PeriwinkleApp.Android.Source.AdapterModels;
using PeriwinkleApp.Android.Source.ViewHolders;
using System.Collections.Generic;

namespace PeriwinkleApp.Android.Source.Adapters
{
	public class ResponseRecyclerAdapter : BaseRecyclerAdapter<ResponseAdapterModel>
	{
		public ResponseRecyclerAdapter() : base(null) { }

		public ResponseRecyclerAdapter(List<ResponseAdapterModel> dataset = null) : base(dataset) { }

		public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
		{
			CardItemViewHolder viewHolder = (CardItemViewHolder)holder;

			// Set the CardView's TextViews
			viewHolder.ImageAvatar.SetImageResource(Resource.Drawable.avatar_male);
			viewHolder.TextName.Text = DataSet[position].Date;
			viewHolder.TextEmail.Text = DataSet[position].BMI;
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