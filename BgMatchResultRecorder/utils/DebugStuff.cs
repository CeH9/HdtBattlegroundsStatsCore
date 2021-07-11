using BgMatchResultRecorder.data;
using BgMatchResultRecorder.network;

namespace BgMatchResultRecorder.utils
{
    internal class DebugStuff
    {
        internal static void LogAppState()
        {               
            Logger.Info(Serializer.toJson(AppState.matchState));
        }
    }
}
