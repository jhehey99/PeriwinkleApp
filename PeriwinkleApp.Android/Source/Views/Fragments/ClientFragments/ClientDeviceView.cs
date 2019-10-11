using System;
using Android.Bluetooth;
using Android.Content;
using Android.OS;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;
using PeriwinkleApp.Core.Sources.Utils;
using System.Collections.Generic;
using PeriwinkleApp.Android.Source.Services.Bluetooth;

namespace PeriwinkleApp.Android.Source.Views.Fragments.ClientFragments
{
    public class ClientDeviceView : Fragment, IMessageReceiver
	{

		private Switch btSwitch;
		private Button btnDiscover;
		private Button btnConnect;
		private BluetoothAdapter btAdapter;

		const int REQUEST_ENABLE_BT = 3;

        BluetoothService btService;
        BluetoothHandler handler;
        BluetoothDevice btDevice;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            return inflater.Inflate(Resource.Layout.client_frag_device, container, false);
        }

		public override void OnViewCreated (View view, Bundle savedInstanceState)
		{
			base.OnViewCreated (view, savedInstanceState);

			// Bluetooth On/Off Switch
			btSwitch = view.FindViewById <Switch> (Resource.Id.swt_cli_dev_bt);
			btSwitch.CheckedChange += OnBluetoothSwitchChanged;

            // Bluetooth Connect Button
            btnConnect = view.FindViewById<Button>(Resource.Id.btn_cli_dev_connect);
            btnConnect.Click += OnBluetoothConnectClicked;

            // Bluetooth Discover Devices Button
            //btnDiscover = view.FindViewById <Button> (Resource.Id.btn_cli_dev_discover);
            //btnDiscover.Click += OnBluetoothDiscoverClicked;

            // Get default bluetooth adapter
            btAdapter = BluetoothAdapter.DefaultAdapter;

            ICollection<BluetoothDevice> pairedDevices = btAdapter.BondedDevices;

            if(pairedDevices.Count > 0)
            {
                foreach(BluetoothDevice device in pairedDevices)
                {
                    string name = device.Name;
                    string address = device.Address;
                    Logger.Log($"Name: {name}\nAddress: {address}");
                    if (name == "AKO SI BLUETOOTH")
                        btDevice = device;
                }
            }

            handler = new BluetoothHandler(this);
            btService = new BluetoothService(this.Context, handler);

		}

		private void OnBluetoothSwitchChanged (object sender, CompoundButton.CheckedChangeEventArgs e)
		{
			if (e.IsChecked)
				TurnOnBluetooth ();
			else
				TurnOffBluetooth ();
		}

		private void OnBluetoothConnectClicked(object sender, EventArgs e)
		{
            Logger.Log("Conect to bluetooth");
            Logger.Log("Device: " + btDevice.Name);
            btService.BtUUID = btDevice.GetUuids()[0].Uuid;
            btService.StartClient(btDevice);
		}

		private void TurnOnBluetooth ()
		{
			btSwitch.Text = "Turn Bluetooth Off";
			if (btAdapter == null)
			{
				// message tayo dito device doesn't support bluetooth
				Logger.Log ("Device doesn't support Bluetooth");
				return;
			}
			
			// not enabled, so we turn it on
			if (!btAdapter.IsEnabled)
			{
				Intent enableIntent = new Intent (BluetoothAdapter.ActionRequestEnable);
				StartActivityForResult (enableIntent, REQUEST_ENABLE_BT);
			}
        }

        private void TurnOffBluetooth ()
		{
			btSwitch.Text = "Turn Bluetooth On";

			if (btAdapter == null)
			{
				// message tayo dito device doesn't support bluetooth
				Logger.Log("Device doesn't support Bluetooth");
				return;
			}

			// enabled, so we turn it off
			if (btAdapter.IsEnabled)
			{
				btAdapter.Disable ();
				Intent enableIntent = new Intent(BluetoothAdapter.ActionRequestEnable);
				StartActivityForResult(enableIntent, REQUEST_ENABLE_BT);
			}
        }

        public void ReceiveMessage(string message)
        {
            Logger.Log(message);
        }
    }
}
