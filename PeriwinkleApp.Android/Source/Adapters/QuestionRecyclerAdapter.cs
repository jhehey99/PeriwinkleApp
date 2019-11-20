using System;
using System.Collections.Generic;
using System.Linq;
using Android.Support.V7.Widget;
using Android.Views;
using PeriwinkleApp.Android.Source.AdapterModels;
using PeriwinkleApp.Android.Source.ViewHolders;
using PeriwinkleApp.Core.Sources.Utils;

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
			viewHolder.QuestionScaleChecked += QuestionScaleChecked;
        }

		private void QuestionScaleChecked(object sender, QuestionScaleCheckedEventArgs e)
		{
			DataSet[e.ScalePosition].Scale = e.ScaleValue;
			Logger.Log($"Selected Question {e.ScalePosition + 1} : Scale = {e.ScaleValue}");
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
            return DataSet.Select (qAdapterModel => qAdapterModel.Scale).ToList ();
        }
    }
}
