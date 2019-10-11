using System.Collections.Generic;
using Android.Support.V7.Widget;
using Android.Views;
using PeriwinkleApp.Android.Source.AdapterModels;
using PeriwinkleApp.Android.Source.ViewHolders;

namespace PeriwinkleApp.Android.Source.Adapters
{
	public class ReminderRecyclerAdapter : BaseRecyclerAdapter<ReminderAdapterModel>
    {
		public ReminderRecyclerAdapter () : base (null) { }
		public ReminderRecyclerAdapter (List <ReminderAdapterModel> dataset = null) : base (dataset) { }

		public override void OnBindViewHolder (RecyclerView.ViewHolder holder, int position)
		{
			CardReminderViewHolder viewHolder = (CardReminderViewHolder) holder;

            // Set the CardView's Elements
			viewHolder.TextTitle.Text = DataSet[position].Title;
			viewHolder.TextMessage.Text = DataSet[position].Message;
		}

		public override RecyclerView.ViewHolder OnCreateViewHolder (ViewGroup parent, int viewType)
		{
			View itemView = LayoutInflater.From (parent.Context)
										  .Inflate (Resource.Layout.card_view_notif_reminder, parent, false);

			CardReminderViewHolder viewHolder = new CardReminderViewHolder (itemView, OnClick);
			return viewHolder;
		}
	}
}
