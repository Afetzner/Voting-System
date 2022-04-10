using Microsoft.VisualStudio.TestTools.UnitTesting;
using VotingSystem.Controller;

namespace UnitTests.ControllerTests.ValidatorUtilsTests
{
    [TestClass]
    public class ValidateLicenseTest
    {
        [TestMethod]
        public void ValidateLicenseSuccess1()
        {
            string name = "A12345678";
            Assert.IsTrue(ValidationUtils.IsValidLicense(name));
        }

        [TestMethod]
        public void ValidateLicenseSuccess2()
        {
            string name = "Z11122233";
            Assert.IsTrue(ValidationUtils.IsValidLicense(name));
        }

        [TestMethod]
        public void ValidateLicenseFailTooShort()
        {
            string name = "T1234567";
            Assert.IsFalse(ValidationUtils.IsValidLicense(name));
        }

        [TestMethod]
        public void ValidateLicenseFailOutOfOrder()
        {
            string name = "9B0000000";
            Assert.IsFalse(ValidationUtils.IsValidLicense(name));
        }

        [TestMethod]
        public void ValidateLicenseFailSubstring()
        {
            string name = "9A12345678";
            Assert.IsFalse(ValidationUtils.IsValidLicense(name));
        }

        [TestMethod]
        public void ValidateLicenseFailContainsSpecialChar()
        {
            string name = "Z88$01000";
            Assert.IsFalse(ValidationUtils.IsValidLicense(name));
        }

        [TestMethod]
        public void ValidateLicenseFailNull()
        {
            string name = null;
            Assert.IsFalse(ValidationUtils.IsValidLicense(name));
        }

        [TestMethod]
        public void ValidateLicenseFailSpace()
        {
            string name = "   ";
            Assert.IsFalse(ValidationUtils.IsValidLicense(name));
        }
    }
}