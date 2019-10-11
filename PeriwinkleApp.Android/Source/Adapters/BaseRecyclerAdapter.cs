using System;
using System.Collections.Generic;
using Android.Support.V7.Util;
using Android.Support.V7.Widget;
using PeriwinkleApp.Android.Source.DiffUtils;

namespace PeriwinkleApp.Android.Source.Adapters
{
    public abstract class BaseRecyclerAdapter<TAdapterModel> : RecyclerView.Adapter
    {
        public event EventHandler<int> ItemClick;

        protected List<TAdapterModel> DataSet;
        
        public override int ItemCount => DataSet.Count;

        protected BaseRecyclerAdapter(List<TAdapterModel> dataset = null)
        {
            DataSet = dataset ?? new List<TAdapterModel>();
        }

        protected void OnClick(int position)
        {
            ItemClick?.Invoke(this, position);
        }

        public virtual void AddItem(TAdapterModel item)
        {
            int addIndex = DataSet.Count;
            DataSet.Add(item);
            NotifyItemInserted(addIndex);
        }
       
        public virtual void RemoveItemAt(int position)
        {
            DataSet.RemoveAt(position);
            NotifyItemRemoved(position);
        }

        public virtual void ReplaceDataSet(List<TAdapterModel> newDataSet)
        {
            DataSet = new List<TAdapterModel>(newDataSet);
            NotifyDataSetChanged();
        }

		public virtual void UpdateList (List <TAdapterModel> newDataset)
		{
			DiffCallback<TAdapterModel> diffCallback = new DiffCallback<TAdapterModel>(DataSet, newDataset);
			DiffUtil.DiffResult diffResult = DiffUtil.CalculateDiff(diffCallback);
			diffResult.DispatchUpdatesTo(this);
			
			DataSet.Clear();
			DataSet.AddRange(newDataset);
        }

    }
}