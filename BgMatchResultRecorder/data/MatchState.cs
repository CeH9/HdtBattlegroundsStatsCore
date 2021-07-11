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
        internal int TurnWhenCaptured { get; set; } = 0;
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

        [JsonProperty]
        internal string DebugMessage { get; set; } = null;
    }

    internal class Board
    {
        [JsonProperty]
        internal List<Minion> Minions { get; set; } = new List<Minion>();
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

    internal class MatchState
    {
        [JsonProperty]
        internal Player Player { get; set; } = new Player();
        //internal Opponent opponent { get; set; }
        [JsonProperty]
        internal int Place { get; set; } = -1;
        [JsonProperty]
        internal bool WasConceded { get; set; } = false;
        [JsonProperty]
        internal List<Race> AvailableRaces { get; set; } = new List<Race>();
        //internal int RankDiff{ get; set; }
        [JsonProperty]
        internal int Rank { get; set; } = -1;
        [JsonProperty]
        internal int LastPlayedTurn { get; set; } = -1;
        [JsonProperty]
        internal DateTime? StartDateTime { get; set; } = null;
        [JsonProperty]
        internal DateTime? EndDateTime { get; set; } = null;
    }
}
