using System;
using System.Threading;
using System.Threading.Tasks;

namespace BgMatchResultRecorder.utils
{
    class RecursiveJob
    {
        private CancellationTokenSource cancellatioinToken = null;
        private CancellationTokenSource cancellatioinTokenAuto = null;

        internal string tag { get; set; } = "UnknownJob";
        internal long initialDelay { get; set; } = 0;

        private long delay = -1;

        internal async void Start(Func<long> action, long autoCancelAfterMs = -1)
        {
            delay = initialDelay;

            try
            {
                do
                {
                    cancellatioinToken = new CancellationTokenSource();

                    if (delay > 0)
                    {
                        await Task.Delay(TimeSpan.FromMilliseconds(delay), cancellatioinToken.Token);
                    }

                    delay = action();
                } while (delay >= 0);
            }
            catch (TaskCanceledException e)
            {
                Logger.Info($"{tag} Catch: {e.Message}");
            }
        }

        internal void Cancel()
        {
            cancellatioinToken?.Cancel();
            cancellatioinTokenAuto?.Cancel();
            delay = -1;
        }

        internal async void AutoCancelAfter(long delayMs, Action cleanUp)
        {
            try
            {
                cancellatioinTokenAuto = new CancellationTokenSource();
                await Task.Delay(TimeSpan.FromMilliseconds(delayMs), cancellatioinTokenAuto.Token);

                Cancel();
                cleanUp();
            }
            catch (TaskCanceledException e)
            {
                Logger.Info($"{tag} AutoCancel Catch: {e.Message}");
                // cleanUp();
            }
        }
    }
}
