using Newtonsoft.Json;

namespace BgMatchResultRecorder.network
{
    class Serializer
    {
        internal static string ToJson(object value)
        {
            return JsonConvert.SerializeObject(value, Formatting.Indented);
        }
    }
}
