using System.Collections.Generic;

namespace BgMatchResultRecorder.data
{
    internal class Hero
    {
        internal string Id { get; set; }
        internal int DbId { get; set; }
        internal int TurnWhenCaptured { get; set; }
    }

    internal class Minion
    {
        internal string Id { get; set; }
        internal int DbId { get; set; }
        internal int ZonePosition { get; set; }
        internal int Health { get; set; }
        internal int Attack { get; set; }
        internal int TavernTier { get; set; }
        internal bool IsGolden { get; set; }
        internal int Poisonous { get; set; }
    }

    internal class Board
    {
        internal List<Minion> Minions { get; set; }
        internal int TurnWhenCaptured { get; set; }
    }

    internal class Player
    {
        internal Hero Hero { get; set; }
        internal Board Board { get; set; }
    }
    internal class Opponent
    {
        internal Hero Hero { get; set; }
    }
    internal class Race
    {
        internal int Id { get; set; }
        internal string Name { get; set; }
    }

    internal class MatchState
    {
        internal Player Player { get; set; }
        //internal Opponent opponent { get; set; }
        internal int Place { get; set; }
        internal bool IsConceded { get; set; }
        internal List<Race> AvailableRaces { get; set; }
        //internal int RankDiff{ get; set; }
        internal int Rank { get; set; }

        // todo StartDate
        // todo EndDate
    }
}
