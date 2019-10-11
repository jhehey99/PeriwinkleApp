using System;
using System.Collections.Generic;
using Android.Support.V7.Widget;
using Android.Views;
using PeriwinkleApp.Android.Source.AdapterModels;
using PeriwinkleApp.Android.Source.ViewHolders;

namespace PeriwinkleApp.Android.Source.Adapters
{
	public class NotificationRecyclerAdapter : BaseRecyclerAdapter<NotificationAdapterModel>
    {
		public NotificationRecyclerAdapter() : base(null) { }
		public NotificationRecyclerAdapter(List<NotificationAdapterModel> dataset = null) : base(dataset) { }

		public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
		{
			CardNotificationViewHolder viewHolder = (CardNotificationViewHolder)holder;

			// Set the CardView's Elements
			viewHolder.TextTitle.Text = DataSet[position].Title;
			viewHolder.TextMessage.Text = DataSet[position].Message;
			viewHolder.AddButtonActionClicked(DataSet[position].ActionClicked);
			viewHolder.SetHasAction (DataSet[position].HasAction);
			//TODO CUSTOM EVENT ARGS PARA SA ACTION CLICKED, KUNYARE MAGKAKAIBA SILA NG BUTTON ACTIONS
		}
		
		public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
		{
			View itemView = LayoutInflater.From(parent.Context)
										  .Inflate(Resource.Layout.card_view_notification, parent, false);

			CardNotificationViewHolder viewHolder = new CardNotificationViewHolder(itemView, OnClick);
			return viewHolder;
		}
    }
}
