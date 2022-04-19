using Microsoft.VisualStudio.TestTools.UnitTesting;
using VotingSystem.Model;
using VotingSystem.Utils;

namespace UnitTests.ModelTests
{
    [TestClass]
    public class BallotIssueOptionTest
    {
        [TestMethod]
        public void BallotIssueOptionTestSuccess1()
        {
            var option = new BallotIssueOption.BallotIssueOptionBuilder()
                .WithTitle("Person A")
                .WithOptionNumber(0)
                .Build();
            
            Assert.AreEqual("Person A", option.Title);
            Assert.AreEqual(0, option.Number);
        }

        [TestMethod]
        public void BallotIssueOptionTestSuccess2()
        {
            var option = new BallotIssueOption.BallotIssueOptionBuilder()
                .WithTitle("Person X")
                .WithOptionNumber(1)
                .Build();

            Assert.AreEqual("Person X", option.Title);
            Assert.AreEqual(1, option.Number);
        }

        [ExpectedException(typeof(InvalidBuilderParameterException), "BallotIssueOption with negative number was allowed")]
        [TestMethod]
        public void BallotIssueOptionFailBadNumber()
        {
            var option = new BallotIssueOption.BallotIssueOptionBuilder()
                .WithTitle("Person A")
                .WithOptionNumber(-1)
                .Build();
        }

        [ExpectedException(typeof(InvalidBuilderParameterException), "BallotIssueOption w/o number was allowed")]
        [TestMethod]
        public void BallotIssueOptionFailNoNumber()
        {
            var option = new BallotIssueOption.BallotIssueOptionBuilder()
                .WithTitle("Sandra")
                .Build();
        }

        [ExpectedException(typeof(InvalidBuilderParameterException), "BallotIssueOption with null title was allowed")]
        [TestMethod]
        public void BallotIssueOptionFailNullTitle()
        {
            var option = new BallotIssueOption.BallotIssueOptionBuilder()
                .WithOptionNumber(0)
                .Build();
        }

        [ExpectedException(typeof(InvalidBuilderParameterException), "BallotIssueOption with no title was allowed")]
        [TestMethod]
        public void BallotIssueOptionFailBlanckTitle()
        {
            var option = new BallotIssueOption.BallotIssueOptionBuilder()
                .WithTitle("   ")
                .WithOptionNumber(0)
                .Build();
        }
    }
}