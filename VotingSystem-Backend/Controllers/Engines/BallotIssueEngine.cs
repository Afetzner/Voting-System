using VotingSystem.Model;

namespace VotingSystem.Controller
{
    /// <summary>
    /// Routes API requests to respective accessor and updates cache
    /// </summary>
    public static class BallotIssueEngine
    {
        private static ResultCacheEngine _cacheManager = ResultCacheEngine.SharedCacheManager;

        public static bool AddIssue(BallotIssue issue)
        {
            bool coll = BallotIssue.Accessor.AddIssue(issue);
            _cacheManager.ForgetCachedIssues();
            return coll;
        }

        public static void RemoveIssue(string serial)
        {
            BallotIssue.Accessor.RemoveIssue(serial);
            _cacheManager.ForgetCachedIssues();
        }

        public static bool IsSerialInUse(string serial)
        {
            return BallotIssue.Accessor.IsSerialInUse(serial);
        }

        public static string GetSerial()
        {
            return BallotIssue.Accessor.GetSerial();
        }
    }
}