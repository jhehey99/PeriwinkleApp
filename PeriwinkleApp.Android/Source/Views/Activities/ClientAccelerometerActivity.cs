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
	public interface IClientAccelerometerActivity
	{
		void DisplayLineChart(LineChart lineAx, LineChart lineAy, LineChart lineAz);
		void SetStartTime(string startTime);
		void SetStopTime(string stopTime);
	}


	[Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = false)]
	public class ClientAccelerometerActivity : AppCompatActivity, IClientAccelerometerActivity, IMessageReceiver
	{
		private TextView txtStartTimeVal, txtStopTimeVal;
		private Button btnStartTime, btnStopTime;

		BluetoothAdapter btAdapter;
		BluetoothService btService;
		BluetoothHandler handler;
		BluetoothDevice btDevice;

		private ChartView chartAx, chartAy, chartAz;

		private IClientAccelerometerPresenter presenter;
		const int REQUEST_ENABLE_BT = 3;

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			//RequestedOrientation = ScreenOrientation.Landscape;
			SetContentView(Resource.Layout.client_frag_accel);

			// set reference sa chartview
			chartAx = FindViewById<ChartView>(Resource.Id.chart_ax);
			chartAy = FindViewById<ChartView>(Resource.Id.chart_ay);
			chartAz = FindViewById<ChartView>(Resource.Id.chart_az);

			// time text views
			txtStartTimeVal = FindViewById<TextView>(Resource.Id.txt_bhv_start_time_val);
			txtStopTimeVal = FindViewById<TextView>(Resource.Id.txt_bhv_stop_time_val);

			// time button
			btnStartTime = FindViewById<Button>(Resource.Id.btn_bhv_start_time);
			btnStopTime = FindViewById<Button>(Resource.Id.btn_bhv_stop_time);

			// button click events
			btnStartTime.Click += OnStartTimeClicked;
			btnStopTime.Click += OnStopTimeClicked;

			// initialize presenter
			presenter = new ClientAccelerometerPresenter(this);
			//presenter.LoadInitialLineChartData ();
			TurnOnBluetooth();
		}

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
						break;
					}
				}
			}

			// TODO AUTO ON NARIN UNG BLUETOOTH BAGO GAWIN TO PARA DIRE-DIRETSO
			handler = new BluetoothHandler(this);
			btService = new BluetoothService(this, handler);
			btService.BtUUID = btDevice.GetUuids()[0].Uuid;

			// connect na
			//btService.StartClient(btDevice);
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
				Logger.Log("BLUETOOTH RESULT CODE : " + resultCode);
				if (resultCode == Result.Ok)
					ConnectToBluetoothDevice();
				else
					OnBackPressed();
			}
		}
		private void OnStartTimeClicked(object sender, EventArgs e)
		{
			btService.Start();
			btService.StartClient(btDevice);
			presenter.StartTimer();
		}

		private void OnStopTimeClicked(object sender, EventArgs e)
		{
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

		#region IClientBehaviorActivity

		private LineChart lineAx, lineAy, lineAz;
		public void DisplayLineChart(LineChart lineAx, LineChart lineAy, LineChart lineAz)
		{
			this.lineAx = lineAx;
			this.lineAy = lineAy;
			this.lineAz = lineAz;
			RunOnUiThread(UpdateLineChart);
		}

		private void UpdateLineChart()
		{
			chartAx.Chart = lineAx;
			chartAy.Chart = lineAy;
			chartAz.Chart = lineAz;
		}

		public void SetStartTime(string startTime)
		{
			txtStartTimeVal.Text = startTime;
		}

		public void SetStopTime(string stopTime)
		{
			txtStopTimeVal.Text = stopTime;
		}
		#endregion


		public override void OnBackPressed()
		{
			base.OnBackPressed();
			Intent intent = new Intent(ApplicationContext, typeof(ClientMainActivity));
			StartActivity(intent);
			Finish();
		}

		public void ReceiveMessage(string message)
		{
			//Console.WriteLine(message);
			// TODO: i-pag hiwa hiwalay ung values

			int ax = 0, ay = 0, az = 0;

			
			if (int.TryParse(message, out int val))
			{
				RunOnUiThread(() => { presenter.AddEntry(ax, ay, az); });
			}
		}
	}
}
