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
        public Spinner SpinScale { get; private set; }

        public CardQuestionViewHolder (View itemView, Action <int> listener) : base (itemView)
        {
            TextId = itemView.FindViewById<TextView>(Resource.Id.card_view_qtn_id);
            TextQuestion = itemView.FindViewById<TextView>(Resource.Id.card_view_qtn_qtn);
            SpinScale = itemView.FindViewById<Spinner>(Resource.Id.spin_scale);
            SpinScale.ItemSelected += SpinnerItemSelected;
            itemView.Click += (sender, e) => listener(base.LayoutPosition);
        }

        public EventHandler <QuestionScaleSelectedEventArgs> QuestionScaleSelected;

        private void SpinnerItemSelected (object sender, AdapterView.ItemSelectedEventArgs e)
        {
            // pag pumili nung scale value, i-ca-call to
            QuestionScaleSelectedEventArgs h = new QuestionScaleSelectedEventArgs(e) {SpinnerPosition = LayoutPosition};
            QuestionScaleSelected(sender, h);
        }
    }

    public class QuestionScaleSelectedEventArgs : AdapterView.ItemSelectedEventArgs
    {
        public int SpinnerPosition { get; set; }

        public QuestionScaleSelectedEventArgs(AdapterView.ItemSelectedEventArgs e) : base(e.Parent, e.View, e.Position, e.Id) { }
    }
}
