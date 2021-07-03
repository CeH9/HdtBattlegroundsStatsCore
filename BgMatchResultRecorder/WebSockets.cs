using System.Threading;
using WebSocketSharp;

namespace BgMatchResultRecorder
{
    public class WebSockets
    {
        public static WebSocket ws;

        public static void Open()
        {
            var host = "localhost:50000";
            Logger.Info($" {host}");

            ws = new WebSocket($"ws://{host}");
            ws.OnMessage += (sender, e) =>
            {
                Logger.Info($"[OnMessage] Thread: {currentThread()} msg: {e.Data}");

                MultiThreadingUtil.dispatcherMain.Invoke(() =>
                {
                    Logger.Info($"[Invoke] Thread: {currentThread()} msg: {e.Data}");
                });
            };
            ws.ConnectAsync();
        }

        public static void Close()
        {
            if (ws == null) return;
            Logger.Info("");

            ws.Close();
            ws = null;
        }

        private static string currentThread()
        {
            return Thread.CurrentThread.Name != null ? Thread.CurrentThread.Name : "Unknown";
        }
    }
}
