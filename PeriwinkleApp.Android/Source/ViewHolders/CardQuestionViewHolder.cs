using System;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;

namespace PeriwinkleApp.Android.Source.ViewHolders
{
    public class CardQuestionViewHolder : RecyclerView.ViewHolder
    {
        public TextView TextId { get; private set; }
        public TextView TextQuestion { get; private set; }
		public RadioGroup RadioScale { get; private set; }

        public CardQuestionViewHolder (View itemView, Action <int> listener) : base (itemView)
        {
            TextId = itemView.FindViewById<TextView>(Resource.Id.card_view_qtn_id);
            TextQuestion = itemView.FindViewById<TextView>(Resource.Id.card_view_qtn_qtn);
			RadioScale = itemView.FindViewById<RadioGroup>(Resource.Id.radio_scale);
			RadioScale.CheckedChange += RadioItemChanged;
            itemView.Click += (sender, e) => listener(base.LayoutPosition);
        }

		public EventHandler<QuestionScaleCheckedEventArgs> QuestionScaleChecked;

		private void RadioItemChanged(object sender, RadioGroup.CheckedChangeEventArgs e)
		{
			RadioButton radioSelected = ItemView.FindViewById<RadioButton>(RadioScale.CheckedRadioButtonId);
			int.TryParse(radioSelected.Text, out int scale);

			QuestionScaleCheckedEventArgs ea = new QuestionScaleCheckedEventArgs(e)
			{
				ScalePosition = LayoutPosition,
				ScaleValue = scale
			};
			QuestionScaleChecked(sender, ea);
		}
	}

	public class QuestionScaleCheckedEventArgs : RadioGroup.CheckedChangeEventArgs
	{ 
		public int ScalePosition { get; set; }
		public int ScaleValue { get; set; }

		public QuestionScaleCheckedEventArgs(RadioGroup.CheckedChangeEventArgs e) : base(e.CheckedId) { }
	}

}
