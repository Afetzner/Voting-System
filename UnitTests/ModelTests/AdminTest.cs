using Microsoft.VisualStudio.TestTools.UnitTesting;
using VotingSystem.Model;
using VotingSystem.Utils;

namespace UnitTests.ModelTests
{
    [TestClass]
    public class AdminTest
    {
        [TestMethod]
        public void AdminBuilderSuccess()
        {
            Admin admin = new AdminBuilder()
                .WithUsername("jdoe16")
                .WithPassword("Abc$900")
                .WithFirstName("jane")
                .WithLastName("doe")
                .WithSerialNumber("A12345678")
                .Build();

            Assert.AreEqual("jdoe16", admin.Username);
            Assert.AreEqual("jane", admin.FirstName);
            Assert.AreEqual("doe", admin.LastName);
            Assert.AreEqual("Abc$900", admin.Password);
            Assert.AreEqual("A12345678", admin.SerialNumber);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidBuilderParameterException), "Admin w/out serial num. was allowed")]
        public void AdminBuilderFailureNullSerial()
        {
            Admin admin = new AdminBuilder()
                .WithUsername("afet001")
                .WithPassword("000zteF!")
                .Build();
        }


        [TestMethod]
        [ExpectedException(typeof(InvalidBuilderParameterException), "Admin with invalid serial number was allowed")]
        public void AdminBuilderFailureBadSerial()
        {
            Admin admin = new AdminBuilder()
                .WithUsername("NoThoughts")
                .WithPassword("Head3mpty@")
                .WithSerialNumber("T-1000")
                .Build();

        }
    }
}
