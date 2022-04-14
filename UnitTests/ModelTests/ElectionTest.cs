using Microsoft.VisualStudio.TestTools.UnitTesting;
using VotingSystem.Model;

namespace UnitTesting.TestModel
{
    [TestClass]
    public class ElectionTest
    {
        [TestMethod]
        public void ElectionBuilderSuccess()
        {
            Election election = new ElectionBuilder()
                .WithState("NE")
                .WithDistrict(3)
                .WithStartDate("2000-01-01")
                .WithEndDate("2000-01-31")
                .Build();

            Assert.AreEqual("NE", election.State);
            Assert.AreEqual(3, election.District);
            Assert.AreEqual("2000-01-01", election.StartDate);
            Assert.AreEqual("2000-01-31", election.EndDate);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidElectionParameterException), "Election w/out state was allowed")]
        public void ElectionBuilderFailureNullArg()
        {
            Election election = new ElectionBuilder()
                .WithDistrict(3)
                .WithStartDate("2000-01-01")
                .WithEndDate("2000-01-31")
                .Build();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidElectionParameterException), "Election with invalid state was allowed")]
        public void ElectionBuilderFailureInvalidState()
        {
            Election election = new ElectionBuilder()
                .WithState("asdjk")
                .WithDistrict(3)
                .WithStartDate("2000-01-01")
                .WithEndDate("2000-01-31")
                .Build();

        }

        [TestMethod]
        [ExpectedException(typeof(InvalidElectionParameterException), "Election with invalid start date was allowed")]
        public void ElectionBuilderFailureBadDate()
        {
            Election election = new ElectionBuilder()
                .WithState("LA")
                .WithDistrict(3)
                .WithStartDate("2000-01-51")
                .WithEndDate("2000-01-31")
                .Build();

        }
    }
}

