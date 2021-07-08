using Newtonsoft.Json;

namespace BgMatchResultRecorder.network
{
    class Serializer
    {
        internal static string toJson(object value)
        {
            return JsonConvert.SerializeObject(value, Formatting.Indented);
        }
    }
}
