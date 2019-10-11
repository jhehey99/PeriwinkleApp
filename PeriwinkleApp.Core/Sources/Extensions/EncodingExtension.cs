using System;
using System.Text;

namespace PeriwinkleApp.Core.Sources.Extensions
{
    public static class EncodingExtension
    {
        public static string ToBase64 (this string plainText)
        {
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            return Convert.ToBase64String (plainTextBytes);
        }

		public static byte[] ToBytesArray (this string str)
		{
			return Encoding.UTF8.GetBytes (str);
		}
		
		public static string ToString (this byte[] bytes)
		{
			return Encoding.UTF8.GetString (bytes);
		}
    }
}
