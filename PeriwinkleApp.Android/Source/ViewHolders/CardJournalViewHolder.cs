using System;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;

namespace PeriwinkleApp.Android.Source.ViewHolders
{
	public class CardJournalViewHolder : RecyclerView.ViewHolder
	{
		public TextView TextTitle { get; private set; }
		public TextView TextDateCreated { get; private set; }
		public Button ButtonViewJournal { get; private set; }

		public CardJournalViewHolder(View itemView, Action<int> listener) : base(itemView)
		{
			TextTitle = itemView.FindViewById<TextView>(Resource.Id.card_view_journal_title);
			TextDateCreated = itemView.FindViewById<TextView>(Resource.Id.card_view_journal_date);
			ButtonViewJournal = itemView.FindViewById<Button>(Resource.Id.card_view_journal_view);
			itemView.Click += (sender, e) => listener(base.LayoutPosition);
		}

		public void AddButtonViewClicked(EventHandler<int> viewClicked, int position)
		{
			ButtonViewJournal.Click += (sender, e) => { viewClicked(sender, position); };
		}
    }
}
