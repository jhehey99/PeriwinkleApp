using System;
using System.Collections.Generic;
using System.Globalization;
using PeriwinkleApp.Core.Sources.Models.Common;

namespace PeriwinkleApp.Core.Sources.Extensions
{
	public static class BasicEntryExtension
	{
		public static string ToFormattedString (this List <BasicEntry> basicEntries)
		{
			// same lang naman ng voltage eh, ung time lang nagkakaiba

			// 4.54 @ 9:00:00.00 AM
			//		@ 9:00:02.00 AM
			// ... ganto format

            string str = null;

			// walang laman entries
			int count = basicEntries.Count;
			if (count <= 0)
				return null;

			// get ung first
			str = $"{basicEntries[0].Voltage.ToString(CultureInfo.InvariantCulture),5} @ {basicEntries[0].Time}";

            if (count == 1)
                return str;

			// pag more than 1 ung time, eto na kadugtong
			for (int i = 1; i < count; i++)
				str += $"{Environment.NewLine}{" ",5} @ {basicEntries[i].Time}";

			return str;
		}
	}
}
