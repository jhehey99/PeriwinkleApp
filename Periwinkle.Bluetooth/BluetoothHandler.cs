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
using Periwinkle.Utilities;
using Periwinkle.Views.Activities;

namespace Periwinkle.Services.Bluetooth
{
	public class BluetoothHandler : Handler
	{
		private DeviceActivity activity;

		public BluetoothHandler(DeviceActivity activity)
		{
			this.activity = activity;
		}

		public override void HandleMessage(Message msg)
		{
			switch (msg.What)
			{
				// write
				case 3:
					var writeBuffer = (byte[])msg.Obj;
					var writeMessage = Encoding.ASCII.GetString(writeBuffer);
					//activity.Adapter.Add ($"Me:  {writeMessage}");

					break;

				// read
				case 2:
					//Logger.Log ("HANDLE MESSAGE CASE 2");
					var readBuffer = (byte[])msg.Obj;
					var readMessage = Encoding.ASCII.GetString(readBuffer);
					activity.EtoUngMessage(readMessage);

					//activity.Adapter.Add ($"{activity.connectedDeviceName}: {readMessage}");
					break;
			}

		}
	}
}