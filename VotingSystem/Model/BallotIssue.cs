using System;
using System.Collections.Generic;
using System.Linq;
using VotingSystem.Utils;
using static System.String;

namespace VotingSystem.Model
{
    public class BallotIssue
    {
        public string SerialNumber { get; }
        public DateTime StartDate { get; }
        public DateTime EndDate { get; }
        public string Title { get; }
        public string Description { get; }
        public List<BallotIssueOption> Options { get; }

        BallotIssue(string serialNum, DateTime start, DateTime end, string title, string description,
            List<BallotIssueOption> options)
        {
            SerialNumber = serialNum;
            StartDate = start;
            EndDate = end;
            Title = title;
            Description = description;
            Options = options;
        }

        public class BallotIssueBuilder
        {
            private bool _selectedStartTime = false;
            private bool _selectedEndTime = false;
            public string SerialNumber;
            public DateTime StartDate;
            public DateTime EndDate;
            public string Title;
            public string Description;
            public List<BallotIssueOption> Options = new();

            public BallotIssueBuilder WithSerialNumber(string serialNum)
            {
                SerialNumber = serialNum;
                return this;
            }

            public BallotIssueBuilder WithStartDate(DateTime start)
            {
                _selectedStartTime = true;
                StartDate = start;
                return this;
            }

            public BallotIssueBuilder WithEndDate(DateTime end)
            {
                _selectedEndTime = true;
                EndDate = end;
                return this;
            }

            public BallotIssueBuilder WithTitle(string title)
            {
                Title = title;
                return this;
            }

            public BallotIssueBuilder WithDescription(string description)
            {
                Description = description;
                return this;
            }

            /// <summary>
            /// Adds options from a list of BallotIssueOptions
            /// </summary>
            public BallotIssueBuilder WithOptions(List<BallotIssueOption> options)
            {
                foreach (var option in options)
                    Options.Add(option);
                return this;
            }

            /// <summary>
            /// Adds a single ballotIssueOption to the BallotIssue
            /// </summary>
            public BallotIssueBuilder WithOption(BallotIssueOption option)
            {
                Options.Add(option);
                return this;
            }

            /// <summary>
            /// Adds options to BallotIssue from a list of titles
            /// </summary>
            public BallotIssueBuilder WithOptions(List<string> titles)
            {
                var count = 0;
                foreach (var option in titles.Select(title => new BallotIssueOption.BallotIssueOptionBuilder()
                             .WithTitle(title)
                             .WithOptionNumber(count++)
                             .Build()))
                {
                    Options.Add(option);
                }
                return this;
            }

            /// <summary>
            /// **preferred** Adds options to the BallotIssue with titles specified
            /// </summary>
            /// <param name="titles">Titles of options</param>
            public BallotIssueBuilder WithOptions(params string[] titles)
            {
                var count = 0;
                foreach (var title in titles)
                {
                    var opt = new BallotIssueOption.BallotIssueOptionBuilder()
                        .WithTitle(title)
                        .WithOptionNumber(count++)
                        .Build();
                    Options.Add(opt);
                }
                return this;
            }

            public BallotIssue Build()
            {
                if (!Validation.IsValidSerialNumber(SerialNumber))
                    throw new InvalidBuilderParameterException($@"Invalid serial number '{SerialNumber}'");
                if (!_selectedStartTime)
                    throw new InvalidBuilderParameterException($@"Invalid BallotIssue w/out start time");
                if (!_selectedEndTime)
                    throw new InvalidBuilderParameterException($@"Invalid BallotIssue w/out end time");
                if (EndDate < StartDate)
                    throw new InvalidBuilderParameterException($@"Invalid start/end date (end before start) s:'{StartDate}' e:'{EndDate}'");
                if (EndDate < DateTime.Now)
                    throw new InvalidBuilderParameterException($@"Invalid end date (before now) '{EndDate}'");
                if (IsNullOrEmpty(Title))
                    throw new InvalidBuilderParameterException($@"Invalid title '{Title}'");
                if (IsNullOrEmpty(Description))
                    throw new InvalidBuilderParameterException($@"Invalid description '{Description }'");
                if (Options.Count == 0)
                    throw new InvalidBuilderParameterException(@"Invalid options list (empty)");
                
                //Check for duplicate option titles/numbers, could be more efficient, but not important
                for (var i = 0; i < Options.Count; i++)
                {
                    for (var j = i + 1; j < Options.Count; j++)
                    {
                        if (Options[i].Number == Options[j].Number)
                            throw new InvalidBuilderParameterException(
                                $@"Ballot Issue with duplicate option numbers '{Options[i].Title}', '{Options[j].Title}' ({Options[i].Number})");
                        if (Options[i].Title.Equals(Options[j].Title))
                            throw new InvalidBuilderParameterException(
                                $@"Ballot Issue with duplicate option titles'{Options[i].Title}', '{Options[j].Title}'");
                    }
                }

                BallotIssue issue = new (SerialNumber, StartDate, EndDate, Title, Description, Options);
                return issue;
            }
        }
    }
}
