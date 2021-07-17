using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace BgMatchResultRecorder.data
{
    internal class Hero
    {
        [JsonProperty]
        internal string Id { get; set; } = GameUtils.INVALID_ID;
        [JsonProperty]
        internal int DbId { get; set; } = GameUtils.INVALID_INT_ID;
        [JsonProperty]
        internal string Name { get; set; } = null;
        [JsonProperty]
        internal int? TurnWhenCaptured { get; set; } = null;

        [JsonProperty]
        internal string DebugMessage { get; set; } = null;
    }

    internal class Minion
    {
        [JsonProperty]
        internal string Id { get; set; } = GameUtils.INVALID_ID;
        [JsonProperty]
        internal int DbId { get; set; } = GameUtils.INVALID_INT_ID;
        [JsonProperty]
        internal string Name { get; set; } = null;
        [JsonProperty]
        internal int ZonePosition { get; set; } = GameUtils.INVALID_INT_ID;
        [JsonProperty]
        internal int Health { get; set; } = GameUtils.INVALID_INT_ID;
        [JsonProperty]
        internal int Attack { get; set; } = GameUtils.INVALID_INT_ID;
        [JsonProperty]
        internal int TavernTier { get; set; } = GameUtils.INVALID_INT_ID;
        [JsonProperty]
        internal bool IsGolden { get; set; } = false;
        [JsonProperty]
        internal bool Poisonous { get; set; } = false;
    }

    internal class Board
    {
        [JsonProperty]
        internal List<Minion> Minions { get; set; } = new List<Minion>();
        [JsonProperty]
        internal int? TurnWhenCaptured { get; set; } = null;

        [JsonProperty]
        internal string DebugMessage { get; set; } = null;
    }

    internal class Player
    {
        [JsonProperty]
        internal Hero Hero { get; set; } = new Hero();
        [JsonProperty]
        internal Board Board { get; set; } = new Board();        
    }
    internal class Opponent
    {
        [JsonProperty]
        internal Hero Hero { get; set; } = new Hero();
    }
    internal class Race
    {
        [JsonProperty]
        internal int Id { get; set; } = GameUtils.INVALID_INT_ID;
        [JsonProperty]
        internal string Name { get; set; } = null;
    }

    internal class AvailableRaces
    {
        [JsonProperty]
        internal List<Race> Races { get; set; } = new List<Race>();

        [JsonProperty]
        internal string DebugMessage { get; set; } = null;
    }

    internal class MatchState
    {
        [JsonProperty]
        internal Player Player { get; set; } = new Player();
        [JsonProperty]
        internal Player Opponent { get; set; } = new Player();
        [JsonProperty]
        internal int? Place { get; set; } = null;
        [JsonProperty]
        internal bool WasConceded { get; set; } = false;
        [JsonProperty]
        internal AvailableRaces AvailableRaces { get; set; } = new AvailableRaces();
        [JsonProperty]
        internal int? RatingDiff { get; set; } = null;
        [JsonProperty]
        internal int? Rating { get; set; } = null;  
        [JsonProperty]
        internal string Region{ get; set; } = null;
        [JsonProperty]
        internal int? LastPlayedTurn { get; set; } = null;
        [JsonProperty]
        internal DateTime? StartDateTime { get; set; } = null;
        [JsonProperty]
        internal DateTime? EndDateTime { get; set; } = null;
    }
}
