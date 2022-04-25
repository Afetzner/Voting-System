using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VotingSystem;
using VotingSystem.Controller;
using VotingSystem.Utils;


namespace VotingSystem.Model
{
    public class Ballot
    {
        public Voter Voter { get; }
        public BallotIssue Issue { get; }
        public BallotIssueOption Choice { get; }

        /*when ballot is submitted, how is data processed?
        Each individual vote on each issue must be recorded in the database
        but only after the voter has reviewed and hit the final "submit" 
        button

        */

        public Ballot(Voter voter, BallotIssue issue, BallotIssueOption choice)
        {
            Voter = voter;
            Issue = issue;
            Choice = choice;
        }
    }

        /* Moot: ballots can now only have one vote
         
         * access database to retrieve all votes associated with a ballot and return a
         * List of votes from a particular ballot
        public List<CandidateElection> getBallotVotes(int ballotId, List<CandidateElection> listBallotVotes)
        {
            for ( )
            {
                ListBallotVotes.Add()
            }
        }
        */

        public class BallotBuilder
        {
            public Voter Voter = null;
            public BallotIssue Issue = null;
            public BallotIssueOption Choice = null;
            private bool _inputtedChoice = false;

            public BallotBuilder WithVoter(Voter voter)
            {
                Voter = voter;
                return this;
            }

            public BallotBuilder WithIssue(BallotIssue issue)
            {
                Issue = issue;
                return this;
            }

            public BallotBuilder WithChoice(BallotIssueOption choice)
            {
                _inputtedChoice = true;
                Choice = choice;
                return this;
            }

            public Ballot Build()
            {
                if (Voter == null)
                    throw new InvalidBuilderParameterException("Invalid (Null) BallotVoter");
                if (Issue == null)
                    throw new InvalidBuilderParameterException("Invalid (null) BallotIssue");
                if (!_inputtedChoice)
                    throw new InvalidBuilderParameterException(
                        "Invalid choice (Null allowed, but must be inputted: WithChoice(Null))");
                Ballot ballot = new (Voter, Issue, Choice);
                return ballot;
            }
        }
}
