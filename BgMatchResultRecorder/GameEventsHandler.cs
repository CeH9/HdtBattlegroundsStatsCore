using BgMatchResultRecorder.data;
using Hearthstone_Deck_Tracker.Enums;
using Hearthstone_Deck_Tracker.Enums.Hearthstone;

namespace BgMatchResultRecorder
{
    internal class GameEventsHandler
    {
        private static bool shouldCheckForOpponentHero = false;

        internal static void resetState()
        {
            shouldCheckForOpponentHero = false;
        }

        internal static void OnGameStart()
        {
            if (!GameUtils.IsBattlegroundsMatch()) return;
            Logger.Info("OnGameStart");

            // Refresh Match State
            AppState.matchState = AppState.DefaultMatchState();
        }

        internal static void OnGameEnd()
        {
            if (!GameUtils.IsBattlegroundsMatch()) return;
            Logger.Info("OnGameEnd");

            shouldCheckForOpponentHero = false;

            MatchState state = AppState.matchState;

            state.LastPlayedTurn = GameUtils.GetLastPlayedTurn();
            state.StartDateTime = GameUtils.GetMatchStartDateTime();
            state.EndDateTime = GameUtils.GetMatchEndDateTime();
            state.WasConceded = GameUtils.WasConceded();
            state.Player.Board = GameUtils.GetBoard();
        }

        internal static void OnInMenu()
        {
            if (!GameUtils.IsBattlegroundsMatch()) return;
            Logger.Info("OnInMenu");

            //OnDebugButtonClicked();
        }

        internal static void OnModeChanged(Mode mode)
        {
            Logger.Info($"OnModeChanged: {mode}");

            if (GameUtils.IsBattlegroundsMatch()) return;

            if(mode == Mode.GAMEPLAY)
            {
                //AppState.matchState
            }
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
            
            //var hero = new Hero();
            //hero.Id = card.Id;
            //hero.Name = card.Name;
            //hero.DbId = card.DbfIf;

            //MatchState.instance.LastOpponentHero = hero;
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

            //OnDebugButtonClicked();
        } 
    }
}