using System.Collections.Generic;

namespace BgMatchResultRecorder.data
{
    internal class Hero
    {
        public string Id { get; set; }
        public int DbId { get; set; }
        public int TurnWhenCaptured { get; set; }
    }

    internal class Minion
    {
        public string Id { get; set; }
        public int DbId { get; set; }
        public int ZonePosition { get; set; }
        public int Health { get; set; }
        public int Attack { get; set; }
        public int TavernTier { get; set; }
        public bool IsGolden { get; set; }
        public int Poisonous { get; set; }
    }

    internal class Board
    {
        public List<Minion> Minions { get; set; }
        public int TurnWhenCaptured { get; set; }
    }

    internal class Player
    {
        public Hero Hero { get; set; }
        public Board Board { get; set; }
    }
    internal class Opponent
    {
        public Hero Hero { get; set; }
    }
    internal class Race
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    internal class MatchState
    {
        public Player Player { get; set; }
        //public Opponent opponent { get; set; }
        public int Place { get; set; }
        public bool IsConceded { get; set; }
        public List<Race> AvailableRaces { get; set; }
        //public int RankDiff{ get; set; }
        public int Rank { get; set; }

        // todo StartDate
        // todo EndDate
    }
}
