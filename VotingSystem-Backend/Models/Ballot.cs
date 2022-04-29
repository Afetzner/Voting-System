using VotingSystem.Utils;
using VotingSystem.Accessor;


namespace VotingSystem.Model
{
    public class Ballot
    {
        public Voter Voter { get; }
        public BallotIssue Issue { get; }
        public BallotIssueOption Choice { get; }
        public String SerialNumber { get; }

        public static readonly IBallotAccessor Accessor = new BallotAccessor();

        public Ballot(Voter voter, BallotIssue issue, BallotIssueOption choice, String serialNumber)
        {
            Voter = voter;
            Issue = issue;
            Choice = choice;
            SerialNumber = serialNumber;
        }
    }

        public class BallotBuilder
        {
            public Voter? Voter;
            public BallotIssue? Issue;
            public BallotIssueOption? Choice;
            public String? SerialNumber;
            private bool _inputtedChoice;

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

            public BallotBuilder WithSerialNumber(String serialNumber)
            {
                SerialNumber = serialNumber;
                return this;
            }

            public Ballot Build()
            {
                if (Voter == null)
                    throw new InvalidBuilderParameterException("Invalid (Null) BallotVoter");
                if (Issue == null)
                    throw new InvalidBuilderParameterException("Invalid (null) BallotIssue");
                if (SerialNumber == null)
                    throw new InvalidBuilderParameterException("Invalid (null) SerialNumber");
                if (!_inputtedChoice || Choice == null)
                    throw new InvalidBuilderParameterException(
                        "Invalid BallotIssue choice (Null allowed, but must be inputted: WithChoice(null))");
                Ballot ballot = new (Voter, Issue, Choice, SerialNumber);
                return ballot;
            }
        }
}
