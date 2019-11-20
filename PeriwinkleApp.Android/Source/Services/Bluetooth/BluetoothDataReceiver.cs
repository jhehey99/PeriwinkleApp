using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using PeriwinkleApp.Core.Sources.Utils;

namespace PeriwinkleApp.Android.Source.Services.Bluetooth
{
    [BroadcastReceiver(Enabled = true)]
    [IntentFilter(new[] { "BluetoothDataFilter" })]
    public class BluetoothDataReceiver : BroadcastReceiver
    {
        public override void OnReceive(Context context, Intent intent)
        {
            string message = intent.GetStringExtra("BluetoothData");
            string[] vals = message.Split(
                                           new[] { "\r\n", "\r", "\n" },
                                           StringSplitOptions.None
                                           );

            foreach(string val in vals)
            {
                Logger.Log($"Value = {val}");
            }
        }
    }
}