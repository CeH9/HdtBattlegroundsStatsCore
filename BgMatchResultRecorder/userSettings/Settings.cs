using Newtonsoft.Json;
using System.IO;

namespace BgMatchResultRecorder
{
    public class Config
    {
        public static readonly string _configLocation = Hearthstone_Deck_Tracker.Config.AppDataPath + @"\Plugins\BgMatchResultRecorder.config";

        public string websocketsServerAddress = "localhost:777";

        public void save()
        {
            File.WriteAllText(_configLocation, JsonConvert.SerializeObject(this, Formatting.Indented));
        }
    }

    public static class Settings
    {
        public static bool IsPluginEnabled = true;
        public static Config config = null;

        public static void LoadConfig()
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

        public static void UnloadConfig()
        {
            config = null;
        }
    }
}