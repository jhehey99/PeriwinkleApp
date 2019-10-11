using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;
using Microcharts;
using Microcharts.Droid;
using SkiaSharp;
using Orientation = Microcharts.Orientation;

namespace Periwinkle.Microcharts
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);

			var charts = CreateQuickstart();
            FindViewById<ChartView>(Resource.Id.chartView1).Chart = charts;
        }
		
		public LineChart CreateQuickstart ()
		{
			Entry[] entries = new[]
						  {
							  new Entry (0)
							  {
								  Label = "9:01",
								  ValueLabel = "0",
								  Color = SKColor.Parse ("#266489"),
							  },
							  new Entry (1)
							  {
								  Label = "9:02",
								  ValueLabel = "1",
								  Color = SKColor.Parse ("#68B9C0"),
							  },
							  new Entry (2)
							  {
								  Label = "9:03",
								  ValueLabel = "2",
								  Color = SKColor.Parse ("#90D585"),
							  },
							  new Entry (3)
							  {
								  Label = "9:04",
								  ValueLabel = "3",
								  Color = SKColor.Parse ("#90D585"),
							  },
							  new Entry (4)
							  {
								  Label = "9:05",
								  ValueLabel = "4",
								  Color = SKColor.Parse ("#90D585"),
							  },
							  new Entry (5)
							  {
								  Label = "9:06",
								  ValueLabel = "5",
								  Color = SKColor.Parse ("#90D585"),
							  },
							  new Entry (5)
							  {
								  Label = "9:07",
								  ValueLabel = "5",
								  Color = SKColor.Parse ("#90D585"),
							  },
							  new Entry (4)
							  {
								  Label = "9:08",
								  ValueLabel = "4",
								  Color = SKColor.Parse ("#90D585"),
							  },
							  new Entry (3)
							  {
								  Label = "9:09",
								  ValueLabel = "3",
								  Color = SKColor.Parse ("#90D585"),
							  },
							  new Entry (2)
							  {
								  Label = "9:10",
								  ValueLabel = "2",
								  Color = SKColor.Parse ("#90D585"),
							  },
							  new Entry (1)
							  {
								  Label = "9:11",
								  ValueLabel = "1",
								  Color = SKColor.Parse ("#90D585"),
							  },
							  new Entry (0)
							  {
								  Label = "9:12",
								  ValueLabel = "0",
								  Color = SKColor.Parse ("#90D585"),
							  },
						  };

			return new LineChart ()
				   {
					   Entries = entries,
					   LabelOrientation = Orientation.Horizontal,
					   ValueLabelOrientation = Orientation.Horizontal,
					   MinValue = 0,
					   MaxValue = 5
				   };
		}

    }
}