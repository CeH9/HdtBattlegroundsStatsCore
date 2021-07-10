using System.Threading;
using System.Threading.Tasks;
using WebSocketSharp;

namespace BgMatchResultRecorder
{
    internal class WebSockets
    {
        internal static WebSocket ws;
        private static bool isEnabled = false;
        private static CancellationTokenSource reconnectionTaskToken = null;

        internal static void Open()
        {
            var host = Settings.config.websocketsServerAddress;
            Logger.Info($"Websockets try open {host}");
            isEnabled = true;

            // TODO secure WSS protocol
            ws = new WebSocket($"ws://{host}");
            ws.OnOpen += (sender, e) =>
            {
                Logger.Info("WebSockets OnOpen");
                ws.SendAsync("Hello from BgStats plugin!", (completed) => { });
            };
            ws.OnMessage += (sender, e) =>
            {
                Logger.Info($"WebSockets OnMessage msg: {e.Data}");

                //MultiThreadingUtil.dispatcherMain.Invoke(() =>
                //{
                //    Logger.Info($"Websockets Invoke Thread: {currentThread()} msg: {e.Data}");
                //});
            };
            ws.OnError += (sender, args) =>
            {
                Logger.Info($"WebSockets [OnError] Message: {args.Message} Exception: {args.Exception}");
            };
            ws.OnClose += (sender, e) =>
            {
                Logger.Info($"WebSockets [OnClose] {e.Code} Reason: {e.Reason}");

                MultiThreadingUtil.dispatcherMain.Invoke(() =>
                {
                    if (isEnabled)
                    {
                        reconnectionTaskToken = new CancellationTokenSource();
                        ScheduleReconnect(reconnectionTaskToken.Token);
                    }
                });
            };
            ws.ConnectAsync();
        }

        internal static void Close()
        {
            Logger.Info("Websockets Close");

            isEnabled = false;
            if (reconnectionTaskToken != null && !reconnectionTaskToken.IsCancellationRequested)
            {
                reconnectionTaskToken.Cancel();
                reconnectionTaskToken = null;
            }

            if (ws == null) return;

            ws.CloseAsync(CloseStatusCode.Normal);

            // Probably ws instance could be garbage collected before
            // CloseAsync finishes, but we dont care 
            ws = null;
        }

        internal static async void ScheduleReconnect(CancellationToken token)
        {
            Logger.Info($"WebSockets schedule reconnect in {Settings.WS_RECONNECT_DELAY.TotalSeconds} seconds");
            try
            {
                await Task.Delay(Settings.WS_RECONNECT_DELAY, token);

                if (isEnabled)
                {
                    reconnectionTaskToken = null;
                    Open();
                }
            }
            catch
            {
                Logger.Info("Websockets reconnection was cancelled!");
            }
        }

        private static string currentThread()
        {
            return Thread.CurrentThread.Name != null ? Thread.CurrentThread.Name : "Unknown";
        }
    }
}
