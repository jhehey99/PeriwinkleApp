using Android.Graphics;

namespace PeriwinkleApp.Android.Source.Utils
{
	public static class GraphicsUtil
	{
		public static string HexConverter()
		{
			Color color = new Color((int)(Java.Lang.Math.Random() * 0x1000000));
			return "#" + color.R.ToString("X2") + color.G.ToString("X2") + color.B.ToString("X2");
		}
    }
}
