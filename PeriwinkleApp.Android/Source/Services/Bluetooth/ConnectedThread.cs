using System;
using System.IO;
using System.Text;
using Android.Bluetooth;
using Android.OS;
using Java.Lang;
using PeriwinkleApp.Core.Sources.Utils;
using Exception = Java.Lang.Exception;
using IOException = Java.IO.IOException;

namespace PeriwinkleApp.Android.Source.Services.Bluetooth
{
	public class ConnectedThread : Thread
	{
		private readonly BluetoothService service;
        private BluetoothSocket socket;
		private Stream InStream;
		private Stream OutStream;
		private byte[] buffer;
		private BluetoothHandler handler;
		public EventHandler OnSocketClosed { get; set; }

        public ConnectedThread (BluetoothService service, BluetoothSocket socket)
		{
			this.service = service;
			this.socket = socket;
			Stream tempIn = null;
			Stream tempOut = null;
			this.handler = this.service.MessageHandler;

            try
            {
				tempIn = this.socket.InputStream;
			}
			catch (IOException e)
			{
				Logger.Log("Error occurred when creating input stream " + e.Message);
            }

			try
			{
				tempOut = this.socket.OutputStream;
			}
			catch (IOException e)
			{
				Logger.Log("Error occurred when creating output  stream " + e.Message);
			}

            InStream = tempIn;
			OutStream = tempOut;
		}

		public override void Run ()
		{
			while (true)
			{
				try
				{
					byte b = 0;
					string s = "";
					do
					{
						b = (byte) InStream.ReadByte();

						if (b == 13 || b == 10)
						{
							InStream.Flush();
							break;
						}
						s += Encoding.ASCII.GetString(new byte[] { b });
					} while (b >= 0);
					//Console.WriteLine("STRING: " + s);
					
					if(s != "")
					{
						Message readMessage = handler.ObtainMessage ((int) MessageConstants.Read, s.Length, -1, s);
						readMessage.SendToTarget ();
					}
				}
				catch (IOException e)
				{
					Logger.Log("Input stream was disconnected " + e.Message);
					//OnSocketClosed(this, new EventArgs());
					break;
				}
            }
		}

		public void Write (byte[] bytesToWrite)
		{
			try
			{
				OutStream.Write (bytesToWrite, 0, bytesToWrite.Length);
				Message writtenMessage = handler.ObtainMessage ((int) MessageConstants.Write, -1, -1, bytesToWrite);
				writtenMessage.SendToTarget ();

			}
			catch (IOException e)
			{
				Logger.Log("Error occurred when sending data " + e.Message);

                Message writeErrorMessage = handler.ObtainMessage ((int) MessageConstants.Toast);
				Bundle bundle = new Bundle();
				bundle.PutString ("toast", "Couldn't send data to the other device");
				writeErrorMessage.Data = bundle;
				handler.SendMessage (writeErrorMessage);
			}
        }
		
		public void Cancel ()
		{
			try
			{
				InStream.Close();
				OutStream.Close();
				socket.Close ();
				Logger.Log("ConnectedThread - Cancel");
			}
			catch (IOException e)
			{
				Logger.Log("Could not close the connect socket " + e.Message);
            }
        }
    }
}
