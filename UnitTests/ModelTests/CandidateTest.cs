using Microsoft.VisualStudio.TestTools.UnitTesting;
using VotingSystem.Model;

namespace UnitTesting.TestModel
{
    [TestClass]
    public class CandidateTest
    {
        [TestMethod]
        public void CandidateBuilderSuccess()
        {
            Candidate candidate = new CandidateBuilder()
                .WithFirstName("Jane")
                .WithLastName("Doe")
                .Build();

            Assert.AreEqual("Jane", candidate.FirstName);
            Assert.AreEqual("Doe", candidate.LastName);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidCandidateParameterException), "Candidate w/out last name. was allowed")]
        public void CandidateBuilderFailureNullArg()
        {
            Candidate candidate = new CandidateBuilder()
                .WithFirstName("Alex")
                .Build();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidCandidateParameterException), "Candidate with invalid first name was allowed")]
        public void CandidateBuilderFailureBadName()
        {
            Candidate candidate = new CandidateBuilder()
                .WithFirstName("Bleh-1")
                .WithLastName("Doe")
                .Build();

        }

        [TestMethod]
        [ExpectedException(typeof(InvalidCandidateParameterException), "Candidate with w/out first name was allowed")]
        public void CandidateBuilderFailureNoFirstName()
        {
            Candidate candidate = new CandidateBuilder()
                .WithLastName("Doe")
                .Build();

        }

        [TestMethod]
        [ExpectedException(typeof(InvalidCandidateParameterException), "Candidate with invalid first name was allowed")]
        public void CandidateBuilderFailureLongFirstName()
        {
            Candidate candidate = new CandidateBuilder()
                .WithFirstName("Janiceeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeee")
                .WithLastName("Doe")
                .Build();

        }
    }
}
