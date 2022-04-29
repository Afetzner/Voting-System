using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VotingSystem.Model;
using VotingSystem.Utils;

namespace UnitTests.ModelTests
{
    [TestClass]
    public class BallotTest
    {
        [TestMethod]
        public void BallotSuccess()
        {
            Voter voter = new VoterBuilder()
                .WithUsername("jdoe16")
                .WithPassword("Abc$900")
                .WithEmail("email@email.com")
                .WithFirstName("Jane")
                .WithLastName("Doe")
                .WithSerialNumber("A12345678")
                .Build();

            var now = DateTime.Now;
            BallotIssueOption option1 = new(0, "A");
            BallotIssueOption option2 = new(1, "B");
            BallotIssueOption option3 = new(2, "C");

            var issue = new BallotIssue.BallotIssueBuilder()
                .WithTitle("Multiple Choice")
                .WithDescription("Choose a response")
                .WithSerialNumber("A12345678")
                .WithStartDate(now)
                .WithEndDate(now.AddDays(1))
                .WithOption(option1)
                .WithOption(option2)
                .WithOption(option3)
                .Build();

            var ballot = new BallotBuilder()
                .WithVoter(voter)
                .WithIssue(issue)
                .WithChoice(option1)
                .Build();

            Assert.AreEqual(voter, ballot.Voter);
            Assert.AreEqual(issue, ballot.Issue);
            Assert.AreEqual(option1, ballot.Choice);

        }

        [TestMethod]
        [ExpectedException(typeof(InvalidBuilderParameterException), "Ballot without voter was allowed")]
        public void BallotFailNullVoter()
        {
            var now = DateTime.Now;
            BallotIssueOption option1 = new(5, "A");
            BallotIssueOption option2 = new(6, "B");
            BallotIssueOption option3 = new(7, "C");

            var issue = new BallotIssue.BallotIssueBuilder()
                .WithTitle("Multiple Choice")
                .WithDescription("Choose a response")
                .WithSerialNumber("A12345678")
                .WithStartDate(now)
                .WithEndDate(now.AddDays(1))
                .WithOption(option1)
                .WithOption(option2)
                .WithOption(option3)
                .Build();

            var ballot = new BallotBuilder()
                .WithIssue(issue)
                .WithChoice(option1)
                .Build();

        }

        [TestMethod]
        [ExpectedException(typeof(InvalidBuilderParameterException), "Ballot without ballot issue was allowed")]
        public void BallotFailNullIssue()
        {
            Voter voter = new VoterBuilder()
                .WithUsername("bleh1")
                .WithPassword("AbH$900")
                .WithFirstName("Larry")
                .WithLastName("Poe")
                .WithSerialNumber("A23458678")
                .Build();

            var now = DateTime.Now;
            BallotIssueOption option1 = new(0, "A");
            BallotIssueOption option2 = new(1, "B");
            BallotIssueOption option3 = new(2, "C");


            var ballot = new BallotBuilder()
                .WithVoter(voter)
                .WithChoice(option1)
                .Build();

        }

        [TestMethod]
        [ExpectedException(typeof(InvalidBuilderParameterException), "Ballot without ballot choice was allowed")]
        public void BallotFailNullChoice()
        {
            Voter voter = new VoterBuilder()
                .WithUsername("bleh1")
                .WithPassword("AbH$900")
                .WithFirstName("Larry")
                .WithLastName("Poe")
                .WithSerialNumber("A23458678")
                .Build();

            var now = DateTime.Now;
            BallotIssueOption option1 = new(0, "A");
            BallotIssueOption option2 = new(1, "B");
            BallotIssueOption option3 = new(2, "C");

            var issue = new BallotIssue.BallotIssueBuilder()
                .WithTitle("Multiple Choice")
                .WithDescription("Choose a response")
                .WithSerialNumber("A12345678")
                .WithStartDate(now)
                .WithEndDate(now.AddDays(1))
                .WithOption(option1)
                .WithOption(option2)
                .WithOption(option3)
                .Build();

            var ballot = new BallotBuilder()
                .WithVoter(voter)
                .WithIssue(issue)
                .Build();

        }
    }

}