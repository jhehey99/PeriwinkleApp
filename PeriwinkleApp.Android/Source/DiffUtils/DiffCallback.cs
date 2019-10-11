using System.Collections.Generic;
using Android.Support.V7.Util;

namespace PeriwinkleApp.Android.Source.DiffUtils
{
	public class DiffCallback <T> : DiffUtil.Callback
	{
		private readonly List <T> oldList;
		private readonly List<T> newList;

		public DiffCallback (List <T> oldList, List <T> newList)
		{
			this.oldList = oldList;
			this.newList = newList;
		}
		
		public override bool AreContentsTheSame (int oldItemPosition, int newItemPosition)
		{
			return oldList[oldItemPosition].Equals(newList[newItemPosition]);
        }

		public override bool AreItemsTheSame (int oldItemPosition, int newItemPosition)
		{
			return oldList[oldItemPosition].Equals (newList[newItemPosition]);
        }

		public override int NewListSize => newList?.Count ?? 0;

        public override int OldListSize => oldList?.Count ?? 0;
    }
}
