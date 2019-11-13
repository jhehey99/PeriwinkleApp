using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Bluetooth;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using PeriwinkleApp.Android.Source.Services.Bluetooth;
using PeriwinkleApp.Core.Sources.Extensions;
using PeriwinkleApp.Core.Sources.Utils;

namespace PeriwinkleApp.Android.Source.Views.Activities
{
	// [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true)]
	[Activity(Label = "@string/app_name", MainLauncher = false)]
	public class ClientCalibrateActivity : Activity, IMessageReceiver
	{
		private Button btnCalibrate, btnStart;

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			// Create your application here
			SetContentView(Resource.Layout.client_frag_calibrate);

			btnCalibrate = FindViewById<Button>(Resource.Id.btn_calibrate);
			btnCalibrate.Click += onCalibrateClicked;

			btnStart = FindViewById<Button>(Resource.Id.btn_start);
			btnStart.Click += onStartClicked;
		}

		protected override void OnStart()
		{
			base.OnStart();
			handler = new BluetoothHandler(this);
			btService = new BluetoothService(this, handler);

			TurnOnBluetooth();
		}

		protected override void OnDestroy()
		{
			base.OnDestroy();

		}

		#region Calibration
		enum CalibrationState { None, Calibrating, Succeeded, Failed }
		private CalibrationState state = CalibrationState.None;
		private CalibrationState State
		{
			get => state;
			set
			{
				state = value;
				applyStateDisplay(state);
			}
		}

		private void applyStateDisplay(CalibrationState s)
		{
			//TODO: Test kung gumagana tong calibration na to
			switch (s)
			{
				case CalibrationState.None:
					btnCalibrate.SetBackgroundColor(Color.Blue);
					btnCalibrate.Text = "Calibrate";
					break;

				case CalibrationState.Calibrating:
					btnCalibrate.SetBackgroundColor(Color.LightYellow);
					btnCalibrate.Text = "Please wait while calibrating";
					break;
				case CalibrationState.Succeeded:
					btnCalibrate.SetBackgroundColor(Color.Green);
					btnCalibrate.Text = "Calibration Succeeded";
					break;
				case CalibrationState.Failed:
					btnCalibrate.SetBackgroundColor(Color.Red);
					btnCalibrate.Text = "Calibration Failed";
					break;
			}
		}

		private void onCalibrateClicked(object sender, EventArgs e)
		{
			Logger.Log("onCalibrateClicked");

			// TEST Write data to bluetooth
			btService.Write("onCalibrateClicked".ToBytesArray());

			// do calibration
			// get calibration results
			// apply state
			// pag success or failed, wait 3 seconds
			// then apply state to idle ulet

		}

		private void onStartClicked(object sender, EventArgs e)
		{
			Logger.Log("onStartClicked");
			btService.Start();
			btService.StartClient(btDevice);
		}

		#endregion Calibration

		#region Bluetooth
		BluetoothAdapter btAdapter;
		BluetoothService btService;
		BluetoothHandler handler;
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
					Logger.Log($"Name: {name}\nAddress: {address}");
					if (name == "AKO SI BLUETOOTH")
					{
						btDevice = device;
						btService.BtUUID = btDevice.GetUuids()[0].Uuid;
						break;
					}
				}
			}
			/*
			handler = new BluetoothHandler(this);
			btService = new BluetoothService(this, handler);
			btService.BtUUID = btDevice.GetUuids()[0].Uuid;
			*/
			// connect na
			//TODO: StartClient pag mag rerecord na, SEE ClientBehaviorActivity
			//			btService.Start();
			//			btService.StartClient(btDevice);
		}

		private void TurnOnBluetooth()
		{
			btAdapter = BluetoothAdapter.DefaultAdapter;
			if (btAdapter == null)
			{
				// message tayo dito device doesn't support bluetooth
				Logger.Log("Device doesn't support Bluetooth");
				return;
			}

			// not enabled, so we turn it on
			if (!btAdapter.IsEnabled)
			{
				Intent enableIntent = new Intent(BluetoothAdapter.ActionRequestEnable);
				base.StartActivityForResult(enableIntent, REQUEST_ENABLE_BT);
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
				Logger.Log("BLUETOOTH RESULT CODE : " + resultCode);
				if (resultCode == Result.Ok)
					ConnectToBluetoothDevice();
				else
					OnBackPressed();
			}
		}

		public void ReceiveMessage(string message)
		{

			RunOnUiThread(() =>
			{
				Console.WriteLine(message);
			});

			/*

			if (int.TryParse(message, out int val))
			{
				RunOnUiThread(() => { presenter.AddEntry(val); });
			}*/
		}
		#endregion
	}
}