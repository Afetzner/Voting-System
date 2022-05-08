using VotingSystem.Model;

namespace VotingSystem.Controller
{
    /// <summary>
    /// Responsible for routing requests for results from the cache 
    /// through either the SharedCache (results not specific to users)
    /// or through the respective UserCache (a user's ballots)
    /// </summary>
    public class ResultCacheEngine
    {
        public static readonly ResultCacheEngine SharedCacheManager = new();
        private static readonly SharedResultCache SharedCache = new();
        private static readonly Dictionary<string, UserResultCache> UserChaches = new();
        private static bool _halt = false;
        
        //Routed to shared cache
        public Dictionary<string, BallotIssue> GetIssues()
        {
            return SharedCache.GetCacheBallotIssues();
        }

        public Dictionary<string, Dictionary<int, int>> GetResults()
        {
            return SharedCache.GetCacheResults();
        }

        public Dictionary<string, List<Voter>> GetVoterParticipation()
        {
            return SharedCache.GetCacheVoterParticipation();
        }

        //Routed to user caches
        public Dictionary<string, Ballot?> GetBallots(string voterSerial)
        {
            while (_halt)
                Thread.Sleep(100);
            
            if (!UserChaches.ContainsKey(voterSerial))
            {
                _halt = true; //Halt other threads
                SpawnUserCache(voterSerial);
            }

            var issues = SharedCache.GetCacheBallotIssues().Values.ToList();
            var result = UserChaches[voterSerial].GetCacheBallots(ref issues);
            
            _halt = false; //Resume other threads

            return result;
        }

        /// <summary>
        /// Create a new instance of a User cache
        /// </summary>
        private static void SpawnUserCache(string voterSerial)
        {
            Console.WriteLine($@"Adding user {voterSerial} to cached users");
            UserResultCache userViewer = new(voterSerial);
            if (UserChaches.ContainsKey(voterSerial))
                return;
            UserChaches.Add(voterSerial, userViewer);
        }

        //Forgetters
        //Need to be called when a cached value become obsolete
        //i.e. a vote is added to an issue => forget the results for that issue

        //Call when a user logs in (user data needs to be refreshed)
        public void ForgetUserCache(string voterSerial)
        {
            Console.WriteLine($@"Forgetting cached user {voterSerial}");
            UserChaches.Remove(voterSerial);
        }

        //Call when an issue is added or removed (not when ballots are submitted) 
        public void ForgetCachedIssues()
        {
            SharedCache.ForgetCachedIssues();
        }

        //Call when a ballot is added or removed for an issue
        public void ForgetCachedResults(string issueSerial)
        {
            SharedCache.ForgetCachedResults(issueSerial);
        }
    
        //Call when a ballot is added or removed from an issue
        public void ForgetCachedParticipation(string issueSerial)
        {
            SharedCache.ForgetCachedVoterParticipation(issueSerial);
        }
    }
}
