using VotingSystem.Utils;
using VotingSystem.Accessor;


namespace VotingSystem.Model
{
    public class Ballot
    {
        public string VoterSerial { get; }
        public string IssueSerial { get; }
        public int Choice { get; }
        public string SerialNumber { get; }

        public static readonly IBallotAccessor Accessor = new BallotAccessor();

        public Ballot(string voter, string issue, int choice, string serialNumber)
        {
            VoterSerial = voter;
            IssueSerial = issue;
            Choice = choice;
            SerialNumber = serialNumber;
        }

        public class BallotBuilder
        {
            public string? Voter;
            public string? Issue;
            public int Choice = -2;
            public string? SerialNumber;
            private bool _inputtedChoice;

            public BallotBuilder WithVoter(string voter)
            {
                Voter = voter;
                return this;
            }

            public BallotBuilder WithIssue(string issue)
            {
                Issue = issue;
                return this;
            }

            /// <summary>
            /// -1 for abstain, else 0..n
            /// </summary>
            public BallotBuilder WithChoice(int choiceNumber)
            {
                _inputtedChoice = true;
                Choice = choiceNumber;
                return this;
            }

            public BallotBuilder WithSerialNumber(string? serialNumber)
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
                if (SerialNumber == null || !Validation.IsValidSerialNumber(SerialNumber))
                    throw new InvalidBuilderParameterException("Invalid (null) SerialNumber");
                if (Choice < -1)
                    throw new InvalidBuilderParameterException("Invalid choice number");
                if (!_inputtedChoice)
                    throw new InvalidBuilderParameterException(
                        "BallotIssue choice not selected");
                Ballot ballot = new(Voter, Issue, Choice, SerialNumber);
                return ballot;
            }
        }
    }
}
