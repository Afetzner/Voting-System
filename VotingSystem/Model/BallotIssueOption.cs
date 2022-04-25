using System;
using System.Threading;
using VotingSystem.Utils;

namespace VotingSystem.Model
{
    public class BallotIssueOption
    {

        private int Number { get; set; }
        private string Title { get; }

        BallotIssueOption(int number, string title)
        {
            Number = number;
            Title = title;
        }

        public class BallotIssueOptionBuilder
        {
            public int Number = 0;
            public string Title;

            public BallotIssueOptionBuilder WithOptionNumber(int number)
            {
                Number = number;
                return this;
            }

            public BallotIssueOptionBuilder WithOption(string title)
            {
                Title = title;
                return this;
            }

            public BallotIssueOption Build()
            {
                if (Number < 0)
                    throw new InvalidBuilderParameterException("Invalid option number '" + Number + "'");
                if (String.IsNullOrWhiteSpace(Title))
                    throw new InvalidBuilderParameterException("Invalid option title '" + Title + "'");
                BallotIssueOption option = new(Number, Title);
                return option;
            }
        }

    }
}