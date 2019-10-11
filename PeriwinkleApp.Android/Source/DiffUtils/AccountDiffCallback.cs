using System.Collections.Generic;
using Android.Support.V7.Util;
using PeriwinkleApp.Android.Source.AdapterModels;

namespace PeriwinkleApp.Android.Source.DiffUtils
{
    public class AccountDiffCallback : DiffUtil.Callback
    {
        private List <AccountAdapterModel> oldList;
        private List <AccountAdapterModel> newList;

        public AccountDiffCallback (List <AccountAdapterModel> oldList, List <AccountAdapterModel> newList)
        {
            this.oldList = oldList;
            this.newList = newList;
        }

        public override bool AreContentsTheSame (int oldItemPosition, int newItemPosition)
        {
            return oldList[oldItemPosition].Equals (newList[newItemPosition]);
        }

        public override bool AreItemsTheSame (int oldItemPosition, int newItemPosition)
        {
            return oldList[oldItemPosition] == newList[newItemPosition];
        }

        public override int NewListSize => newList?.Count ?? 0;

        public override int OldListSize => oldList?.Count ?? 0;
    }
}
