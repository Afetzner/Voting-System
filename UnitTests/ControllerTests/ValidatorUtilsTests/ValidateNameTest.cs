using Microsoft.VisualStudio.TestTools.UnitTesting;
using VotingSystem.Controller;

namespace UnitTests.ControllerTests.ValidatorUtilsTests
{
    [TestClass]
    public class ValidateNameTest
    {
        [TestMethod]
        public void ValidateNameSuccess1()
        {
            string name = "Alexander";
            Assert.IsTrue(ValidationUtils.IsValidName(name));
        }

        [TestMethod]
        public void ValidateNameSuccess2()
        {
            string name = "McDaniel";
            Assert.IsTrue(ValidationUtils.IsValidName(name));
        }

        [TestMethod]
        public void ValidateNameFailTooShort()
        {
            string name = "A";
            Assert.IsFalse(ValidationUtils.IsValidName(name));
        }

        [TestMethod]
        public void ValidateNameFailContainsNumber()
        {
            string name = "Sara4ah";
            Assert.IsFalse(ValidationUtils.IsValidName(name));
        }

        [TestMethod]
        public void ValidateNameFailContainsSpecialChar()
        {
            string name = "Zach%ary";
            Assert.IsFalse(ValidationUtils.IsValidName(name));
        }

        [TestMethod]
        public void ValidateNameFailNull()
        {
            string name = null;
            Assert.IsFalse(ValidationUtils.IsValidName(name));
        }

        [TestMethod]
        public void ValidateNameFailSpace()
        {
            string name = "   ";
            Assert.IsFalse(ValidationUtils.IsValidName(name));
        }
    }
}