using BgMatchResultRecorder.data;
using BgMatchResultRecorder.utils;
using Hearthstone_Deck_Tracker.Enums;
using Hearthstone_Deck_Tracker.Enums.Hearthstone;

namespace BgMatchResultRecorder
{
    internal class GameEventsHandler
    {
        internal static void OnGameStart()
        {
            if (!GameUtils.IsBattlegroundsMatch()) return;
            Logger.Info("OnGameStart");

            // Refresh Match State
            AppState.matchState = new MatchState();
            AppState.lastKnownRating = GameUtils.GetBattlegroundsRating();
            AppState.lastKnownRegion= GameUtils.GetRegion();
        }

        internal static void OnGameEnd()
        {
            if (!GameUtils.IsBattlegroundsMatch()) return;
            Logger.Info("OnGameEnd");

            shouldCheckOpponentHero = false;
            shouldCheckRating = true;

            MatchState state = AppState.matchState;

            state.LastPlayedTurn = GameUtils.GetLastPlayedTurn();
            state.StartDateTime = GameUtils.GetMatchStartDateTime();
            state.EndDateTime = GameUtils.GetMatchEndDateTime();
            state.WasConceded = GameUtils.WasConceded();
            state.Player.Board = GameUtils.GetPlayerBoard();
            state.Place = GameUtils.GetPlayerPlace();

            state.Region = AppState.lastKnownRegion;
            state.AvailableRaces = AppState.lastKnownAvailableRaces.Value;

            AppState.lastFinishedMatchId = AppState.nextMatchId;
            AppState.nextMatchId = GameUtils.GetRandomUniqueId();

            DebugStuff.StartLoggingOpponent(interval: 500, autoCancelAfterMs: 3 * 1000);

            DebugStuff.LogAppState();
        }

        internal static void OnInMenu()
        {
            if (!GameUtils.IsBattlegroundsMatch()) return;
            Logger.Info("OnInMenu");

            DebugStuff.StartLoggingOpponent(interval: 500, autoCancelAfterMs: 3 * 1000);
        }

        internal static void OnModeChanged(Mode mode)
        {
            Logger.Info($"OnModeChanged: {mode} rating: {GameUtils.GetBattlegroundsRating()}");

            if (mode == Mode.BACON)
            {
                TryUpdateRating();
                AppState.lastKnownRegion = GameUtils.GetRegion();
            }

            if (GameUtils.IsBattlegroundsMatch()) return;

            // GAMEPLAY triggers after GameStart but before First Turn.
            // Info about Races already available.
            if (mode == Mode.GAMEPLAY)
            {
                AppState.lastKnownAvailableRaces = VersionedBox<AvailableRaces>.From(GameUtils.GetAvailableRaces());
                DebugStuff.StartLoggingRaces(interval: 500, autoCancelAfterMs: 3 * 1000);
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
            if (card != null)
            {
                Logger.Info($"OnOpponentCreateInPlay: dbId: {card.DbfIf} Name: {card.Name} Id: {card.Id} type: {card.Type}");
            }
            else
            {
                Logger.Info($"OnOpponentCreateInPlay: null");
            }

            TryCaptureOpponentHero(card);
        }

        internal static void TurnStart(ActivePlayer player)
        {
            if (!GameUtils.IsBattlegroundsMatch()) return;

            DebugStuff.StartLoggingOpponent(interval: 500, autoCancelAfterMs: 3 * 1000);

            TryUpdateAvailableRaces();

            if (GameUtils.IsCombatPhase(player))
            {
                Logger.Info($"TurnStart: Combat Phase");
                //todo earlier
                shouldCheckOpponentHero = true;
            }
            else
            {
                Logger.Info($"TurnStart: Shopping Phase");
                shouldCheckOpponentHero = false;

                if (AppState.matchState.Player.Hero.TurnWhenCaptured == null)
                {
                    AppState.matchState.Player.Hero = GameUtils.GetPlayerHero();
                }
            }
        }


        // ---------------------------------------------------------------------------------------
        //                                          Utils 
        // ---------------------------------------------------------------------------------------
        private static bool shouldCheckOpponentHero = false;
        private static bool shouldCheckRating = false;

        internal static void resetState()
        {
            shouldCheckOpponentHero = false;
            shouldCheckRating = false;
        }

        internal static void TryCaptureOpponentHero(Hearthstone_Deck_Tracker.Hearthstone.Card card)
        {
            if (!shouldCheckOpponentHero || card == null || card.Type != GameUtils.CARD_TYPE_HERO) return;

            Logger.Info($"TryCaptureOpponentHero Hero: {card.LocalizedName} id: ${card.DbfIf}");

            shouldCheckOpponentHero = false;
            AppState.matchState.Opponent.Hero = GameUtils.GetHero(card);
        }

        internal static void TryUpdateRating()
        {
            if (!shouldCheckRating) return;

            int? newRating = GameUtils.GetBattlegroundsRating();
            Logger.Info($"TryUpdateRating New: {newRating} lastKnown: {AppState.lastKnownRating}");

            if (newRating != null)
            {
                AppState.matchState.RatingDiff = newRating - AppState.lastKnownRating;
                AppState.matchState.Rating = newRating;

                AppState.lastKnownRating = newRating;
            }

            shouldCheckRating = false;
        }

        internal static void TryUpdateAvailableRaces()
        {
            if (AppState.lastKnownAvailableRaces?.ShouldUpdate() == true || AppState.lastKnownAvailableRaces.Value?.DebugMessage != null)
            {
                Logger.Info($"UpdateAvailableRaces");
                AppState.lastKnownAvailableRaces = VersionedBox<AvailableRaces>.From(GameUtils.GetAvailableRaces());
            }
        }
    }
}