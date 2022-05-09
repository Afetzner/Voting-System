using System;
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
            var ballot = new Ballot.Builder()
                .WithVoter("A12345679")
                .WithIssue("A12345678")
                .WithChoice(0)
                .WithSerialNumber("B45479965")
                .Build();

            Assert.AreEqual("A12345679", ballot.VoterSerial);
            Assert.AreEqual("A12345678", ballot.IssueSerial);
            Assert.AreEqual(0, ballot.Choice);
            Assert.AreEqual("B45479965", ballot.SerialNumber);

        }

        [TestMethod]
        [ExpectedException(typeof(InvalidBuilderParameterException), "Ballot without voter was allowed")]
        public void BallotFailNullVoter()
        {
            var ballot = new Ballot.Builder()
                .WithIssue("B45479965")
                .WithChoice(3)
                .WithSerialNumber("B78885425")
                .Build();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidBuilderParameterException), "Ballot without ballot issue was allowed")]
        public void BallotFailNullIssue()
        {

            var ballot = new Ballot.Builder()
                .WithVoter("B45479965")
                .WithChoice(-1)
                .WithSerialNumber("B15564412")
                .Build();

        }

        [TestMethod]
        [ExpectedException(typeof(InvalidBuilderParameterException), "Ballot without ballot choice was allowed")]
        public void BallotFailNullChoice()
        {
            var ballot = new Ballot.Builder()
                .WithVoter("A23458678")
                .WithIssue("A23458678")
                .WithSerialNumber("B98652330")
                .Build();

        }

        [TestMethod]
        [ExpectedException(typeof(InvalidBuilderParameterException), "Ballot without serial number was allowed")]
        public void BallotFailNullSerial()
        {
            
            var ballot = new Ballot.Builder()
                .WithVoter("A23458678")
                .WithIssue("A23458678")
                .WithChoice(0)
                .Build();
        }
    }
}