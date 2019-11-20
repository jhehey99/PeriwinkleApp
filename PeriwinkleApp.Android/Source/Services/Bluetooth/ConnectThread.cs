using System;
using Android.Bluetooth;
using Java.IO;
using Java.Lang;
using Java.Util;
using PeriwinkleApp.Core.Sources.Utils;
using Console = System.Console;
using Exception = Java.Lang.Exception;

namespace PeriwinkleApp.Android.Source.Services.Bluetooth
{
	public class ConnectThread : Thread
	{
		private readonly BluetoothService service;
		private readonly BluetoothSocket socket;
		private BluetoothAdapter BtAdapter;
        private BluetoothDevice device;


        public ConnectThread (BluetoothService service, BluetoothDevice device)
		{
			this.service = service;
			this.device = device;
			BtAdapter = this.service.BtAdapter;

            BluetoothSocket tempSocket = null;
			try
			{
				tempSocket = device.CreateInsecureRfcommSocketToServiceRecord(UUID.FromString("00001101-0000-1000-8000-00805F9B34FB"));
				//tempSocket = device.CreateRfcommSocketToServiceRecord(service.BtUUID);
			}
			catch (IOException e)
			{
				Logger.Log("Socket's create() method failed " + e.Message);
			}

            socket = tempSocket;
		}

		public override void Run ()
		{
			Logger.Log("ConnectThread Run");
			BtAdapter.CancelDiscovery ();

			try
			{
				socket.Connect ();
			}
			catch (IOException e)
			{
				CloseSocket ();
				return;
			}

			service.ManageConnectedSocket (socket);
        }

		public void Cancel ()
		{
			Logger.Log("ConnectThread Cancel");
			CloseSocket();
		}

		private void CloseSocket ()
		{
			Logger.Log("ConnectThread CloseSocket");
			try
			{
				socket.Close();
			}
			catch (IOException e)
			{
				Logger.Log("Could not close the client socket " + e.Message);
			}
        }

    }
}
