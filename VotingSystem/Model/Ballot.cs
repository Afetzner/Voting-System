using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VotingSystem;
using VotingSystem.Controller;

/*
This table represents a Ballot that has multiple votes (CandidateElections)
CREATE TABLE Ballot (
	BallotId int AUTO_INCREMENT,
    VoterId int,
    ElectionId int,
    PRIMARY KEY (BallotId),
	FOREIGN KEY(VoterId) REFERENCES Voter(VoterId),
	FOREIGN KEY(ElectionId) REFERENCES Election(ElectionId)
);
*/

namespace VotingSystem.Model
{
    public class Ballot
    {
        public Voter Voter { get; }
        public Election Election { get; }
        private List<CandidateElection> BallotIssueVotes = null;

        public List<CandidateElection> BallotIssueVotes
        {
            get { return BallotIssueVotes; }
        }
        /*when ballot is submitted, how is data processed?
        Each individual vote on each issue must be recorded in the database
        but only after the voter has reviewed and hit the final "submit" 
        button

        */

        public Ballot(int voterId, int electionId)
        {
            Voter = VoterController.GetInfo(voterId);
            Election = ElectionController.GetInfo(electionId);
            BallotIssueVotes = null;
        }
    }

        public static void addBallotIssueVote(CandidateElection ballotIssue)
        {
            BallotIssueVotes.Add(ballotIssue);
        }
    }
}
