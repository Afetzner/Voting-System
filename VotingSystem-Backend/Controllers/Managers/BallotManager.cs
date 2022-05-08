using VotingSystem.Model;

namespace VotingSystem.Controller
{
    /// <summary>
    /// Routes API requests to respective accessor and updates cache
    /// </summary>
    public static class BallotManager 
    {
        private static readonly ResultCacheManager _cache = ResultCacheManager.SharedCacheManager;

        public static bool AddBallot(Ballot ballot)
        {
            bool coll = Ballot.Accessor.AddBallot(ballot);
            _cache.ForgetUserCache(ballot.VoterSerial);
            _cache.ForgetCachedResults(ballot.IssueSerial);
            return coll;
        }

        public static Ballot? GetBallot(string voterSerial, string issueSerial)
        {
            return Ballot.Accessor.GetBallot(voterSerial, issueSerial);
        }

        public static void RemoveBallot(string serial)
        {
            Ballot.Accessor.RemoveBallot(serial);
        }

        public static bool IsSerialInUse(string ballotSerial)
        {
            return Ballot.Accessor.IsSerialInUse(ballotSerial);
        }

        public static string GetSerial()
        {
            return Ballot.Accessor.GetSerial();
        }
    }
}