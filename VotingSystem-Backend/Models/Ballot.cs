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

        public class Builder
        {
            public string? Voter;
            public string? Issue;
            public int Choice = -2;
            public string? SerialNumber;
            private bool _inputtedChoice;

            public Builder WithVoter(string voter)
            {
                Voter = voter;
                return this;
            }

            public Builder WithIssue(string issue)
            {
                Issue = issue;
                return this;
            }

            /// <summary>
            /// -1 for abstain, else 0..n
            /// </summary>
            public Builder WithChoice(int choiceNumber)
            {
                _inputtedChoice = true;
                Choice = choiceNumber;
                return this;
            }

            public Builder WithSerialNumber(string? serialNumber)
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
                if (!Validation.IsValidSerialNumber(Voter))
                    throw new InvalidBuilderParameterException(@$"Invalid BallotVoter '{Voter}'");
                if (!Validation.IsValidSerialNumber(Issue))
                    throw new InvalidBuilderParameterException(@$"Invalid BallotIssue '{Issue}'");
                if (!Validation.IsValidSerialNumber(SerialNumber))
                    throw new InvalidBuilderParameterException(@$"Invalid SerialNumber '{SerialNumber}'");
                if (!_inputtedChoice)
                    throw new InvalidBuilderParameterException(
                        "BallotIssue choice not selected");
                if (Choice < -1)
                    throw new InvalidBuilderParameterException("Invalid choice number");
                Ballot ballot = new(Voter, Issue, Choice, SerialNumber);
                return ballot;
            }
        }
    }
}
