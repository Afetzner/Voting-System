using VotingSystem.Accessor;

namespace VotingSystem.Model
{
    public class BallotAccessor : IBallotAccessor
    {
        //TODO
        public bool AddBallot(Ballot ballot)
        {
            throw new NotImplementedException();
        }

        public List<Ballot> GetBallotsByVoter(string voterSerial)
        {
            throw new NotImplementedException();
        }

        public List<BallotIssue> GetIssuesVotedOn(string voterSerial)
        {
            throw new NotImplementedException();
        }

        public bool IsSerialInUse(string ballotSerial)
        {
            throw new NotImplementedException();
        }

        public void RemoveBallot(string serial)
        {
            throw new NotImplementedException();
        }
    }
}