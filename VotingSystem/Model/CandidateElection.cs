using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VotingSystem;
using VotingSystem.Controller;

namespace VotingSystem.Model
{
    /* This class represents a single vote on a single issue on a ballot
     * in an election by a voter */
    public class CandidateEelction
    {
        public Candidate Candidate{ get; }
        public Election Election { get; }
        public Ballot Ballot { get; }
        public int Vote { get; }

        public static IDbController<CandidateEelction> DbController = new CandidateElectionController();

        public CandidateEelction(int candidateId, int electionId, int ballotId, int vote)
        {
            Candidate = CandidateController.GetInfo(candidateId);
            Election = ElectionController.GetInfo(electionId);
            Ballot = BallotController.GetInfo(ballotId);
            Vote = vote;
        }
    }
}
