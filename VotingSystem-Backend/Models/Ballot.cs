using VotingSystem.Controller;
using VotingSystem.Utils;


namespace VotingSystem.Model
{
    public class Ballot
    {
        public Voter Voter { get; }
        public BallotIssue Issue { get; }
        public BallotIssueOption Choice { get; }

        public static BallotIssueController Controller = new ();

        public Ballot(Voter voter, BallotIssue issue, BallotIssueOption choice)
        {
            Voter = voter;
            Issue = issue;
            Choice = choice;
        }
    }

        public class BallotBuilder
        {
            public Voter Voter;
            public BallotIssue Issue;
            public BallotIssueOption Choice;
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

            public Ballot Build()
            {
                if (Voter == null)
                    throw new InvalidBuilderParameterException("Invalid (Null) BallotVoter");
                if (Issue == null)
                    throw new InvalidBuilderParameterException("Invalid (null) BallotIssue");
                if (!_inputtedChoice)
                    throw new InvalidBuilderParameterException(
                        "Invalid BallotIssue choice (Null allowed, but must be inputted: WithChoice(null))");
                Ballot ballot = new (Voter, Issue, Choice);
                return ballot;
            }
        }
}
