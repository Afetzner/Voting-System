using Microsoft.VisualStudio.TestTools.UnitTesting;
using VotingSystem.Model;

namespace UnitTesting.TestModel
{
    [TestClass]
    public class VoterTest
    {
        [TestMethod]
        public void VoterBuilderSuccess()
        {
            Voter voter = new VoterBuilder()
                .WithFirstName("Jane")
                .WithLastName("Doe")
                .WithMiddleName("X")
                .WithLicenseNumber("A12345678")
                .Build();

            Assert.AreEqual("Jane", voter.FirstName);
            Assert.AreEqual("Doe", voter.LastName);
            Assert.AreEqual("X", voter.MiddleName);
            Assert.AreEqual("A12345678", voter.LicenseNumber);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidBuilderParameterException), "Voter w/out license num. was allowed")]
        public void VoterBuilderFailureNullArg()
        {
            Voter voter = new VoterBuilder()
                .WithFirstName("Alex")
                .WithLastName("F")
                .WithMiddleName("Z")
                .Build();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidBuilderParameterException), "Voter with invalid name was allowed")]
        public void VoterBuilderFailureBadName()
        {
            Voter voter = new VoterBuilder()
                .WithFirstName("Jane1")
                .WithLastName("Doe")
                .WithMiddleName("X")
                .WithLicenseNumber("A12345678")
                .Build();

        }

        [TestMethod]
        [ExpectedException(typeof(InvalidBuilderParameterException), "Voter with invalid name was allowed")]
        public void VoterBuilderFailureBadLicense()
        {
            Voter voter = new VoterBuilder()
                .WithFirstName("Jane")
                .WithLastName("Doe")
                .WithMiddleName("X")
                .WithLicenseNumber("A1234567")
                .Build();

        }
    }
}
