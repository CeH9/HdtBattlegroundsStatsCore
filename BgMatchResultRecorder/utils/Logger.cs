using Hearthstone_Deck_Tracker.Utility.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BgMatchResultRecorder
{
    static class Logger
    {
        const bool IsLogsEnabled = true;

        internal static void Info(string msg)
        {
            if (!IsLogsEnabled) return;

            Log.Info(msg, "log", "BgStatsCorePlugin");
        }
    }
}
