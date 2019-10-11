using System;
using System.Collections.Generic;
using Android.Support.V7.Widget;
using Android.Views;
using PeriwinkleApp.Android.Source.AdapterModels;
using PeriwinkleApp.Android.Source.ViewHolders;

namespace PeriwinkleApp.Android.Source.Adapters
{
	public class ReactiveRecyclerAdapter : BaseRecyclerAdapter <ReactiveAdapterModel>
	{
		public EventHandler ActionClicked { get; set; }

		public ReactiveRecyclerAdapter() : base(null) { }
		public ReactiveRecyclerAdapter(List<ReactiveAdapterModel> dataset = null) : base(dataset) { }

        public override void OnBindViewHolder (RecyclerView.ViewHolder holder, int position)
		{
			CardReactiveViewHolder viewHolder = (CardReactiveViewHolder)holder;

			// Set the CardView's Elements
			viewHolder.TextTitle.Text = DataSet[position].Title;
			viewHolder.TextMessage.Text = DataSet[position].Message;
			viewHolder.AddButtonActionClicked (DataSet[position].ActionClicked);
			//TODO CUSTOM EVENT ARGS PARA SA ACTION CLICKED, KUNYARE MAGKAKAIBA SILA NG BUTTON ACTIONS
		}

		public override RecyclerView.ViewHolder OnCreateViewHolder (ViewGroup parent, int viewType)
		{
			View itemView = LayoutInflater.From(parent.Context)
										  .Inflate(Resource.Layout.card_view_notif_reactive, parent, false);

			CardReactiveViewHolder viewHolder = new CardReactiveViewHolder(itemView, OnClick);
			return viewHolder;
		}
	}
}
