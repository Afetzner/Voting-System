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
        private List<Voter>? _allVoters;                                        // List of all voters
        private List<BallotIssue>? _issues;                                     // All ballot-issues
        private List<Ballot>? _votes;                                           // Votes submitted by the current voter
        private Dictionary<BallotIssueOption, int>? _results;                   // Vote counts for each issue option
        private Dictionary<BallotIssue, HashSet<Voter>>? _voterParticipation;   // Set of voters who participated in each issue
        
        ResultViewer(Voter loggedInVoter)
        {
            _currVoter = loggedInVoter;
        }

        /// <summary>
        /// Retrives all the issues from the DB (Already written ,only slight refactor)
        /// </summary>
        /// <returns></returns>
        public List<BallotIssue> GetBallotIssues()
        {
            if (_issues != null)
                return _issues;

            _issues = new List<BallotIssue>();
            //TODO
            //retrieve ballot issues and cache in _issues
            return _issues;
        }

        /// <summary>
        /// Retrieves all the subbmitted ballots of the current voter
        /// </summary>
        /// <param name="voter"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public List<Ballot> GetBallots(Voter voter = null)
        {
            if (_votes != null)
                return _votes;
            if (voter != null)
                _currVoter = voter;
            if (_currVoter == null)
                throw new ArgumentNullException("null voter arg w/out pre-assigned current voter");

            _votes = new List<Ballot>();
            //TODO
            //Assign query results to _votes
            return _votes;
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
        
        public Dictionary<BallotIssue,HashSet<Voter>> GetVoterParticipation()
        {
            if (_voterParticipation != null)
                return _voterParticipation;
            if (_issues == null)
                GetBallotIssues();
            _voterParticipation = new Dictionary<BallotIssue, HashSet<Voter>>();
            //TODO
            //Dictionary<string, Voter> votersDict
            //foreach (var issue in _issues)
            //   HashSet<voters> partipants = new()
            //   listOfVoterSerials = getVoterPartipation
            //   foreach(string voterserial in list)
            //        if voterserial not in votersDict
            //             voter = getVoter
            //             voterDict.Add(voter)
            //        partipants.add(voter)
            //        _voterParicpation.add(isse:particpants)
            return _voterParticipation;
        }
    }
}