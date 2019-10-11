using Android.Bluetooth;
using Android.Content;
using Android.OS;
using Java.Lang;
using Java.Util;
using PeriwinkleApp.Core.Sources.Utils;
using System;
using System.Runtime.CompilerServices;

namespace PeriwinkleApp.Android.Source.Services.Bluetooth
{
	public enum MessageConstants
	{
		Read,
		Write,
		Toast
	};

	public class BluetoothService
	{
		public BluetoothAdapter BtAdapter { get; private set; }
		public BluetoothHandler MessageHandler { get; private set; }
		//public Handler MessageHandler { get; private set; }

		public string BtName { get; } = "Periwinkle";
        public UUID MyUUID { get; } = UUID.FromString("038c866f-e381-4f24-bb67-32f45580a3c6");
        public UUID BtUUID { get; set; }
		public BluetoothDevice btDevice { get; set; }

		private Context context;

		private AcceptThread acceptThread;
		private ConnectThread connectThread;
		private ConnectedThread connectedThread;

		public BluetoothService (Context context, BluetoothHandler handler)
		{
			this.context = context;
			MessageHandler = handler;
			BtAdapter = BluetoothAdapter.DefaultAdapter;
		}

		public void ManageConnectedSocket (BluetoothSocket socket)
		{
			connectedThread = new ConnectedThread (this, socket);
			connectedThread.OnSocketClosed += OnSocketClosed;
            connectedThread.Start ();
		}

		private void OnSocketClosed(object sender, EventArgs e)
		{
			Logger.Log("OnSocketClosed");
			Stop();
			Start();
			StartClient(btDevice);
		}

		public void Start ()
		{
			if (connectThread != null)
			{
				connectThread.Cancel ();
				connectThread = null;
			}

			if (acceptThread == null)
			{
				acceptThread = new AcceptThread (this);
				acceptThread.Start ();
			}
		}

		public void StartClient (BluetoothDevice device)
		{
			connectThread = new ConnectThread (this, device);
			connectThread.Start ();
		}

        public void Stop()
        {
            if (connectThread != null)
            {
                connectThread.Cancel();
                connectThread = null;
            }

			if (acceptThread != null)
			{
				acceptThread.Cancel();
				acceptThread = null;
			}

            if (connectedThread != null)
            {
                connectedThread.Cancel();
                connectedThread = null;
            }
        }

		public void Write (byte[] bytesToWrite)
		{
			connectedThread.Write (bytesToWrite);
		}
	}
}
