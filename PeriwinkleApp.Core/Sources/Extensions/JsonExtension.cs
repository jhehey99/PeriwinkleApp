using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace PeriwinkleApp.Core.Sources.Extensions
{
    public static class JsonExtension
    {
        public static string PrettySerialize (this object obj)
        {
            string json = JsonConvert.SerializeObject (obj);
            return JToken.Parse (json).ToString (Formatting.Indented);
        }

        public static string JsonSerialize (this object obj)
        {
            return JsonConvert.SerializeObject (obj);
        }

        public static TObject JsonDeserialize <TObject> (this string json, JsonSerializerSettings settings = null)
        {
            settings = settings ?? new JsonSerializerSettings {NullValueHandling = NullValueHandling.Ignore};
            return JsonConvert.DeserializeObject <TObject> (json, settings);
        }
        
    }
}
