using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

using Android.App;
using Android.Bluetooth;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Java.IO;
using Java.Lang;
using Java.Util;
using Newtonsoft.Json.Serialization;
using Periwinkle.Utilities;
using Console = System.Console;
using Exception = Java.Lang.Exception;
using IOException = Java.IO.IOException;

namespace Periwinkle.Services.Bluetooth
{
    public class BluetoothConnectionService
    {
        public static string appName = "Periwinkle";

        //public static UUID MY_UUID_INSECURE { get; set; }


        public static UUID MY_UUID_INSECURE =
            UUID.FromString("038c866f-e381-4f24-bb67-32f45580a3c6");

        public BluetoothAdapter bluetoothAdapter;
        private Context context;
        private BluetoothHandler handler;

        private AcceptThread insecureAcceptThread;
        private ConnectThread connectThread;
        private ConnectedThread connectedThread;

        private BluetoothDevice device;
        private UUID deviceUUID;

        public static bool IsConnected;

        public BluetoothConnectionService(Context context, BluetoothHandler handler)
        {
            this.context = context;
            this.handler = handler;
            bluetoothAdapter = BluetoothAdapter.DefaultAdapter;
            Start();
        }

        public void Connected(BluetoothSocket socket)
        {
            Logger.Log("Connected: Starting");
            connectedThread = new ConnectedThread(this, socket);
            connectedThread.Start();

            //var msg = handler.ObtainMessage(device_name);
            //Bundle bundle = new Bundle();
            //bundle.PutString(device_name, device.Name);
            //msg.Data = bundle;
            //handler.SendMessage(msg);

        }

        // ACCEPT THREAD
        private class AcceptThread : Thread
        {
            private BluetoothServerSocket serverSocket;
            private BluetoothConnectionService service;

            public AcceptThread(BluetoothConnectionService service)
            {
                this.service = service;
                BluetoothServerSocket tmp = null;

                // create a listening server socket
                try
                {
                    tmp = service.bluetoothAdapter.ListenUsingInsecureRfcommWithServiceRecord(appName, MY_UUID_INSECURE);
                    Logger.Log("AcceptThread: Setting up Service using " + MY_UUID_INSECURE);
                }
                catch (IOException e)
                {
                    Logger.Log("AcceptThread: " + e.Message);
                }

                serverSocket = tmp;
            }

            public override void Run()
            {
                Logger.Log("Run: AcceptThread Running");

                BluetoothSocket socket = null;

                try
                {
                    Logger.Log("Run: RFCOM server socket start.....");

                    socket = serverSocket.Accept();

                    Logger.Log("Run: RFCOM server socket Accepted Connection");
                }
                catch (IOException e)
                {
                    Logger.Log("Run: " + e.Message);
                }

                if (socket != null)
                {
                    service.Connected(socket);
                }

                Logger.Log("END AcceptThread");
            }

            public void Cancel()
            {
                Logger.Log("Cancel: Cancelling AcceptThread");
                try
                {
                    serverSocket.Close();
                }
                catch (IOException e)
                {
                    Logger.Log("Cancel: " + e.Message);
                }
            }
        }

        // CONNECT THREAD
        private class ConnectThread : Thread
        {
            private BluetoothConnectionService service;
            private BluetoothSocket socket;

            public ConnectThread(BluetoothConnectionService service, BluetoothDevice dev, UUID uuid)
            {
                this.service = service;
                service.device = dev;
                service.deviceUUID = uuid;
            }

            public override void Run()
            {
                BluetoothSocket tmp = null;

                Logger.Log("Run: ConnectThread");

                // Get a BluetoothSocket for a connection with the
                // given BluetoothDevice

                try
                {
                    Logger.Log("ConnectThread: Trying to create InsecureRfcommSocket using UUID: " + MY_UUID_INSECURE);

                    tmp = service.device.CreateInsecureRfcommSocketToServiceRecord(service.deviceUUID);

                }
                catch (IOException e)
                {
                    Logger.Log("ConnectThread: Could not create InsecureRfcommSocket " + e.Message);
                }

                if (tmp == null)
                    return;

                socket = tmp;

                // first, always cancel discovery, very slow
                service.bluetoothAdapter.CancelDiscovery();

                // blocking call, will return on successful connection
                try
                {
                    socket.Connect();
                    Logger.Log("ConnectThread: Connected");
                    IsConnected = true;
                }
                catch (Exception e)
                {
                    try
                    {
                        socket.Close();
                        Logger.Log("Run: Closed Socket");
                    }
                    catch (IOException ex)
                    {
                        Logger.Log("ConnectThread: Unable to close connection in socket " + ex.Message);
                    }
                    Logger.Log("Run: ConnectThread: Could not connect to UUID: " + MY_UUID_INSECURE + e.Message);
                }

                service.Connected(socket);
            }

            public void Cancel()
            {
                try
                {
                    IsConnected = false;
                    socket.Close();
                }
                catch (IOException e)
                {
                    Logger.Log("Cancel: close() failed " + e.Message);
                }
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void Start()
        {
            if (connectThread != null)
            {
                connectThread.Cancel();
                connectThread = null;
            }
            if (insecureAcceptThread == null)
            {
                insecureAcceptThread = new AcceptThread(this);
                insecureAcceptThread.Start();
            }
        }

        public void StartClient(BluetoothDevice dev, UUID uuid)
        {
            connectThread = new ConnectThread(this, dev, uuid);
            connectThread.Start();
        }

        private class ConnectedThread : Thread
        {
            private BluetoothConnectionService service;
            private BluetoothSocket socket;
            private Stream inStream;
            private Stream outStream;

            public ConnectedThread(BluetoothConnectionService service, BluetoothSocket socket)
            {
                this.service = service;
                this.socket = socket;

                Stream tmpIn = null;
                Stream tmpOut = null;

                try
                {
                    tmpIn = socket.InputStream;
                    tmpOut = socket.OutputStream;
                }
                catch (IOException e)
                {
                    Logger.Log(e.Message);
                    IsConnected = false;
                }

                inStream = tmpIn;
                outStream = tmpOut;

            }

            public override void Run()
            {
                Logger.Log("ConnectedThread: Run");

                byte[] buffer = new byte[16];

                int bytes;

                Logger.Log("IsConnected = " + IsConnected.ToString());

                while (IsConnected)
                {
                    try
                    {
                        //Logger.Log("READ READ READ HAHAHAHA1");
                        bytes = inStream.Read(buffer, 0, buffer.Length);// (buffer, 0, buffer.Length);


                        //inStream.FlushAsync();

                        //                    bytes = inStream.Read (buffer);
                        //string readMessage = new string(buffer, 0, bytes);
                        //                    Logger.Log ("READ READ READ HAHAHAHA2");
                        //                    string message = Encoding.ASCII.GetString (buffer, 0, bytes);

                        service.handler
                               .ObtainMessage(2, bytes, -1, buffer)
                               .SendToTarget();

                        //Logger.Log(message);


                        // Send the obtained bytes to the UI Activity
                        //                    service.handler
                        //                           .ObtainMessage(2, bytes, -1, buffer)
                        //                           .SendToTarget();
                    }
                    catch (IOException e)
                    {
                        Logger.Log(e.Message);
                        IsConnected = false;
                    }
                }
            }

            public void Write(byte[] buffer)
            {
                try
                {
                    outStream.Write(buffer, 0, buffer.Length);

                    // Share the sent message back to the UI Activity
                    //                    service.handler
                    //                           .ObtainMessage(3, -1, -1, buffer)
                    //                           .SendToTarget();
                }
                catch (Java.IO.IOException e)
                {
                    Logger.Log(e.Message);
                }
            }

            public void Cancel()
            {
                try
                {
                    socket.Close();
                }
                catch (IOException e)
                {
                    Logger.Log(e.Message);
                }
            }
        }

        public void Write(byte[] buffer)
        {
            //ConnectedThread cThread;
            connectedThread.Write(buffer);
        }
    }
}