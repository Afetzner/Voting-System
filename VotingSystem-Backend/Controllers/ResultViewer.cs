using System;
using VotingSystem.Model;
using VotingSystem.Accessor;

namespace VotingSystem.Controller
{
    public class ResultViewer
    {
        //Each of these are assigned ON-DEMAND
        //I.e. we don't want to grab everything in the constructer, 
        //Fill it out as needed and cache it
        private Voter? _currVoter;                                      // Logged-in voter, null for general view
        private List<BallotIssue>? _issues;                             // All ballot-issues
        private Dictionary<string, int?>? _ballots;                     // Choice submitted by the current voter on each issue
        private Dictionary<string, Dictionary<int, int>>? _results;     // Vote counts for each issue option
        private Dictionary<string, List<Voter>>? _voterParticipation;   // List of voters who participated in each issue
        
        private static readonly IResultAccessor Accessor = new ResultAccessor();

        /// <summary>
        /// Retrives all the issues from the DB (Already written ,only slight refactor)
        /// </summary>
        /// <returns></returns>
        public List<BallotIssue> GetBallotIssues()
        {
            if (_issues != null)
                return _issues;

            _issues = new List<BallotIssue>();
            //retrieve ballot issues and cache in _issues
            _issues = BallotIssue.Accessor.GetBallotIssues();

            return _issues;
        }

        /// <summary>
        /// Retrieves all the subbmitted ballots of the current voter
        /// </summary>
        /// <param name="voter"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public Dictionary<string, int?> GetBallots(Voter voter)
        {
            if (_ballots != null)
                return _ballots;
            _currVoter = voter;
            if (_currVoter == null)
                throw new ArgumentNullException("null voter arg w/out pre-assigned current voter");
            if (_issues == null)
                GetBallotIssues();
            if (_issues == null)
                throw new ArgumentNullException();

            _ballots = new Dictionary<string, int?>();
            //Assign query results to _votes
           foreach (var issue in _issues)
            {
                var ballot = Ballot.Accessor.GetBallot(_currVoter.SerialNumber, issue.SerialNumber);
                if (ballot == null)
                    _ballots.Add(issue.SerialNumber, null);
                else
                    _ballots.Add(issue.SerialNumber, ballot.Choice);
            }
            return _ballots;
        }

        /// <summary>
        /// Retrieves the poll results of every issue from the DB
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, Dictionary<int, int>> GetPollResults()
        {
            if (_results != null)
                return _results;
            if (_issues == null)
                GetBallotIssues();
            if (_issues == null)
                throw new ArgumentNullException();
            _results = new Dictionary<string, Dictionary<int, int>>();

            foreach (var issue in _issues)
            {
                var issueResults = Accessor.GetIssueResults(issue.SerialNumber);
                _results.Add(issue.SerialNumber, issueResults);
            }
            return _results;
        }
        
        /// <summary>
        /// Retrieves a list of voters who participated in *each* election
        /// </summary>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public Dictionary<string,List<Voter>> GetVoterParticipation()
        {
            if (_voterParticipation != null)
                return _voterParticipation;
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
