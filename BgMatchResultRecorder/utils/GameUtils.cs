using HearthDb.Enums;
using Hearthstone_Deck_Tracker;
using Hearthstone_Deck_Tracker.Enums;
using Hearthstone_Deck_Tracker.Hearthstone;
using Hearthstone_Deck_Tracker.Hearthstone.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BgMatchResultRecorder
{
    class GameUtils
    {
        internal const string CARD_TYPE_HERO = "Hero";

        public static Guid GetGameId()
        {
            return Core.Game.CurrentGameStats.GameId;
        }

        internal static bool IsShoppingPhase(ActivePlayer player)
        {
            return player == ActivePlayer.Player;
        }

        internal static bool IsCombatPhase(ActivePlayer player)
        {
            return player == ActivePlayer.Opponent;
        }

        internal static bool IsBattlegroundsMatch()
        {
            return Core.Game.IsBattlegroundsMatch;
        }





        // =================================== DEBUG ZONE ============================================

        // IList<HearthDb.Enums.Race>
        internal static void GetAvailableRaces()
        {
            try
            {
                var result = BattlegroundsUtils.GetAvailableRaces(GetGameId()).ToList();
                Logger.Info($"getAvailableRaces: {String.Join(", ", result)}");
            }
            catch (Exception e)
            {
                Logger.Info($"getAvailableRaces Catch: {e.Message}");
            }
        }

        internal static void GetBattlegroundsRank()
        {
            try
            {
                var result = Core.Game.BattlegroundsRatingInfo.Rating;
                Logger.Info($"GetBattlegroundsRank: {result}");
            }
            catch (Exception e)
            {
                Logger.Info($"GetBattlegroundsRank Catch: {e.Message}");
            }
        }

        internal static void GetRegion()
        {
            try
            {
                var result = Core.Game.CurrentRegion;
                Logger.Info($"GetRegion: {result}");
            }
            catch (Exception e)
            {
                Logger.Info($"GetRegion Catch: {e.Message}");
            }
        }

        internal static void GetTurnNumber()
        {
            try
            {
                var result = Core.Game.GetTurnNumber();
                Logger.Info($"GetTurnNumber: {result}");
            }
            catch (Exception e)
            {
                Logger.Info($"GetTurnNumber Catch: {e.Message}");
            }
        }

        internal static Entity GetHero(int playerId)
        {
            //var heroId = Core.Game.PlayerEntity.GetTag(GameTag.HERO_ENTITY);                
            Entity hero = Core.Game.Entities.Values
                .Where(x => x.IsHero && x.GetTag(GameTag.PLAYER_ID) == playerId)
                .First();

            return hero;
        }

        internal static void GetPlayerHero()
        {
            try
            {
                var hero = GetHero(Core.Game.Player.Id);
                Logger.Info($"GetPlayerHero id: {hero.Id} cardId: {hero.CardId} name: {hero.Name}");
            }
            catch (Exception e)
            {
                Logger.Info($"GetPlayerHero Catch: {e.Message}");
            }
        }

        internal static void GetOpponentHero()
        {
            try
            {
                var hero = GetHero(Core.Game.Opponent.Id);
                Logger.Info($"GetOpponentHero id: {hero.Id} cardId: {hero.CardId} name: {hero.Name}");
            }
            catch (Exception e)
            {
                Logger.Info($"GetOpponentHero Catch: {e.Message}");
            }
        }

        internal static void GetBattlegroundsPlace()
        {
            try
            {
                Entity hero = GetHero(Core.Game.Player.Id);
                var place = hero.GetTag(GameTag.PLAYER_LEADERBOARD_PLACE);

                Logger.Info($"getBattlegroundsPlace: {place}");
            }
            catch (Exception e)
            {
                Logger.Info($"getBattlegroundsPlace Catch: {e.Message}");
            }
        }

        internal static void GetBattlegroundsAllPlaces()
        {
            try
            {
                Logger.Info($"GetBattlegroundsAllPlaces");

                IList<Entity> heroes = Core.Game.Entities.Values
                    .Where(x => x.IsHero)
                    .ToList();

                foreach (var hero in heroes)
                {
                    var place = hero.GetTag(GameTag.PLAYER_LEADERBOARD_PLACE);
                    Logger.Info($"Hero id: {hero.Id} cardId: {hero.CardId} name: {hero.Name} place: {place}");                        
                }
            }
            catch (Exception e)
            {
                Logger.Info($"GetBattlegroundsAllPlaces Catch: {e.Message}");
            }
        }

        internal static void PrintPlayerBoard()
        {
            Logger.Info("PrintPlayerBoard");

            var board = Core.Game.Player.Board;
            IOrderedEnumerable<Entity> entities = board.Where(x => x.IsMinion)
                .Select(x => x.Clone())
                .OrderBy(x => x.GetTag(GameTag.ZONE_POSITION));

            entities.ToList().ForEach(entity =>
            {
                var id = entity.Id;
                var CardId = entity.CardId;
                var isLevelTwo = entity.GetTag(GameTag.BACON_MINION_IS_LEVEL_TWO);

                Logger.Info($"Entity Id: {id} CardId: {CardId} IsLevelTwo: {isLevelTwo}");
            });

            Logger.Info($"---- Found {entities.Count()} entities ----");
        }
    }
}
