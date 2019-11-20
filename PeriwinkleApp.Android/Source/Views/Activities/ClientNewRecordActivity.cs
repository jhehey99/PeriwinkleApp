using System;
using System.Collections.Generic;
using Android.App;
using Android.Bluetooth;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Widget;
using Microcharts;
using Microcharts.Droid;
using PeriwinkleApp.Android.Source.Presenters.ClientPresenters;
using PeriwinkleApp.Android.Source.Services.Bluetooth;
using PeriwinkleApp.Core.Sources.Utils;
using v7App = Android.Support.V7.App;

namespace PeriwinkleApp.Android.Source.Views.Activities
{
	public interface IClientNewRecordActivity
	{
		void SetStartTime(string startTime);
		void SetStopTime(string stopTime);
		void DisplayLineChart(LineChart linePiezo, LineChart lineAx, LineChart lineAy, LineChart lineAz);
	}

	[Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = false)]
	public class ClientNewRecordActivity : AppCompatActivity, IClientNewRecordActivity, IMessageReceiver
	{
		/* Bluetooth */
		const int REQUEST_ENABLE_BT = 3;
		private BluetoothAdapter btAdapter;
		private BluetoothService btService;
		private BluetoothHandler handler;
		private BluetoothDevice btDevice;

		private ChartView chartPiezo, chartAx, chartAy, chartAz;
		private LineChart linePiezo, lineAx, lineAy, lineAz;
		private IClientNewRecordPresenter presenter;

		private TextView txtStartTime, txtStopTime;
		private Button btnStart, btnStop;

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			SetContentView(Resource.Layout.client_frag_record_new);

			// set reference sa chartview
			chartPiezo = FindViewById<ChartView>(Resource.Id.chart_piezo);
			chartAx = FindViewById<ChartView>(Resource.Id.chart_ax);
			chartAy = FindViewById<ChartView>(Resource.Id.chart_ay);
			chartAz = FindViewById<ChartView>(Resource.Id.chart_az);

			txtStartTime = FindViewById<TextView>(Resource.Id.txt_start_time_val);
			txtStopTime = FindViewById<TextView>(Resource.Id.txt_stop_time_val);

			btnStart = FindViewById<Button>(Resource.Id.btn_start_time);
			btnStop = FindViewById<Button>(Resource.Id.btn_stop_time);

			btnStart.Click += OnStartTimeClicked;
			btnStop.Click += OnStopTimeClicked;

			// other initialize
			presenter = new ClientNewRecordPresenter(this);
		}

		protected override void OnStart()
		{
			base.OnStart();
			TurnOnBluetooth();
		}

		public void SetStartTime(string startTime)
		{
			txtStartTime.Text = startTime;
		}

		public void SetStopTime(string stopTime)
		{
			txtStopTime.Text = stopTime;
		}

		public void DisplayLineChart(LineChart linePiezo, LineChart lineAx, LineChart lineAy, LineChart lineAz)
		{
			//TODO linePiezo
			this.linePiezo = linePiezo;
			this.lineAx = lineAx;
			this.lineAy = lineAy;
			this.lineAz = lineAz;
			RunOnUiThread(UpdateLineChart);
		}

		private void UpdateLineChart()
		{
			//TODO linePiezo
			chartPiezo.Chart = linePiezo;
			chartAx.Chart = lineAx;
			chartAy.Chart = lineAy;
			chartAz.Chart = lineAz;
		}

		public void ReceiveMessage(string message)
		{
			if (message == "" || (message.Length > 0 && message[0] == ','))
				return;

			const int fieldCount = 4; // piezo, ax, ay, az
			string[] fields = message.Split(",");

			if(fields.Length == fieldCount)
			{
				Logger.Log(message);
				float.TryParse(fields[0], out float piezo);
				float.TryParse(fields[1], out float ax);
				float.TryParse(fields[2], out float ay);
				float.TryParse(fields[3], out float az);
				RunOnUiThread(() =>
				{
					presenter.AddEntry(piezo, ax, ay, az);
				});
			}
		}

		protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
		{
			base.OnActivityResult(requestCode, resultCode, data);
			Logger.Log($"New Record - Bluetooth Activity result | code: {REQUEST_ENABLE_BT} | request: {requestCode}");
			if (requestCode == REQUEST_ENABLE_BT && resultCode == Result.Ok)
			{
				ConnectToBluetoothDevice();
			}
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
				Logger.Log("New Record - Turning on bluetooth");
				Intent enableIntent = new Intent(BluetoothAdapter.ActionRequestEnable);
				StartActivityForResult(enableIntent, REQUEST_ENABLE_BT);
			}
			else
			{
				ConnectToBluetoothDevice();
			}
		}

		private void ConnectToBluetoothDevice()
		{
			Logger.Log("New Record - Connecting to bluetooth device");
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
						break;
					}
				}
			}

			// TODO AUTO ON NARIN UNG BLUETOOTH BAGO GAWIN TO PARA DIRE-DIRETSO
			handler = new BluetoothHandler(this);
			btService = new BluetoothService(this, handler);
			btService.BtUUID = btDevice.GetUuids()[0].Uuid;

			// connect na
			Logger.Log("Start Bluetooth Client");
			btService.StartClient(btDevice);
		}

		private void OnStartTimeClicked(object sender, EventArgs e)
		{
			if (presenter.IsEnabled)
				return;

			if (btDevice == null)
			{
				v7App.AlertDialog.Builder alert = new v7App.AlertDialog.Builder(this);
				alert.SetTitle("Bluetooth Device can't be found");
				alert.SetMessage("Please check if device is turned on");
				alert.SetNeutralButton("OK", delegate
				{
					alert.Dispose();
					OnBackPressed();
				});
				alert.Show();
			}
			else
			{
				//btService.Start(); - acceptThread to eh
				//btService.StartClient(btDevice);
				presenter.StartTimer();
			}
		}

		private void OnStopTimeClicked(object sender, EventArgs e)
		{
			if (!presenter.IsEnabled)
				return;

			presenter.StopTimer();
			btService.Stop();

			v7App.AlertDialog.Builder alert = new v7App.AlertDialog.Builder(this);
			alert.SetTitle("Behavior saved to your account");
			alert.SetMessage("You may view the details of your behavior");
			alert.SetNeutralButton("OK", delegate
			{
				alert.Dispose();
				OnBackPressed();
			});
			alert.Show();
		}
	}
}