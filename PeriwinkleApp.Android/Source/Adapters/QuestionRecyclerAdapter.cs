using System.Collections.Generic;
using System.Linq;
using Android.Support.V7.Widget;
using Android.Views;
using PeriwinkleApp.Android.Source.AdapterModels;
using PeriwinkleApp.Android.Source.ViewHolders;

namespace PeriwinkleApp.Android.Source.Adapters
{
    public class QuestionRecyclerAdapter : BaseRecyclerAdapter <QuestionAdapterModel>
    {
		public QuestionRecyclerAdapter () : base (null) { }

        public QuestionRecyclerAdapter (List <QuestionAdapterModel> dataset = null) : base (dataset) { }

        public override void OnBindViewHolder (RecyclerView.ViewHolder holder, int position)
        {
            CardQuestionViewHolder viewHolder = (CardQuestionViewHolder) holder;

            // Set the CardView's Elements
            viewHolder.TextId.Text = $"Question {DataSet[position].Id}";
            viewHolder.TextQuestion.Text = DataSet[position].Question;
            viewHolder.QuestionScaleSelected += QuestionScaleSelected;
        }

        private void QuestionScaleSelected(object sender, QuestionScaleSelectedEventArgs e)
        {
            // SpinnerPosition is LayoutPosition
            // Position is Selected Spinner Value
            DataSet[e.SpinnerPosition].Scale = e.Position;
//            Logger.Log ($"Selected Question {e.SpinnerPosition + 1} : Scale = {e.Position}");
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder (ViewGroup parent, int viewType)
        {
            View itemView = LayoutInflater.From (parent.Context)
                                          .Inflate (Resource.Layout.card_view_question, parent, false);

            CardQuestionViewHolder viewHolder = new CardQuestionViewHolder (itemView, OnClick);
            return viewHolder;
        }

        public List <int> GetScaleValueList ()
        {
            // QuestionAdapterModel.Scale
            return DataSet.Select (qAdapterModel => qAdapterModel.Scale).ToList ();
        }
    }
}
