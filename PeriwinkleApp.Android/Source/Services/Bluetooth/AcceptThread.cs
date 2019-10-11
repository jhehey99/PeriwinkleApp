using Android.Bluetooth;
using Java.IO;
using Java.Lang;
using PeriwinkleApp.Core.Sources.Utils;
using Exception = Java.Lang.Exception;

namespace PeriwinkleApp.Android.Source.Services.Bluetooth
{
	public class AcceptThread : Thread
    {
        private readonly BluetoothServerSocket serverSocket;
		private readonly BluetoothService service;

		public AcceptThread(BluetoothService service)
		{
			this.service = service;
			BluetoothServerSocket tempSocket = null;

			try
			{
				BluetoothAdapter adapter = this.service.BtAdapter;
				tempSocket = adapter.ListenUsingInsecureRfcommWithServiceRecord (service.BtName, service.MyUUID);
			}
			catch (IOException e)
			{
				Logger.Log ("Socket's listen() method failed " + e.Message);
			}

            serverSocket = tempSocket;
		}

		public override void Run ()
		{
			// ReSharper disable once TooWideLocalVariableScope
			BluetoothSocket socket = null;
            while (true)
			{
				try
				{
					socket = serverSocket.Accept ();
				}
				catch (IOException e)
				{
					Logger.Log("Socket's accept() method failed " + e.Message);
					break;
				}

				if (socket == null)
					continue;

				// connection accepted
				service.ManageConnectedSocket (socket);
				serverSocket.Close ();
				break;
			}
		}
		
		public void Cancel ()
		{
			try
			{
				serverSocket.Close ();
			}
			catch (Exception e)
			{
				Logger.Log("Could not close the connect socket " + e.Message);
			}
		}

    }
}
