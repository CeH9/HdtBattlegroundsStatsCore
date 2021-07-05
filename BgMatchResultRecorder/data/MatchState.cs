namespace BgMatchResultRecorder.data
{
    public class Hero
    {   
        public string Id { get; set; }
        public int DbId { get; set; }
        public string Name { get; set; }
}

    public class MatchState
    {
        public static MatchState instance = null;

        public Hero LastOpponentHero { get; set; }
    }
}
