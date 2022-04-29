using VotingSystem.Utils;
using static System.String;

namespace VotingSystem.Model
{
    public class BallotIssueOption
    {
        public int Number { get; }
        public string Title { get; }

        public BallotIssueOption(int number, string title)
        {
            Number = number;
            Title = title;
        }

        public class BallotIssueOptionBuilder
        {
            public int Number = -1;
            public string? Title;
            
            public BallotIssueOptionBuilder WithOptionNumber(int number)
            {
                Number = number;
                return this;
            }

            public BallotIssueOptionBuilder WithTitle(string? title)
            {
                Title = title;
                return this;
            }

            public BallotIssueOption Build()
            {
                if (Number < 0)
                    throw new InvalidBuilderParameterException("Invalid option number '" + Number + "'");
                if (IsNullOrWhiteSpace(Title))
                    throw new InvalidBuilderParameterException("Invalid option title '" + Title + "'");
                BallotIssueOption option = new(Number, Title);
                return option;
            }
        }

    }
}