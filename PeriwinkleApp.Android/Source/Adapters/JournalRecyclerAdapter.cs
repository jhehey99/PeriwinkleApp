using System;
using System.Collections.Generic;
using Android.Support.V7.Widget;
using Android.Views;
using PeriwinkleApp.Android.Source.AdapterModels;
using PeriwinkleApp.Android.Source.ViewHolders;

namespace PeriwinkleApp.Android.Source.Adapters
{
	public class JournalRecyclerAdapter : BaseRecyclerAdapter<JournalAdapterModel>
    {
		public EventHandler ViewJournalClicked { get; set; }

		public JournalRecyclerAdapter() : base(null) { }
		public JournalRecyclerAdapter(List<JournalAdapterModel> dataset = null) : base(dataset) { }

		public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
		{
			CardJournalViewHolder viewHolder = (CardJournalViewHolder)holder;

			viewHolder.TextTitle.Text = DataSet[position].Title;
			viewHolder.TextDateCreated.Text = "Date Created: " + DataSet[position].DateCreated.ToString("D");
			viewHolder.AddButtonViewClicked(DataSet[position].ViewJournalClicked, position);
		}
		
		public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
		{
			View itemView = LayoutInflater.From(parent.Context)
										  .Inflate(Resource.Layout.card_view_journal, parent, false);

			CardJournalViewHolder viewHolder = new CardJournalViewHolder(itemView, OnClick);
			return viewHolder;
		}

    }
}
