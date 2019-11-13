using System;
using System.Collections.Generic;
using Android.Support.V7.Widget;
using Android.Views;
using PeriwinkleApp.Android.Source.AdapterModels;
using PeriwinkleApp.Android.Source.ViewHolders;
using PeriwinkleApp.Core.Sources.Models.Domain;

namespace PeriwinkleApp.Android.Source.Adapters
{
	public class SensorRecordRecyclerAdapter : BaseRecyclerAdapter<SensorRecordAdapterModel>
	{
		public EventHandler ViewReportClicked { get; set; }
		public SensorRecordRecyclerAdapter() : base(null) { }
		public SensorRecordRecyclerAdapter(List<SensorRecordAdapterModel> dataset = null) : base(dataset) { }

		public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
		{
			CardSensorRecordViewHolder viewHolder = (CardSensorRecordViewHolder)holder;

			SensorRecordType type = DataSet[position].RecordType;
			string recordType = "";
			if (type == SensorRecordType.Acceleration)
				recordType = "Acceleration";
			else
				recordType = "Piezo";

			viewHolder.TextRecordType.Text = recordType;
			viewHolder.TextStartTime.Text = "Start DateTime: " + DataSet[position].StartTime.ToString("F");
			viewHolder.TextStopTime.Text = "Stop DateTime: " + DataSet[position].StopTime.ToString("F");
			viewHolder.AddButtonViewClicked(DataSet[position].ViewReportClicked, position);
		}

		public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
		{
			View itemView = LayoutInflater.From(parent.Context)
										  .Inflate(Resource.Layout.card_view_sensor_record, parent, false);

			CardSensorRecordViewHolder viewHolder = new CardSensorRecordViewHolder(itemView, OnClick);
			return viewHolder;
		}

	}
}