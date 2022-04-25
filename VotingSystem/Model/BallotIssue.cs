using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using VotingSystem.Utils;

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
                StartDate = start;
                return this;
            }

            public BallotIssueBuilder WithEndDate(DateTime end)
            {
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

            public BallotIssueBuilder WithOptions(List<BallotIssueOption> options)
            {
                Options = options;
                return this;
            }

            public BallotIssue Build()
            {
                if (!Validation.IsValidSerialNumber(SerialNumber))
                    throw new InvalidBuilderParameterException("Invalid serial number '" + SerialNumber + "'");
                if (EndDate < StartDate)
                    throw new InvalidBuilderParameterException("Invalid start/end date (end before start) s:'" + StartDate+ "' e:" + EndDate + "'");
                if (EndDate < DateTime.Now)
                    throw new InvalidBuilderParameterException("Invalid end date (before now) '" + EndDate + "'");
                if (String.IsNullOrEmpty(Title))
                    throw new InvalidBuilderParameterException("Invalid title '" + Title + "'");
                if (String.IsNullOrEmpty(Description))
                    throw new InvalidBuilderParameterException("Invalid description '" + Description + "'");
                if (Options.Count == 0)
                    throw new InvalidBuilderParameterException("Invalid options list (empty) \n'" + Options + "'");

                BallotIssue issue = new (SerialNumber, StartDate, EndDate, Title, Description, Options);
                return issue;
            }
        }
    }
}
