using BgMatchResultRecorder.data;
using BgMatchResultRecorder.network;
using Hearthstone_Deck_Tracker;
using System;
using System.Linq;

namespace BgMatchResultRecorder.utils
{
    internal class DebugStuff
    {
        internal static void LogAppState()
        {
            Logger.Info(Serializer.ToJson(AppState.matchState));
            Logger.Info($"lastKnownRating: {AppState.lastKnownRating}");
            Logger.Info($"lastKnownRegion: {AppState.lastKnownRegion}");
            Logger.Info($"lastKnownAvailableRaces: {Serializer.ToJson(AppState.lastKnownAvailableRaces?.Value)}");
        }


        // ================================ Logging Opponent's Hero ============================================
        private static RecursiveJob logOpponentJob = null;

        internal static void ToggleLoggingOpponent()
        {
            if (logOpponentJob == null)
            {
                StartLoggingOpponent(interval: 500, autoCancelAfterMs: 5 * 1000);
            }
            else
            {
                StopLoggingOpponent();
            }
        }

        internal static void StartLoggingOpponent(long interval = 500, long? autoCancelAfterMs = null)
        {
            string tag = "OpponentHero";

            if (logOpponentJob != null)
            {
                Logger.Info($"{tag}: Already started! Skip");
                return;
            }

            Logger.Info($"{tag}: StartLoggingOpponent");

            logOpponentJob = new RecursiveJob { initialDelay = 0, tag = tag };

            if (autoCancelAfterMs.HasValue)
            {
                logOpponentJob.AutoCancelAfter((long)autoCancelAfterMs, () => { StopLoggingOpponent(); });
            }

            logOpponentJob.Start(() =>
            {
                try
                {
                    Hearthstone_Deck_Tracker.Hearthstone.Player opponent = Core.Game.Opponent;
                    Hearthstone_Deck_Tracker.Hearthstone.Entities.Entity hero = opponent.Board.FirstOrDefault(x => x.IsHero);

                    if (hero != null)
                    {
                        Logger.Info($"{tag}: Id: {hero.Id} cardId: {hero.CardId} Name: {hero.Name} | {TimeStamp()}");
                    }
                    else
                    {
                        Logger.Info($"{tag}: null | {TimeStamp()}");
                    }
                }
                catch (Exception e) { Logger.Info($"{tag}: Catch: {e} | {TimeStamp()}"); }

                return interval;
            });
        }

        internal static void StopLoggingOpponent()
        {
            if (logOpponentJob == null) return;

            logOpponentJob.Cancel();
            logOpponentJob = null;
        }

        // ================================ Logging Races ============================================
        private static RecursiveJob logRacesJob = null;

        internal static void ToggleLoggingRaces()
        {
            if (logRacesJob == null)
            {
                StartLoggingRaces(interval: 500, autoCancelAfterMs: 5 * 1000);
            }
            else
            {
                StopLoggingRaces();
            }
        }

        internal static void StartLoggingRaces(long interval = 500, long? autoCancelAfterMs = null)
        {
            string tag = "RacesDump";

            if (logRacesJob != null)
            {
                Logger.Info($"{tag}: Already started! Skip");
                return;
            }

            Logger.Info($"{tag}: StartLoggingRaces");


            logRacesJob = new RecursiveJob { initialDelay = 0, tag = tag };

            if (autoCancelAfterMs.HasValue)
            {
                logRacesJob.AutoCancelAfter((long)autoCancelAfterMs, () => { StopLoggingRaces(); });
            }

            logRacesJob.Start(() =>
            {
                var races = GameUtils.GetAvailableRaces().Races.Select(x => x.Name);
                Logger.Info($"{String.Join(",", races)} | {TimeStamp()}");

                return interval;
            });
        }

        internal static void StopLoggingRaces()
        {
            if (logRacesJob == null) return;

            logRacesJob.Cancel();
            logRacesJob = null;
        }


        // ================================ Utils ============================================
        // HDT filters out equivalent logs, so make it unique by appending Timestamp to the end
        internal static string TimeStamp()
        {
            return $"Timestamp: { DateTimeOffset.Now.ToUnixTimeMilliseconds()}";
        }
    }
}
