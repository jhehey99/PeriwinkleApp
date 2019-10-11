using System;
using System.Collections.Generic;
using PeriwinkleApp.Android.Source.Adapters;

namespace PeriwinkleApp.Android.Source.Factories
{
    public class AccountAdapterFactory
    {
        private static readonly IDictionary <string, AccountRecyclerAdapter> AdapterDictionary;

        static AccountAdapterFactory()
        {
            AdapterDictionary = new Dictionary <string, AccountRecyclerAdapter> ();
        }

        public static AccountRecyclerAdapter CreateAccountRecyclerAdapter (EventHandler<int> itemClick, string key = null)
        {
            // di pwede null ung key
            if (key == null)
                return null;

            bool containsKey = AdapterDictionary.ContainsKey(key);

            // key exists
            if (containsKey)
                return AdapterDictionary[key];

            // key doesn't exists, we create a new adapter and add it to the dictionary
            AccountRecyclerAdapter adapter = new AccountRecyclerAdapter ();
            adapter.ItemClick += itemClick;
            AdapterDictionary.Add (key, adapter);
            return adapter;
        }
        
        public static AccountRecyclerAdapter ReadAccountRecyclerAdapter (string key = null)
        {
            // di pwede null ung key, or wala sa dictionary
            if (key == null || !AdapterDictionary.ContainsKey(key))
                return null;
            
            return AdapterDictionary[key];
        }
    }
}
