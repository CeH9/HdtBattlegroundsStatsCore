using BgMatchResultRecorder.data;
using BgMatchResultRecorder.network;
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
        internal const string INVALID_ID = "Unknown";
        internal const int INVALID_INT_ID = -1;

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

        internal static DateTime? GetMatchStartDateTime()
        {
            try
            {
                var stats = Core.Game.CurrentGameStats;

                return stats.StartTime;
            }
            catch (Exception e)
            {
                var msg = e.Message;
                Logger.Info($"GetMatchStartDateTime Catch: {msg}");
                return null;
            }
        }

        internal static DateTime? GetMatchEndDateTime()
        {
            try
            {
                var stats = Core.Game.CurrentGameStats;

                return stats.EndTime;
            }
            catch (Exception e)
            {
                Logger.Info($"GetMatchEndDateTime Catch: {e.Message}");
                return null;
            }
        }

        internal static int GetTurnNumber()
        {
            return Core.Game.GetTurnNumber();
        }

        internal static string GetRandomUniqueId()
        {
            return Guid.NewGuid().ToString();
        }

        internal static int GetLastPlayedTurn()
        {
            try
            {
                Hearthstone_Deck_Tracker.Stats.GameStats stats = Core.Game.CurrentGameStats;

                return stats.Turns;
            }
            catch (Exception e)
            {
                Logger.Info($"LastPlayedTurn Catch: {e.Message}");
                return -1;
            }
        }

        internal static bool WasConceded()
        {
            try
            {
                var stats = Core.Game.CurrentGameStats;

                return stats.WasConceded;
            }
            catch (Exception e)
            {
                Logger.Info($"WasConceded Catch: {e.Message}");
                return false;
            }
        }

        internal static Board GetPlayerBoard()
        {
            try
            {
                List<Minion> minions = Core.Game.Player.Board
                    .Where(x => x.IsMinion)
                    .OrderBy(x => x.GetTag(GameTag.ZONE_POSITION))
                    .Select(x => ToMinion(x))
                    .ToList();

                return new Board
                {
                    Minions = minions,
                    TurnWhenCaptured = GetTurnNumber()
                };
            }
            catch (Exception e) { return new Board { DebugMessage = e.ToString() }; }
        }

        internal static Board GetOpponentBoard()
        {
            try
            {
                List<Minion> minions = Core.Game.Opponent.Board
                    .Where(x => x.IsMinion)
                    .OrderBy(x => x.GetTag(GameTag.ZONE_POSITION))
                    .Select(x => ToMinion(x))
                    .ToList();

                return new Board
                {
                    Minions = minions,
                    TurnWhenCaptured = GetTurnNumber()
                };
            }
            catch (Exception e) { return new Board { DebugMessage = e.ToString() }; }
        }

        private static Minion ToMinion(Entity entity)
        {
            Card dbCard = Database.GetCardFromId(entity.CardId);

            Minion minion = new Minion
            {
                Id = entity.CardId,
                DbId = dbCard.DbfIf,
                Name = dbCard.Name,
                ZonePosition = entity.GetTag(GameTag.ZONE_POSITION),
                Health = entity.GetTag(GameTag.ATK),
                Attack = entity.GetTag(GameTag.HEALTH),
                TavernTier = entity.GetTag(GameTag.TECH_LEVEL),
                IsGolden = entity.HasTag(GameTag.PREMIUM),
                Poisonous = entity.HasTag(GameTag.POISONOUS)
            };

            return minion;
        }

        internal static AvailableRaces GetAvailableRaces()
        {
            try
            {
                return new AvailableRaces
                {
                    Races = BattlegroundsUtils.GetAvailableRaces(GetGameId())
                    .Select(race => new data.Race
                    {
                        Id = (int)race,
                        Name = race.ToString()
                    })
                    .ToList()
                };
            }
            catch (Exception e) { return new AvailableRaces { DebugMessage = e.ToString() }; }
        }

        internal static Hero GetHeroFromCard(Card card)
        {
            try
            {
                return new Hero
                {
                    Id = card.Id,
                    Name = card.Name,
                    DbId = card.DbfIf,
                    TurnWhenCaptured = GetTurnNumber()
                };
            }
            catch (Exception e) { return new Hero { DebugMessage = e.ToString() }; }
        }

        internal static Hero GetPlayerHero()
        {
            try
            {
                Entity hero = GetHeroPlayerEntity();
                Card dbHero = Database.GetCardFromId(hero.CardId);

                return new Hero
                {
                    Id = dbHero.Id,
                    Name = dbHero.Name,
                    DbId = dbHero.DbfIf,
                    TurnWhenCaptured = GetTurnNumber()
                };
            }
            catch (Exception e) { return new Hero { DebugMessage = e.ToString() }; }
        }

        internal static int? GetBattlegroundsRating()
        {
            try
            {
                return Core.Game.BattlegroundsRatingInfo.Rating;
            }
            catch { return null; }
        }

        internal static string GetRegion()
        {
            try
            {
                return Core.Game.CurrentRegion.ToString();
            }
            catch { return null; }
        }

        internal static int? GetPlayerPlace()
        {
            try
            {
                return GetHeroPlayerEntity().GetTag(GameTag.PLAYER_LEADERBOARD_PLACE);
            }
            catch { return null; }
        }



        // =================================== Utils ============================================     
        internal static Entity GetHeroPlayerEntity()
        {
            return GetHeroEntityByPlayerId(Core.Game.Player.Id);
        }

        internal static Entity GetHeroEntityByPlayerId(int playerId)
        {
            return Core.Game.Entities.Values
                   .Where(x => x.IsHero && x.GetTag(GameTag.PLAYER_ID) == playerId)
                   .First();
        }

        // =================================== DEBUG ZONE ============================================     
        internal static void GetPlayerInfo()
        {
            try
            {
                string result = Core.Game.Player.Name;
                Logger.Info($"GetPlayerInfo: {result}");
            }
            catch (Exception e)
            {
                Logger.Info($"GetPlayerInfo Catch: {e.Message}");
            }
        }

        internal static void GetOpponentInfo()
        {
            try
            {
                string result = Core.Game.Opponent.Name;
                Logger.Info($"GetOpponentInfo: {result}");
            }
            catch (Exception e)
            {
                Logger.Info($"GetOpponentInfo Catch: {e.Message}");
            }
        }

        internal static void GetAllHeroes()
        {
            try
            {
                Logger.Info($"--- Heroes ---");
                Core.Game.Entities.Values
                    .Where(x => x.HasTag(GameTag.PLAYER_LEADERBOARD_PLACE))
                    .ToList()
                    .ForEach(hero =>
                    {
                        if (hero.HasCardId)
                        {
                            var place = hero.GetTag(GameTag.PLAYER_LEADERBOARD_PLACE);

                            var card = Database.GetCardFromId(hero.CardId);
                            var name = card.Name;
                            var localizedName = card.LocalizedName;
                            var blizzardId = card.DbfIf;

                            Logger.Info($"Hero Name: {name} place: {place} LocalizedName: {localizedName} Id: {blizzardId}");
                        }
                        else
                        {
                            Logger.Info($"Hero CardId Not Found!");
                        }
                    });
                Logger.Info($"--- End ---");

                //Logger.Info($"GetAllEntities:\n{Serializer.toJson(result)}");
            }
            catch (Exception e)
            {
                Logger.Info($"GetAllEntities Catch: {e.Message}");
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
    }
}
