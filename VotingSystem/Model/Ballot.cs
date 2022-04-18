using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VotingSystem;
using VotingSystem.Controller;


namespace VotingSystem.Model
{
    public class Ballot
    {
        public int VoterId { get; }
        public int ElectionId { get; }
        public List<CandidateElection> ListBallotVotes { get; }

        /*when ballot is submitted, how is data processed?
        Each individual vote on each issue must be recorded in the database
        but only after the voter has reviewed and hit the final "submit" 
        button

        */

        public Ballot(int voterId, int electionId)
        {
            VoterId = voterId;
            ElectionId = electionId;
            ListBallotVotes = null;
        }
    }

        /*access database to retrieve all votes associated witha ballot and return a
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
            public int VoterId = -1;
            public int ElectionId = -1;
            public List<CandidateElection> ListBallotVotes = null;

            public BallotBuilder WithVoter(int voterId)
            {
                VoterId = voterId;
                return this;
            }

            public BallotBuilder WithElection(int electionId)
            {
                ElectionId = electionId;
                return this;
            }

            public BallotBuilder WithListBallotVotes(List<CandidateElection> listBallotVotes)
            {
                ListBallotVotes = listBallotVotes;
                return this;
            }

            public Ballot Build()
            {
                Ballot ballot = new Ballot(VoterId, ElectionId);
                return ballot;
            }
        }
}
