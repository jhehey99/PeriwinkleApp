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
    public interface IMessageReceiver
    {
        void ReceiveMessage(string message);
    }

    public class BluetoothHandler : Handler
    {
        private IMessageReceiver receiver;
        
        public BluetoothHandler(IMessageReceiver receiver)
        {
            this.receiver = receiver;
        }

        public override void HandleMessage(Message msg)
        {
            //Logger.Log("Handle Message: " + msg.What);

            switch (msg.What)
            {
                // read
                case (int) MessageConstants.Read:
					//Logger.Log ("HANDLE MESSAGE CASE 2");
					string message = (string) msg.Obj;
					receiver.ReceiveMessage(message);
					break;
				//TODO: Write
            }
        }

    }
}