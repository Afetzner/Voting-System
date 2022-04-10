using Microsoft.VisualStudio.TestTools.UnitTesting;
using VotingSystem.Controller;

namespace UnitTests.ControllerTests.ValidatorUtilsTests
{
    [TestClass]
    public class ValidatePasswordTest
    {
        [TestMethod]
        public void ValidatePasswordSuccess1()
        {
            string name = "1abdX!";
            Assert.IsTrue(ValidationUtils.IsValidPassword(name));
        }

        [TestMethod]
        public void ValidatePasswordSuccess2()
        {
            string name = "@zwx2ZYZ#";
            Assert.IsTrue(ValidationUtils.IsValidPassword(name));
        }

        [TestMethod]
        public void ValidatePasswordSuccess3()
        {
            string name = "$%34&aA";
            Assert.IsTrue(ValidationUtils.IsValidPassword(name));
        }

        [TestMethod]
        public void ValidatePasswordSuccess4()
        {
            string name = "a12!00aTy89";
            Assert.IsTrue(ValidationUtils.IsValidPassword(name));
        }

        [TestMethod]
        public void ValidatePasswordFailTooShort()
        {
            string name = "a!1";
            Assert.IsFalse(ValidationUtils.IsValidPassword(name));
        }

        [TestMethod]
        public void ValidatePasswordFailNoSpecialChar()
        {
            string name = "Alex11";
            Assert.IsFalse(ValidationUtils.IsValidPassword(name));
        }

        [TestMethod]
        public void ValidatePasswordFailNoUpperChar()
        {
            string name = "alex11!";
            Assert.IsFalse(ValidationUtils.IsValidPassword(name));
        }

        [TestMethod]
        public void ValidatePasswordFailNoLowerChar()
        {
            string name = "ALEX11!";
            Assert.IsFalse(ValidationUtils.IsValidPassword(name));
        }

        [TestMethod]
        public void ValidatePasswordFailNoNumber()
        {
            string name = "Joseph$";
            Assert.IsFalse(ValidationUtils.IsValidPassword(name));
        }

        [TestMethod]
        public void ValidatePasswordFailContainsContainsSpace()
        {
            string name = "sandra #12";
            Assert.IsFalse(ValidationUtils.IsValidPassword(name));
        }

        [TestMethod]
        public void ValidatePasswordFailNull()
        {
            Assert.IsFalse(ValidationUtils.IsValidPassword(null));
        }

        [TestMethod]
        public void ValidatePasswordFailJustSpace()
        {
            string name = "   ";
            Assert.IsFalse(ValidationUtils.IsValidPassword(name));
        }
    }
}