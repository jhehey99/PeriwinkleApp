using System;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;

namespace PeriwinkleApp.Android.Source.ViewHolders
{
	public class CardSensorRecordViewHolder : RecyclerView.ViewHolder
	{
		public TextView TextFilename { get; private set; }
		public TextView TextStartTime { get; private set; }
		public TextView TextStopTime { get; private set; }
		public Button ButtonViewReport { get; private set; }

		public CardSensorRecordViewHolder(View itemView, Action<int> listener) : base(itemView)
		{
			TextFilename = itemView.FindViewById<TextView>(Resource.Id.card_view_bhv_filename);
			TextStartTime = itemView.FindViewById<TextView>(Resource.Id.card_view_bhv_starttime);
			TextStopTime = itemView.FindViewById<TextView>(Resource.Id.card_view_bhv_stoptime);
			ButtonViewReport = itemView.FindViewById<Button>(Resource.Id.card_view_bhv_report);
			itemView.Click += (sender, e) => listener(base.LayoutPosition);
		}

		public void AddButtonViewClicked(EventHandler<int> viewClicked, int position)
		{
			ButtonViewReport.Click += (sender, e) => { viewClicked(sender, position); };
		}
	}
}