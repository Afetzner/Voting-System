using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VotingSystem.Model;
using VotingSystem.Utils;

namespace UnitTests.ModelTests
{
    [TestClass]
    public class BallotIssueTest
    {
        [TestMethod]
        public void BallotIssueSuccessManualOptionBuildersIndividual()
        {
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

            Assert.AreEqual("Multiple Choice", issue.Title);
            Assert.AreEqual("Choose a response", issue.Description);
            Assert.AreEqual("A12345678", issue.SerialNumber);
            Assert.AreEqual(now, issue.StartDate);
            Assert.AreEqual(now.AddDays(1), issue.EndDate);
            CollectionAssert.AreEqual(new []{option1, option2, option3}, issue.Options);
        }

        [TestMethod]
        public void BallotIssueSuccessManualOptionBuildersList()
        {
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
                .WithOptions(new List<BallotIssueOption>{option1, option2, option3 })
                .Build();

            Assert.AreEqual("Multiple Choice", issue.Title);
            Assert.AreEqual("Choose a response", issue.Description);
            Assert.AreEqual("A12345678", issue.SerialNumber);
            Assert.AreEqual(now, issue.StartDate);
            Assert.AreEqual(now.AddDays(1), issue.EndDate);
            CollectionAssert.AreEqual(new[] { option1, option2, option3 }, issue.Options);
        }

        [TestMethod]
        public void BallotIssueSuccessStringListOptionBuilder()
        {
            var now = DateTime.Now;
            List<string> titles = new (){"A", "B", "C"};

            var issue = new BallotIssue.BallotIssueBuilder()
                .WithTitle("Multiple Choice")
                .WithDescription("Choose a response")
                .WithSerialNumber("A12345678")
                .WithStartDate(now)
                .WithEndDate(now.AddDays(1))
                .WithOptions(titles)
                .Build();

            Assert.AreEqual("Multiple Choice", issue.Title);
            Assert.AreEqual("Choose a response", issue.Description);
            Assert.AreEqual("A12345678", issue.SerialNumber);
            Assert.AreEqual(now, issue.StartDate);
            Assert.AreEqual(now.AddDays(1), issue.EndDate);
            Assert.AreEqual("A", issue.Options[0].Title);
            Assert.AreEqual("B", issue.Options[1].Title);
            Assert.AreEqual("C", issue.Options[2].Title);
            Assert.AreEqual(0, issue.Options[0].Number);
            Assert.AreEqual(1, issue.Options[1].Number);
            Assert.AreEqual(2, issue.Options[2].Number);
        }

        [TestMethod]
        public void BallotIssueSuccessParamsStringOptionBuilders()
        {
            var now = DateTime.Now;

            var issue = new BallotIssue.BallotIssueBuilder()
                .WithTitle("Multiple Choice")
                .WithDescription("Choose a response")
                .WithSerialNumber("A12345678")
                .WithStartDate(now)
                .WithEndDate(now.AddDays(1))
                .WithOptions("A", "B", "C")
                .Build();

            Assert.AreEqual("Multiple Choice", issue.Title);
            Assert.AreEqual("Choose a response", issue.Description);
            Assert.AreEqual("A12345678", issue.SerialNumber);
            Assert.AreEqual(now, issue.StartDate);
            Assert.AreEqual(now.AddDays(1), issue.EndDate);
            Assert.AreEqual("A", issue.Options[0].Title);
            Assert.AreEqual("B", issue.Options[1].Title);
            Assert.AreEqual("C", issue.Options[2].Title);
            Assert.AreEqual(0, issue.Options[0].Number);
            Assert.AreEqual(1, issue.Options[1].Number);
            Assert.AreEqual(2, issue.Options[2].Number);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidBuilderParameterException), "Ballot issue w/ start before end allowed")]
        public void BallotIssueFailStartAfterEnd()
        {
            var now = DateTime.Now;

            var issue = new BallotIssue.BallotIssueBuilder()
                .WithTitle("Multiple Choice")
                .WithDescription("Choose a response")
                .WithSerialNumber("A12345678")
                .WithStartDate(now)
                .WithEndDate(now.AddDays(-1))
                .WithOptions("A", "B", "C")
                .Build();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidBuilderParameterException), "Ballot issue w/ duplicate option titles allowed")]
        public void BallotIssueFailStartDuplicateOptionTitle()
        {
            var now = DateTime.Now;

            var issue = new BallotIssue.BallotIssueBuilder()
                .WithTitle("Multiple Choice")
                .WithDescription("Choose a response")
                .WithSerialNumber("A12345678")
                .WithStartDate(now)
                .WithEndDate(now.AddDays(1))
                .WithOptions("A", "A", "C")
                .Build();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidBuilderParameterException),
            "Ballot issue w/ duplicate option numbers allowed")]
        public void BallotIssueFailDuplicateNumbers()
        {
            var now = DateTime.Now;
            BallotIssueOption option1 = new(0, "A");
            BallotIssueOption option2 = new(1, "B");
            BallotIssueOption option3 = new(1, "C");

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
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidBuilderParameterException), "Ballot issue w/ null start allowed")]
        public void BallotIssueFailNullTitle()
        {
            var now = DateTime.Now;

            var issue = new BallotIssue.BallotIssueBuilder()
                .WithDescription("Choose a response")
                .WithSerialNumber("A12345678")
                .WithStartDate(now)
                .WithEndDate(now.AddDays(1))
                .WithOptions("A", "B", "C")
                .Build();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidBuilderParameterException), "Ballot issue w/ null description allowed")]
        public void BallotIssueFailNullDescription()
        {
            var now = DateTime.Now;

            var issue = new BallotIssue.BallotIssueBuilder()
                .WithTitle("Multiple Choice")
                .WithSerialNumber("A12345678")
                .WithStartDate(now)
                .WithEndDate(now.AddDays(1))
                .WithOptions("A", "B", "C")
                .Build();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidBuilderParameterException), "Ballot issue w/ null serial num allowed")]
        public void BallotIssueFailNullSerial()
        {
            var now = DateTime.Now;

            var issue = new BallotIssue.BallotIssueBuilder()
                .WithTitle("Multiple Choice")
                .WithDescription("Choose a response")
                .WithStartDate(now)
                .WithEndDate(now.AddDays(1))
                .WithOptions("A", "B", "C")
                .Build();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidBuilderParameterException), "Ballot issue w/ null options allowed")]
        public void BallotIssueFailNullOptions()
        {
            var now = DateTime.Now;

            var issue = new BallotIssue.BallotIssueBuilder()
                .WithTitle("Multiple Choice")
                .WithDescription("Choose a response")
                .WithSerialNumber("A12345678")
                .WithStartDate(now)
                .WithEndDate(now.AddDays(1))               
                .Build();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidBuilderParameterException), "Ballot issue w/ null start allowed")]
        public void BallotIssueFailNullStart()
        {
            var now = DateTime.Now;

            var issue = new BallotIssue.BallotIssueBuilder()
                .WithTitle("Multiple Choice")
                .WithDescription("Choose a response")
                .WithSerialNumber("A12345678")
                .WithEndDate(now.AddDays(1))
                .WithOptions("A", "B", "C")
                .Build();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidBuilderParameterException), "Ballot issue w/ null end allowed")]
        public void BallotIssueFailNullEnd()
        {
            var now = DateTime.Now;

            var issue = new BallotIssue.BallotIssueBuilder()
                .WithTitle("Multiple Choice")
                .WithDescription("Choose a response")
                .WithSerialNumber("A12345678")
                .WithStartDate(now)
                .WithOptions("A", "B", "C")
                .Build();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidBuilderParameterException), "Ballot issue w/ null end before now allowed")]
        public void BallotIssueFailNullEndBeforeNow()
        {
            var now = DateTime.Now.AddDays(-2);

            var issue = new BallotIssue.BallotIssueBuilder()
                .WithTitle("Multiple Choice")
                .WithDescription("Choose a response")
                .WithSerialNumber("A12345678")
                .WithStartDate(now)
                .WithOptions("A", "B", "C")
                .Build();
        }
    }
}
