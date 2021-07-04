using System.Linq;
using HearthDb.Enums;
using Hearthstone_Deck_Tracker.Enums;
using Hearthstone_Deck_Tracker.Enums.Hearthstone;
using Hearthstone_Deck_Tracker.Hearthstone.Entities;
using Core = Hearthstone_Deck_Tracker.API.Core;
using BgUtils = Hearthstone_Deck_Tracker.Hearthstone.BattlegroundsUtils;
using System;

namespace BgMatchResultRecorder
{
    public class CoreEventsHandler
    {
        internal static void GameStart()
        {
            Logger.Info("CoreEvents GameStart");

            if (Hearthstone_Deck_Tracker.Core.Game.IsBattlegroundsMatch)
            {
                try
                {
                    var gameId = Core.Game.CurrentGameStats.GameId;
                    var availableRaces = BgUtils.GetAvailableRaces(gameId).ToList();

                    Logger.Info($"AvailableRaces: {String.Join(", ", availableRaces)}");

                    Logger.Info($"Rank: {Core.Game.BattlegroundsRatingInfo.Rating}");
                    Logger.Info($"Region: {Core.Game.CurrentRegion}");
                }
                catch (Exception e)
                {
                    Logger.Info($"Catch: {e.Message}");
                }
            }
        }

        internal static void OnGameEnd()
        {
            Logger.Info("OnGameEnd");

            if (Hearthstone_Deck_Tracker.Core.Game.IsBattlegroundsMatch)
            {

            }
        }

        internal static void OnInMenu()
        {
            Logger.Info("OnInMenu");

            try
            {
                Logger.Info($"Rank: {Core.Game.BattlegroundsRatingInfo.Rating}");
                Logger.Info($"Region: {Core.Game.CurrentRegion}");
            }
            catch (Exception e)
            {
                Logger.Info($"Catch: {e.Message}");
            }
        }

        internal static void OnModeChanged(Mode mode)
        {
            Logger.Info($"OnModeChanged: {mode}");
        }
        internal static void OnGameWon()
        {
            Logger.Info("OnGameWon");
        }
        internal static void OnGameLost()
        {
            Logger.Info("OnGameLost");
        }
        internal static void OnGameTied()
        {
            Logger.Info("OnGameTied");
        }

        internal static void TurnStart(ActivePlayer player)
        {
            var turn = Hearthstone_Deck_Tracker.Core.Game.GetTurnNumber();

            if (BattlegroundsUtils.IsCombatPhase(player))
            {
                Logger.Info($"TurnStart: {turn} Combat Phase");
                PrintPlayerBoard();

                var gameId = Core.Game.CurrentGameStats.GameId;
                var availableRaces = BgUtils.GetAvailableRaces(gameId).ToList();
                Logger.Info($"AvailableRaces: {String.Join(", ", availableRaces)}");
            }
            else
            {
                Logger.Info($"TurnStart: {turn} Shopping Phase");
            }
        }

        public static void OnDebugButtonClicked()
        {
            Logger.Info("==== OnDebugButtonClicked ====");
            try
            {
                var gameId = Core.Game.CurrentGameStats.GameId;
                var availableRaces = BgUtils.GetAvailableRaces(gameId).ToList();

                Logger.Info($"AvailableRaces: {String.Join(", ", availableRaces)}");
            }
            catch (Exception e)
            {
                Logger.Info($"AvailableRaces Catch: {e.Message}");
            }

            try
            {
                Logger.Info($"Rank: {Core.Game.BattlegroundsRatingInfo.Rating}");
            }
            catch (Exception e)
            {
                Logger.Info($"Rank Catch: {e.Message}");
            }
            try
            {
                Logger.Info($"Region: {Core.Game.CurrentRegion}");
            }
            catch (Exception e)
            {
                Logger.Info($"Region Catch: {e.Message}");
            }
        }

        public static void PrintPlayerBoard()
        {
            Logger.Info("PrintPlayerBoard");

            var board = Core.Game.Player.Board;
            IOrderedEnumerable<Entity> entities = board.Where(x => x.IsMinion)
                .Select(x => x.Clone())
                .OrderBy(x => x.GetTag(GameTag.ZONE_POSITION));

            entities.ToList().ForEach(entity =>
            {
                Logger.Info($"Entity Id: {entity.Id} CardId: {entity.CardId} Name: {entity.Name}");
            });

            Logger.Info($"---- Found {entities.Count()} entities ----");
        }
    }
}
