using System.Linq;
using BgMatchResultRecorder.data;
using HearthDb.Enums;
using Hearthstone_Deck_Tracker.Enums;
using Hearthstone_Deck_Tracker.Enums.Hearthstone;
using Hearthstone_Deck_Tracker.Hearthstone.Entities;
using Core = Hearthstone_Deck_Tracker.API.Core;

namespace BgMatchResultRecorder
{
    public class CoreEventsHandler
    {
        private static bool shouldCheckForOpponentHero = false;

        internal static void resetState()
        {
            shouldCheckForOpponentHero = false;
        }

        internal static void GameStart()
        {
            if (!GameUtils.IsBattlegroundsMatch()) return;
            Logger.Info("CoreEvents GameStart");

            OnDebugButtonClicked();
        }

        internal static void OnGameEnd()
        {
            if (!GameUtils.IsBattlegroundsMatch()) return;
            Logger.Info("OnGameEnd");
            shouldCheckForOpponentHero = false;

            OnDebugButtonClicked();
        }

        internal static void OnInMenu()
        {
            if (!GameUtils.IsBattlegroundsMatch()) return;
            Logger.Info("OnInMenu");

            OnDebugButtonClicked();
        }

        internal static void OnModeChanged(Mode mode)
        {
            Logger.Info($"OnModeChanged: {mode}");

            //if (GameUtils.IsBattlegroundsMatch()) return;
            OnDebugButtonClicked();

        }
        internal static void OnGameWon()
        {
            if (!GameUtils.IsBattlegroundsMatch()) return;
            Logger.Info("OnGameWon");
        }
        internal static void OnGameLost()
        {
            if (!GameUtils.IsBattlegroundsMatch()) return;
            Logger.Info("OnGameLost");
        }
        internal static void OnGameTied()
        {
            if (!GameUtils.IsBattlegroundsMatch()) return;
            Logger.Info("OnGameTied");
        }

        internal static void OnOpponentCreateInPlay(Hearthstone_Deck_Tracker.Hearthstone.Card card)
        {
            if (!GameUtils.IsBattlegroundsMatch()) return;
            if (!shouldCheckForOpponentHero || card == null || card.Type != GameUtils.CARD_TYPE_HERO) return;

            Logger.Info($"OnOpponentCreateInPlay Hero: {card.LocalizedName}");

            shouldCheckForOpponentHero = false;
            
            var hero = new Hero();
            hero.Id = card.Id;
            hero.Name = card.Name;
            hero.DbId = card.DbfIf;

            MatchState.instance.LastOpponentHero = hero;
        }

        internal static void TurnStart(ActivePlayer player)
        {
            if (!GameUtils.IsBattlegroundsMatch()) return;

            if (GameUtils.IsCombatPhase(player))
            {
                Logger.Info($"TurnStart: Combat Phase");
                shouldCheckForOpponentHero = true;
                //PrintPlayerBoard();
            }
            else
            {
                Logger.Info($"TurnStart: Shopping Phase");
                shouldCheckForOpponentHero = false;
            }

            OnDebugButtonClicked();
        }

        public static void OnDebugButtonClicked()
        {
            Logger.Info("==== OnDebugButtonClicked ====");

            GameUtils.GetAvailableRaces();
            GameUtils.GetBattlegroundsRank();
            GameUtils.GetRegion();
            GameUtils.GetTurnNumber();

            GameUtils.GetPlayerHero();
            GameUtils.GetOpponentHero();

            GameUtils.GetBattlegroundsPlace();
            GameUtils.GetBattlegroundsAllPlaces();

            Logger.Info("======== OnDebug ========");
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