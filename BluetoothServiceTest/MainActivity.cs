using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;
using System;
using Android.Bluetooth;
using Android.Content;
using System.Collections.Generic;

namespace BluetoothServiceTest
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
		Button Connect, Write;
		TextView Text;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);


			Connect = FindViewById<Button>(Resource.Id.connect);
			Write = FindViewById<Button>(Resource.Id.write);
			Text = FindViewById<Button>(Resource.Id.text);

			Connect.Click += OnConnectClicked;
			Write.Click += OnWriteClicked;
		}

		public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
		{
			Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

			base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
		}

		private void OnConnectClicked(object sender, EventArgs e)
		{
			Text.Text = "Connecting to Bluetooth";
			TurnOnBluetooth();
		}

		private void OnWriteClicked(object sender, EventArgs e)
		{
			throw new NotImplementedException();
		}


		#region MINE
		BluetoothAdapter btAdapter;
		BluetoothDevice btDevice;

		const int REQUEST_ENABLE_BT = 3;

		private void ConnectToBluetoothDevice()
		{
			ICollection<BluetoothDevice> pairedDevices = btAdapter.BondedDevices;

			if (pairedDevices.Count > 0)
			{
				foreach (BluetoothDevice device in pairedDevices)
				{
					string name = device.Name;
					string address = device.Address;
					if (name == "AKO SI BLUETOOTH")
					{
						Console.WriteLine($"Bluetooth Device: Name: {name}\nAddress: {address}");
						btDevice = device;
						break;
					}
				}
			}

			// TODO AUTO ON NARIN UNG BLUETOOTH BAGO GAWIN TO PARA DIRE-DIRETSO
			//handler = new BluetoothHandler(this);
			//btService = new BluetoothService(this, handler);
			//btService.BtUUID = btDevice.GetUuids()[0].Uuid;

			// connect na
			//btService.StartClient(btDevice);
		}

		private void TurnOnBluetooth()
		{
			btAdapter = BluetoothAdapter.DefaultAdapter;
			if (btAdapter == null)
			{
				// message tayo dito device doesn't support bluetooth
				Console.WriteLine("Device doesn't support Bluetooth");
				return;
			}

			// not enabled, so we turn it on
			if (!btAdapter.IsEnabled)
			{
				Intent enableIntent = new Intent(BluetoothAdapter.ActionRequestEnable);
				StartActivityForResult(enableIntent, REQUEST_ENABLE_BT);
			}
			else
			{
				ConnectToBluetoothDevice();
			}
		}
		
		protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
		{
			if (requestCode == REQUEST_ENABLE_BT)
			{
				Console.WriteLine("BLUETOOTH RESULT CODE : " + resultCode);
				if (resultCode == Result.Ok)
					ConnectToBluetoothDevice();
				else
					OnBackPressed();
			}
		}

		#endregion

	}
}