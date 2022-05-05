using VotingSystem.Model;
using VotingSystem.Accessor;

// Contatins an single user

namespace VotingSystem.Controller
{
    public class UserResultsViewer
    {
        //Contains a sinlge user's info (serial)
        //Might contain user's ballots (null if not alredy accessed)
        private string _currVoterSerial;                               // Logged-in voter's serial, null for general view
        private Dictionary<string, Ballot?>? _ballots;                  // Choice submitted by the current voter on each issue

        private static readonly IResultAccessor Accessor = new ResultAccessor();

        public UserResultsViewer(string userSerial)
        {
            _currVoterSerial = userSerial;
        }

        /// <summary>
        /// Retrieves all the subbmitted ballots of the current voter
        /// </summary>
        /// <returns>Map: issue-serial --> voter's ballot on issue </returns>
        /// <exception cref="ArgumentNullException"></exception>
        public Dictionary<string, Ballot?> GetBallots(List<BallotIssue> ballotIssues)
        {
            //Return cached
            if (_ballots != null)
                return _ballots;

            _ballots = new Dictionary<string, Ballot?>();
            foreach (var issue in ballotIssues)
            {
                var ballot = Ballot.Accessor.GetBallot(_currVoterSerial, issue.SerialNumber);
                _ballots.Add(issue.SerialNumber, ballot);
            }
            return _ballots;
        }
    }
}