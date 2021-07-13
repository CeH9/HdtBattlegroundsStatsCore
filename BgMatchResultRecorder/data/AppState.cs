
namespace BgMatchResultRecorder.data
{
    internal class AppState
    {
        internal static MatchState matchState = null;

        internal static int? lastKnownRating;
        internal static string lastKnownRegion = null;

        internal static string lastFinishedMatchId = null;
        internal static string nextMatchId = GameUtils.GetRandomUniqueId();

        internal static VersionedBox<AvailableRaces> lastKnownAvailableRaces = new VersionedBox<AvailableRaces>();
    }

    internal class VersionedBox<T>
    {
        internal T Value { get; set; }

        internal string Version { get; set; } = null;

        // If Versions differs than current value is from old match
        internal bool ShouldUpdate()
        {
            return Version != AppState.nextMatchId;
        }

        internal static VersionedBox<T> From(T value)
        {
            return new VersionedBox<T> { Value = value, Version = AppState.nextMatchId };
        }
    }
}
