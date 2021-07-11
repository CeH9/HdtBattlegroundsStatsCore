using System.Collections.Generic;

namespace BgMatchResultRecorder.data
{
    internal class AppState
    {
        internal static MatchState matchState = null;

        internal static MatchState DefaultMatchState()
        {
            var state = new MatchState();
            //var player = new Player();
            //player.Hero = new Hero
            //{
            //    Id = "-",
            //    DbId = -1,
            //    TurnWhenCaptured = -1
            //};
            //player.Board = new Board { Minions = new List<Minion>() };

            //var invalidRace = new Race
            //{
            //    Id = (int)HearthDb.Enums.Race.INVALID,
            //    Name = HearthDb.Enums.Race.INVALID.ToString()
            //};

            //state.Player = player;
            //state.Place = -1;
            //state.WasConceded = false;
            //state.AvailableRaces = new List<Race> { invalidRace };
            //state.Rank = -1;

            return state;
        }
    }
}
