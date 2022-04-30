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
            Admin admin = new Admin.AdminBuilder()
                .WithUsername("jdoe16")
                .WithPassword("Abc$900")
                .WithEmail("email@email.com")
                .WithSerialNumber("A12345678")
                .WithFirstName("jane")
                .WithLastName("doe")
                .Build();

            Assert.AreEqual("jdoe16", admin.Username);
            Assert.AreEqual("Abc$900", admin.Password);
            Assert.AreEqual("email@email.com", admin.Email);
            Assert.AreEqual("A12345678", admin.SerialNumber);
            Assert.AreEqual("jane", admin.FirstName);
            Assert.AreEqual("doe", admin.LastName);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidBuilderParameterException), "Admin w/out serial num. was allowed")]
        public void AdminBuilderFailureNoSerial()
        {
            Admin admin = new Admin.AdminBuilder()
                .WithUsername("NoThoughts")
                .WithPassword("Head3mpty")
                .WithEmail("email@email.com")
                .WithFirstName("jane")
                .WithLastName("doe")
                .Build();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidBuilderParameterException), "Admin w/out username was allowed")]
        public void AdminBuilderFailureNoUsername()
        {
            Admin admin = new Admin.AdminBuilder()
                .WithPassword("Head3mpty")
                .WithEmail("email@email.com")
                .WithFirstName("jane")
                .WithLastName("doe")
                .WithSerialNumber("A12345678")
                .Build();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidBuilderParameterException), "Admin w/out password was allowed")]
        public void AdminBuilderFailureNoPassword()
        {
            Admin admin = new Admin.AdminBuilder()
                .WithUsername("NoThoughts")
                .WithEmail("email@email.com")
                .WithFirstName("jane")
                .WithLastName("doe")
                .WithSerialNumber("A12345678")
                .Build();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidBuilderParameterException), "Admin w/out first name was allowed")]
        public void AdminBuilderFailureNoFirst()
        {
            Admin admin = new Admin.AdminBuilder()
                .WithUsername("NoThoughts")
                .WithPassword("Head3mpty")
                .WithEmail("email@email.com")
                .WithSerialNumber("A12345678")
                .WithLastName("doe")
                .Build();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidBuilderParameterException), "Admin w/out last name was allowed")]
        public void AdminBuilderFailureNoLast()
        {
            Admin admin = new Admin.AdminBuilder()
                .WithUsername("NoThoughts")
                .WithPassword("Head3mpty")
                .WithEmail("email@email.com")
                .WithSerialNumber("A12345678")
                .WithFirstName("jane")
                .Build();
        }


        [TestMethod]
        [ExpectedException(typeof(InvalidBuilderParameterException), "Admin with invalid serial number was allowed")]
        public void AdminBuilderFailureBadSerial()
        {
            Admin admin = new Admin.AdminBuilder()
                .WithUsername("NoThoughts")
                .WithPassword("Head3mpty")
                .WithEmail("email@email.com")
                .WithSerialNumber("A-12345678")
                .WithFirstName("jane")
                .Build();
        }
    }
}
