using System;
using System.Collections.Generic;
using Android.Support.V7.Widget;
using Android.Views;
using PeriwinkleApp.Android.Source.AdapterModels;
using PeriwinkleApp.Android.Source.ViewHolders;

namespace PeriwinkleApp.Android.Source.Adapters
{
	public class BehaviorRecyclerAdapter : BaseRecyclerAdapter <BehaviorAdapterModel>
	{
		public EventHandler ViewReportClicked { get; set; }

		public BehaviorRecyclerAdapter () : base (null) { }
		public BehaviorRecyclerAdapter (List <BehaviorAdapterModel> dataset = null) : base (dataset) { }

		public override void OnBindViewHolder (RecyclerView.ViewHolder holder, int position)
		{
			CardBehaviorViewHolder viewHolder = (CardBehaviorViewHolder) holder;

			viewHolder.TextFilename.Text = DataSet[position].Filename.Substring (0, DataSet[position].Filename.Length - 4);
			viewHolder.TextStartTime.Text = "Start DateTime: " + DataSet[position].StartTime.ToString("F");
			viewHolder.TextStopTime.Text = "Stop DateTime: " + DataSet[position].StopTime.ToString("F");
			viewHolder.AddButtonViewClicked (DataSet[position].ViewReportClicked, position);
		}
		
		public override RecyclerView.ViewHolder OnCreateViewHolder (ViewGroup parent, int viewType)
		{
			View itemView = LayoutInflater.From(parent.Context)
										  .Inflate(Resource.Layout.card_view_bhv_graph, parent, false);
			
			CardBehaviorViewHolder viewHolder = new CardBehaviorViewHolder(itemView, OnClick);
			return viewHolder;
        }
	}
}
