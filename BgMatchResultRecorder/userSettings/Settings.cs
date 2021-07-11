using Newtonsoft.Json;
using System.IO;

namespace BgMatchResultRecorder
{
    internal class Config
    {
        internal static readonly string _configLocation = Hearthstone_Deck_Tracker.Config.AppDataPath + @"\Plugins\BgMatchResultRecorder.config";

        [JsonProperty]
        internal string websocketsServerAddress = "localhost:777";

        internal void save()
        {
            File.WriteAllText(_configLocation, JsonConvert.SerializeObject(this, Formatting.Indented));
        }
    }

    internal static class Settings
    {
        internal static bool IsPluginEnabled = true;
        internal static System.TimeSpan WS_RECONNECT_DELAY = System.TimeSpan.FromMilliseconds(150_000);

        internal static Config config = null;

        internal static void LoadConfig()
        {
            try
            {
                // Load from file, if available
                config = JsonConvert.DeserializeObject<Config>(File.ReadAllText(Config._configLocation));
            }
            catch
            {
                // Create new
                config = new Config();
                config.save();
            }
        }

        internal static void UnloadConfig()
        {
            config = null;
        }
    }
}