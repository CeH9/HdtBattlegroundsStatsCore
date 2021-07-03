using System.Linq;
using HearthDb.Enums;
using Hearthstone_Deck_Tracker.Enums;
using Hearthstone_Deck_Tracker.Enums.Hearthstone;
using Hearthstone_Deck_Tracker.Hearthstone.Entities;
using Core = Hearthstone_Deck_Tracker.API.Core;

namespace BgMatchResultRecorder
{
    public class CoreEventsHandler
    {
        internal static void GameStart()
        {
            Logger.Info("BgMatchResultRecorderPlugin GameStart");

            if (Hearthstone_Deck_Tracker.Core.Game.IsBattlegroundsMatch)
            {

            }
        }

        internal static void OnGameEnd()
        {
            Logger.Info("BgMatchResultRecorderPlugin OnGameEnd");

            if (Hearthstone_Deck_Tracker.Core.Game.IsBattlegroundsMatch)
            {

            }
        }

        internal static void OnInMenu()
        {
            Logger.Info("BgMatchResultRecorderPlugin OnInMenu");
        }

        internal static void OnModeChanged(Mode mode)
        {
            Logger.Info($"BgMatchResultRecorderPlugin OnModeChanged: {mode}");
        }
        internal static void OnGameWon()
        {
            Logger.Info("BgMatchResultRecorderPlugin OnGameWon");
        }
        internal static void OnGameLost()
        {
            Logger.Info("BgMatchResultRecorderPlugin OnGameLost");
        }
        internal static void OnGameTied()
        {
            Logger.Info("BgMatchResultRecorderPlugin OnGameTied");
        }

        internal static void TurnStart(ActivePlayer player)
        {
            var turn = Hearthstone_Deck_Tracker.Core.Game.GetTurnNumber();

            if (BattlegroundsUtils.IsCombatPhase(player))
            {
                Logger.Info($"BgMatchResultRecorderPlugin TurnStart: {turn} Combat Phase");
                PrintPlayerBoard();
            }
            else
            {
                Logger.Info($"BgMatchResultRecorderPlugin TurnStart: {turn} Shopping Phase");
            }
        }

        public static void PrintPlayerBoard()
        {
            Logger.Info("BgMatchResultRecorderPlugin PrintPlayerBoard");

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
