namespace BgMatchResultRecorder.utils.watchers
{
    class OpponentWatcher
    {
        private RecursiveJob job = null;
        private string tag = "OpponentWatcher";
        private long intervalMs = 150;
        private long cancelAfterMs = 5000; // 5 sec

        internal void Start()
        {

            if (job != null)
            {
                Logger.Info($"{tag}: Already started! Skip");
                return;
            }

            Logger.Info($"{tag}: Start");

            job = new RecursiveJob { initialDelay = 0, tag = tag };
            job.AutoCancelAfter(cancelAfterMs, () => { Stop(); });

            job.Start(() =>
            {
                //try
                //{
                //    Hearthstone_Deck_Tracker.Hearthstone.Player opponent = Core.Game.Opponent;
                //    Hearthstone_Deck_Tracker.Hearthstone.Entities.Entity hero = opponent.Board.FirstOrDefault(x => x.IsHero);

                //    if (hero != null)
                //    {
                //        Logger.Info($"{tag}: Id: {hero.Id} cardId: {hero.CardId} Name: {hero.Name} | {TimeStamp()}");
                //    }
                //    else
                //    {
                //        Logger.Info($"{tag}: null | {TimeStamp()}");
                //    }
                //}
                //catch (Exception e) { Logger.Info($"{tag}: Catch: {e} | {TimeStamp()}"); }

                return intervalMs;
            });
        }

        internal void Stop()
        {
            if (job == null) return;

            job.Cancel();
            job = null;
        }
    }
}
