using System;
using System.Collections.Generic;
using System.Linq;

namespace PeriwinkleApp.Core.Sources.Extensions
{
    public static partial class ApiUriExtension
    {
        public static string ToGetParams (this IEnumerable <KeyValuePair <string, string>> keyValuePairs)
        {
            List <string> parameters = 
                keyValuePairs.Select (keyValuePair => keyValuePair.Key + "=" + keyValuePair.Value).ToList ();

            return String.Join ("&", parameters);
        }    
        
        public static string ToUrl (this ApiUri apiUri)
        {
            return !serviceUris.TryGetValue (apiUri, out string uriStr)
                       ? String.Empty
                       : "http://" + ipAddress + "/periwinkle/api" + uriStr;
        }
    }
}
