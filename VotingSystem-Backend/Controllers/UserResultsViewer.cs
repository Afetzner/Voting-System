using VotingSystem.Model;
// Contatins an single user

namespace VotingSystem.Controller
{
    public class UserResultsViewer
    {
        //Contains a sinlge user's info (serial)
        //Might contain user's ballots (null if not alredy accessed)
        private string? _currVoterSerial;                               // Logged-in voter's serial, null for general view
        private Dictionary<string, Ballot?>? _ballots;                  // Choice submitted by the current voter on each issue

        public UserResultsViewer(string userSerial)
        {
            _currVoterSerial = userSerial;
        }

        /// <summary>
        /// Retrieves all the subbmitted ballots of the current voter
        /// </summary>
        /// <returns>Map: issue-serial --> voter's ballot on issue </returns>
        /// <exception cref="ArgumentNullException"></exception>
        public Dictionary<string, Ballot?> GetBallots()
        {
            throw new NotImplementedException();
        }
    }
}