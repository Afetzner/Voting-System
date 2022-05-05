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
        private List<BallotIssue>? _issues;                              // All ballot-issues
        private Dictionary<string, Dictionary<int, int>?>? _results;     // Vote counts for each issue option
        private Dictionary<string, List<Voter>?>? _voterParticipation;   // List of voters who participated in each issue
        
        private static readonly IResultAccessor Accessor = new ResultAccessor();

        /// <summary>
        /// Retrives all the issues from the DB
        /// </summary>
        /// <returns>List of issues (fixed order)</returns>
        public List<BallotIssue> GetBallotIssues()
        {
            //Return cache
            if (_issues != null)
                return _issues;

            //Get and cache issues
            _issues = new List<BallotIssue>();
            _issues = Accessor.GetBallotIssues();

            return _issues;
        }

        /// <summary>
        /// Retrieves the poll results of every issue from the DB
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, Dictionary<int, int>> GetResults()
        {
            //Get cached issues if exists else from db
            _issues = GetBallotIssues();

            // Batch get all issue results
            if (_results == null)
#pragma warning disable CS8619 // Nullability of reference types in value doesn't match target type.
                _results = Accessor.GetAllResults(_issues);
#pragma warning restore CS8619 // Nullability of reference types in value doesn't match target type.
                              //Get issue results only for forgoten results
            else
                foreach (var issue in _issues)
                    if (!_results.ContainsKey(issue.SerialNumber))
                        _results.Add(issue.SerialNumber, Accessor.GetIssueResults(issue.SerialNumber));

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
        public Dictionary<string,List<Voter>> GetVoterParticipation()
        {
            //Get cached issues if exists else from db
            _issues = GetBallotIssues();
            
            if (_voterParticipation == null)
                _voterParticipation = new Dictionary<string, List<Voter>?>();

            //Get cached particpation if exist, otherwise from db
            foreach (var issue in _issues)
            {
                //Get issue particpation if not cached
                if (!_voterParticipation.ContainsKey(issue.SerialNumber))
                {
                    List<Voter> issueParticipants = Accessor.GetVoterParticipation(issue.SerialNumber);
                    _voterParticipation.Add(issue.SerialNumber, issueParticipants);
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
