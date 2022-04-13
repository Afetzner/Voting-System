using Microsoft.VisualStudio.TestTools.UnitTesting;
using VotingSystem.Controller;

namespace UnitTests.ControllerTests.ValidatorUtilsTests
{
    [TestClass]
    public class ValidateStateTest
    {
        [TestMethod]
        public void ValidateStateSuccess1()
        {
            string state = "NE";
            Assert.IsTrue(ValidationUtils.IsValidState(state));
        }

        [TestMethod]
        public void ValidateStateSuccess2()
        {
            string state = "CA";
            Assert.IsTrue(ValidationUtils.IsValidState(state));
        }

        [TestMethod]
        public void ValidateStateFailTooShort()
        {
            string state = "A";
            Assert.IsFalse(ValidationUtils.IsValidState(state));
        }

        [TestMethod]
        public void ValidateStateFailContainsNumber()
        {
            string state = "L4A";
            Assert.IsFalse(ValidationUtils.IsValidState(state));
        }

        [TestMethod]
        public void ValidateStateFailContainsSpecialChar()
        {
            string state = "hj#as";
            Assert.IsFalse(ValidationUtils.IsValidState(state));
        }

        [TestMethod]
        public void ValidateStateFailNull()
        {
            string state = null;
            Assert.IsFalse(ValidationUtils.IsValidState(state));
        }

        [TestMethod]
        public void ValidateStateFailSpace()
        {
            string state = "   ";
            Assert.IsFalse(ValidationUtils.IsValidState(state));
        }

        [TestMethod]
        public void ValidateStateFailLowerCase()
        {
            string state = "ne";
            Assert.IsFalse(ValidationUtils.IsValidState(state));
        }
}
}