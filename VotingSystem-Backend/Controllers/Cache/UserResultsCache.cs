using VotingSystem.Model;

namespace VotingSystem.Controller
{
    /// <summary>
    /// Responsible for retriving and caching results specific to a user
    /// Currently only the user's ballots
    /// </summary>
    public class UserResultsCache
    {
        private readonly string _currVoterSerial;              // Logged-in voter's serial, null for general view
        private Dictionary<string, Ballot?>? _ballots;         // Choice submitted by the current voter on each issue

        public UserResultsCache(string userSerial)
        {
            _currVoterSerial = userSerial;
        }

        /// <summary>
        /// Retrieves all the subbmitted ballots of the current voter
        /// </summary>
        /// <returns>Map: issue-serial --> voter's ballot on issue </returns>
        /// <exception cref="ArgumentNullException"></exception>
        public Dictionary<string, Ballot?> GetCacheBallots(ref List<BallotIssue> issues)
        {
            //Return cached
            if (_ballots != null)
                return _ballots;

            //Get and cache voter's ballots
            _ballots = new Dictionary<string, Ballot?>();
            foreach (var issue in issues)
            {
                var ballot = Ballot.Accessor.GetBallot(_currVoterSerial, issue.SerialNumber);
                _ballots.Add(issue.SerialNumber, ballot);
            }
            return _ballots;
        }
    }
}