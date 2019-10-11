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
	public interface IClientBehaviorActivity
	{
		void DisplayLineChart(LineChart lineChart);
		void SetStartTime (string startTime);
		void SetStopTime (string stopTime);
		void CreateRandomEntryOnUiThread ();
	}


	[Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = false)]
    public class ClientBehaviorActivity : AppCompatActivity, IClientBehaviorActivity, IMessageReceiver
    {
		private TextView txtStartTimeVal, txtStopTimeVal;
		private Button btnStartTime, btnStopTime;

        BluetoothAdapter btAdapter;
        BluetoothService btService;
        BluetoothHandler handler;
        BluetoothDevice btDevice;

        private ChartView chartView;

		private IClientBehaviorPresenter presenter;
		const int REQUEST_ENABLE_BT = 3;

		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);

			//RequestedOrientation = ScreenOrientation.Landscape;
			SetContentView (Resource.Layout.client_frag_behavior);

			// set reference sa chartview
			chartView = FindViewById <ChartView> (Resource.Id.chart_view_behavior);

            // time text views
			txtStartTimeVal = FindViewById<TextView>(Resource.Id.txt_bhv_start_time_val);
			txtStopTimeVal = FindViewById<TextView>(Resource.Id.txt_bhv_stop_time_val);

            // time button
			btnStartTime = FindViewById <Button> (Resource.Id.btn_bhv_start_time);
			btnStopTime = FindViewById <Button> (Resource.Id.btn_bhv_stop_time);

			// button click events
			btnStartTime.Click += OnStartTimeClicked;
			btnStopTime.Click += OnStopTimeClicked;

			// initialize presenter
            presenter = new ClientBehaviorPresenter (this);
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
			if(requestCode == REQUEST_ENABLE_BT)
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
            presenter.StartTimer ();
		}

        private void OnStopTimeClicked (object sender, EventArgs e)
		{
			presenter.StopTimer ();
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

		private LineChart chart;
        public void DisplayLineChart(LineChart lineChart)
		{
			this.chart = lineChart;
			RunOnUiThread (UpdateLineChart);
		}

		private void UpdateLineChart ()
		{
			chartView.Chart = chart;
        }

        public void SetStartTime (string startTime)
		{
			txtStartTimeVal.Text = startTime;
		}

        public void SetStopTime (string stopTime)
		{
			txtStopTimeVal.Text = stopTime;
		}

		public void CreateRandomEntryOnUiThread ()
		{
			// Kailangan kasi i-update ung ui on the ui thread.
			// ung timer kasi, is nagru-run on a separate thread.
			RunOnUiThread (() => { presenter.CreateRandomEntry (); });
		}

#endregion


        public override void OnBackPressed()
		{
			base.OnBackPressed();
			Intent intent = new Intent (ApplicationContext, typeof (ClientMainActivity));
			StartActivity (intent);
			Finish();
        }

        public void ReceiveMessage(string message)
        {
            //Console.WriteLine(message);
            
            if(int.TryParse(message, out int val))
            {
                RunOnUiThread(() => { presenter.AddEntry(val); });
            }
        }
    }
}
