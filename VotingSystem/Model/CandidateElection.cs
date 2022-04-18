using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VotingSystem;
using VotingSystem.Controller;

    namespace VotingSystem.Model
{
    public class CandidateElection
    {
        public int CandidateId { get; }
        public int ElectionId { get; }
        public int BallotId { get; }
        public int Vote { get; }

		public CandidateElection(int candidateId, int electionId, int ballotId, int vote)
		{
			CandidateId = candidateId;
			ElectionId = electionId;
			BallotId = ballotId;
			Vote = vote;
		}
    }
	public class CandidateElectionBuilder
	{
		private int CandidateId = -1;
		private int ElectionId = -1;
		private int BallotId = -1;
		private int Vote = -1;

		public CandidateElectionBuilder() { }

		public CandidateElectionBuilder WithCandidateId(int candidateId)
		{
			CandidateId = candidateId;
			return this;
		}

		public CandidateElectionBuilder WithElectionId(int electionId)
		{
			ElectionId = electionId;
			return this;
		}

		public CandidateElectionBuilder WithBallotId(int ballotId)
		{
			BallotId = ballotId;
			return this;
		}

		public CandidateElectionBuilder WithVote(int vote)
		{
			Vote = vote;
			return this;
		}

		public CandidateElection Build()
		{
			CandidateElection candidateElection = new CandidateElection(CandidateId, ElectionId, BallotId, Vote);
			return candidateElection;
		}
	}
}