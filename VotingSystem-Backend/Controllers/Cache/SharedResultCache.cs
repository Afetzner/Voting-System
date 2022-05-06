using VotingSystem.Model;
using VotingSystem.Accessor;

namespace VotingSystem.Controller
{
    /// <summary>
    /// Responsible for getting and cacheing results not specific to users
    /// ex. to get the issue results, you would need the issues,
    ///     you would likely already have gotten the issues.
    ///     Instead of re-asking for the issues, store the issues the 
    ///     first time you get them, use them later.
    /// </summary>
    public class SharedResultCache
    {
        //Each of these are assigned ON-DEMAND
        //I.e. we don't want to grab everything in the constructer, 
        //Fill it out as needed and cache it
        private Dictionary<string, BallotIssue>? _issues;                // All ballot-issues
        private Dictionary<string, Dictionary<int, int>?>? _results;     // Vote counts for each issue option
        private Dictionary<string, List<Voter>?>? _voterParticipation;   // List of voters who participated in each issue
        
        private static readonly IResultAccessor Accessor = new ResultAccessor();

        /// <summary>
        /// Retrives all the issues from the DB
        /// </summary>
        /// <returns>List of issues (fixed order)</returns>
        public Dictionary<string, BallotIssue> GetCacheBallotIssues()
        {
            //Return cache
            if (_issues != null)
                return _issues;

            //Get and cache issues
            _issues = new Dictionary<string, BallotIssue>();
            var issuesLst = Accessor.GetBallotIssues();
            foreach (var issue in issuesLst)
                _issues.Add(issue.SerialNumber, issue);
            return _issues;
        }

        /// <summary>
        /// Retrieves the poll results of every issue from the DB
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, Dictionary<int, int>> GetCacheResults()
        {
            //Get cached issues if exists else from db
            _issues = GetCacheBallotIssues();

            // Batch get all issue results
            if (_results == null)
#pragma warning disable CS8619 // Nullability of reference types in value doesn't match target type.
                _results = Accessor.GetAllResults(_issues.Values.ToList());
#pragma warning restore CS8619 // Nullability of reference types in value doesn't match target type.
                              //Get issue results only for forgoten results
            else
                foreach (var issue in _issues.Keys)
                    if (!_results.ContainsKey(issue))
                        _results.Add(issue, Accessor.GetIssueResults(issue));

            //All values *should* be non-null after above loop
#pragma warning disable CS8619 // Nullability of reference types in value doesn't match target type.
            return _results;
#pragma warning restore CS8619 // Nullability of reference types in value doesn't match target type.
        }
        
        /// <summary>
        /// Retrieves a list of voters who participated in *each* election
        /// </summary>
        /// <returns>Map 'issue-serial' --> list of participating voters</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public Dictionary<string,List<Voter>> GetCacheVoterParticipation()
        {
            //Get cached issues if exists else from db
            _issues = GetCacheBallotIssues();
            
            if (_voterParticipation == null)
                _voterParticipation = new Dictionary<string, List<Voter>?>();

            //Get cached particpation if exist, otherwise from db
            foreach (var issue in _issues.Keys)
            {
                //Get issue particpation if not cached
                if (!_voterParticipation.ContainsKey(issue))
                {
                    List<Voter> issueParticipants = Accessor.GetVoterParticipation(issue);
                    _voterParticipation.Add(issue, issueParticipants);
                }
            }
            //All values *should* be non-null after above loop
#pragma warning disable CS8619 // Nullability of reference types in value doesn't match target type.
            return _voterParticipation;
#pragma warning restore CS8619 // Nullability of reference types in value doesn't match target type.
        }
    
        public void ForgetCachedIssues()
        {
            //Forgeting issues requires forgetting results and participation too
            _results = null;
            _voterParticipation = null;
            _issues = null;
        }

        public void ForgetCachedResults(string issueSerial)
        {
            if (_results == null)
                return;
            if (_results.ContainsKey(issueSerial))
                _results.Remove(issueSerial);
        }

        public void ForgetCachedVoterParticipation(string issueSerial)
        {
            if (_voterParticipation == null)
                return;
            if (_voterParticipation.ContainsKey(issueSerial))
                _voterParticipation.Remove(issueSerial);
        }
    }
}
