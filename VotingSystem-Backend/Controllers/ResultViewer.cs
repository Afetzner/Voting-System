using System;
using VotingSystem.Model;
using VotingSystem.Accessor;

namespace VotingSystem.Controller
{
    /// <summary>
    /// Responsible for getting and cacheing results from quieres
    /// ex. to get the issue results, you would need the issues,
    ///     you would likely already have gotten the issues.
    ///     Instead of re-asking for the issues, store the issues the 
    ///     first time you get them, use them later.
    /// </summary>
    public class ResultViewer
    {
        //Each of these are assigned ON-DEMAND
        //I.e. we don't want to grab everything in the constructer, 
        //Fill it out as needed and cache it
        private string? _currVoterSerial;                               // Logged-in voter's serial, null for general view
        private List<BallotIssue>? _issues;                             // All ballot-issues
        private Dictionary<string, Ballot?>? _ballots;                  // Choice submitted by the current voter on each issue
        private Dictionary<string, Dictionary<int, int>>? _results;     // Vote counts for each issue option
        private Dictionary<string, List<Voter>>? _voterParticipation;   // List of voters who participated in each issue
        
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
            _issues = BallotIssue.Accessor.GetBallotIssues();

            return _issues;
        }

        /// <summary>
        /// Retrieves all the subbmitted ballots of the current voter
        /// </summary>
        /// <returns>Map: issue-serial --> voter's ballot on issue </returns>
        /// <exception cref="ArgumentNullException"></exception>
        public Dictionary<string, Ballot?> GetBallots(string voterSerial)
        {
            //Return cached
            if (_ballots != null)
                return _ballots;
            
            //Get and cache voter's ballots
            _currVoterSerial = voterSerial;
            if (_currVoterSerial == null)
                throw new ArgumentNullException("null curr voter");
            if (_issues == null)
                GetBallotIssues();
            if (_issues == null)
                throw new ArgumentNullException("null issues");

            _ballots = new Dictionary<string, Ballot?>();
            foreach (var issue in _issues)
            {
                var ballot = Ballot.Accessor.GetBallot(_currVoterSerial, issue.SerialNumber);
                _ballots.Add(issue.SerialNumber, ballot);
                }
            return _ballots;
        }

        /// <summary>
        /// Retrieves the poll results of every issue from the DB
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, Dictionary<int, int>> GetResults()
        {
            // Return cache
            if (_results != null)
                return _results;

            //Get and cache poll results
            if (_issues == null)
                GetBallotIssues();
            if (_issues == null)
                throw new ArgumentNullException("null issues");

            _results = Accessor.GetResults(_issues);
            return _results;
        }
        
        /// <summary>
        /// Retrieves a list of voters who participated in *each* election
        /// </summary>
        /// <returns>Map 'issue-serial' --> list of participating voters</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public Dictionary<string,List<Voter>> GetVoterParticipation()
        {
            //Return cache
            if (_voterParticipation != null)
                return _voterParticipation;

            //Get and cahce voter participation
            if (_issues == null)
                GetBallotIssues();
            if (_issues == null)
                throw new ArgumentNullException();
            
            _voterParticipation = new Dictionary<string, List<Voter>>();
            foreach (var issue in _issues)
            {
                List<Voter> issueParticipants = Accessor.GetVoterParticipation(issue.SerialNumber);
                _voterParticipation.Add(issue.SerialNumber, issueParticipants);
            }
            return _voterParticipation;
        }
    }
}
