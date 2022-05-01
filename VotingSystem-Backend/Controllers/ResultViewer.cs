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
        private Voter? _currVoter;                                              // Logged-in voter, null for general view
        private List<BallotIssue>? _issues;                                     // All ballot-issues
        private List<Ballot>? _ballots;                                         // Votes submitted by the current voter
        private Dictionary<BallotIssueOption, int>? _results;                   // Vote counts for each issue option
        private Dictionary<BallotIssue, List<Voter>>? _voterParticipation;      // List of voters who participated in each issue

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
        public List<Ballot> GetBallots(Voter voter)
        {
            if (_ballots != null)
                return _ballots;
            _currVoter = voter;
            if (_currVoter == null)
                throw new ArgumentNullException("null voter arg w/out pre-assigned current voter");

            _ballots = new List<Ballot>();
            //Assign query results to _votes
           _votes = Ballot.Accessor.GetBallotsByVoter(voter.SerialNumber);
            return _ballots;
        }

        //Not sure how best to return results
        //Ideally the results should be somehow organized by issue
        // Perhaps another function that returns results for a paricular issue or 
        // packages them up? (it would call on this)
        /// <summary>
        /// Retrieves the poll results of every issue from the DB
        /// </summary>
        /// <returns></returns>
        public Dictionary<BallotIssueOption, int> GetPollResults()
        {
            if (_results != null)
                return _results;
            if (_issues == null)
                GetBallotIssues();
            _results = new Dictionary<BallotIssueOption, int>();

            //TODO
            //Get results for each issue
            //foreach(var issue in _issues) 
            //   get issue results...
            //   foreach (option in issue.options)
            //      _results.Add(issue, voteCount)
            return _results;

        }
        
        /// <summary>
        /// Retrieves a list of voters who participated in *each* election
        /// </summary>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public Dictionary<BallotIssue,List<Voter>> GetVoterParticipation()
        {
            if (_voterParticipation != null)
                return _voterParticipation;
            if (_issues == null)
                GetBallotIssues();
            if (_issues == null)
                throw new ArgumentNullException();
            
            _voterParticipation = new Dictionary<BallotIssue, List<Voter>>();

            foreach (var issue in _issues)
            {
                List<Voter> issueParticipants = ResultAccessor.GetVoterParticipation(issue);
                _voterParticipation.Add(issue, issueParticipants);
            }
            return _voterParticipation;
        }
    }
}
