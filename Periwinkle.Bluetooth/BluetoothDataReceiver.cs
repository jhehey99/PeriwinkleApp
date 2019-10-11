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
using Java.Lang;
using Periwinkle.Utilities;
using Periwinkle.Views.Activities;

namespace Periwinkle.Services.Bluetooth
{
	[BroadcastReceiver(Enabled = true)]
	[IntentFilter(new[] { "BluetoothDataFilter" })]
	public class BluetoothDataReceiver : BroadcastReceiver
	{
		public override void OnReceive(Context context, Intent intent)
		{
			//Logger.Log ("OnReceive");

			string message = intent.GetStringExtra("BluetoothData");

			//            string [] vals = message.Split (';');

			string[] vals = message.Split(

										  new[] { "\r\n", "\r", "\n" },
										  StringSplitOptions.None
										 );

			foreach (string val in vals)
			{
				if (int.TryParse(val, out int value))
				{
					Logger.Log("val = " + val);

					//Logger.Log (value.ToString());
					float voltage = (float)value / 1024.0f * 5.0f;
					if (BehaviorActivity.Instance != null)
					{
						BehaviorActivity.Instance.ReceiveBluetoothData(voltage);
						//                        Thread.Sleep (50);
					}
				}
			}

			//            Logger.Log (message);
			//((BehaviorActivity) context).ReceiveBluetoothData (message);
		}
	}
}