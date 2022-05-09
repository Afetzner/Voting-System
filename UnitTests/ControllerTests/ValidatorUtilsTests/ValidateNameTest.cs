using Microsoft.VisualStudio.TestTools.UnitTesting;
using VotingSystem.Utils;

namespace UnitTests.ControllerTests.ValidatorUtilsTests
{
    [TestClass]
    public class ValidateNameTest
    {
        [TestMethod]
        public void ValidateNameSuccess1()
        {
            string name = "Alexander";
            Assert.IsTrue(Validation.IsValidName(name));
        }

        [TestMethod]
        public void ValidateNameSuccess2()
        {
            string name = "McDaniel";
            Assert.IsTrue(Validation.IsValidName(name));
        }

        [TestMethod]
        public void ValidateNameFailTooShort()
        {
            string name = "A";
            Assert.IsFalse(Validation.IsValidName(name));
        }

        [TestMethod]
        public void ValidateNameFailContainsNumber()
        {
            string name = "Sara4ah";
            Assert.IsFalse(Validation.IsValidName(name));
        }

        [TestMethod]
        public void ValidateNameFailContainsSpecialChar()
        {
            string name = "Zach%ary";
            Assert.IsFalse(Validation.IsValidName(name));
        }

        [TestMethod]
        public void ValidateNameFailNull()
        {
            string name = null;
            Assert.IsFalse(Validation.IsValidName(name));
        }

        [TestMethod]
        public void ValidateNameFailSpace()
        {
            string name = "   ";
            Assert.IsFalse(Validation.IsValidName(name));
        }
    }
}