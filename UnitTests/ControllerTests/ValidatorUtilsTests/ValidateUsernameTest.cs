using Microsoft.VisualStudio.TestTools.UnitTesting;
using VotingSystem.Controller;

namespace UnitTests.ControllerTests.ValidatorUtilsTests
{
    [TestClass]
    public class ValidateUsernameTest
    {
        [TestMethod]
        public void ValidateUsernameSuccess1()
        {
            string name = "uzername";
            Assert.IsTrue(ValidationUtils.IsValidUsername(name));
        }

        [TestMethod]
        public void ValidateUsernameSuccess2()
        {
            string name = "JoseJuanCarlos11";
            Assert.IsTrue(ValidationUtils.IsValidUsername(name));
        }

        [TestMethod]
        public void ValidateUsernameFailTooShort()
        {
            string name = "brie";
            Assert.IsFalse(ValidationUtils.IsValidUsername(name));
        }

        [TestMethod]
        public void ValidateUsernameFailContainsSpecialChar()
        {
            string name = "Deez_Almonds";
            Assert.IsFalse(ValidationUtils.IsValidUsername(name));
        }

        [TestMethod]
        public void ValidateUsernameFailContainsContainsSpace()
        {
            string name = "Deez Almonds";
            Assert.IsFalse(ValidationUtils.IsValidUsername(name));
        }

        [TestMethod]
        public void ValidateUsernameFailNull()
        {
            Assert.IsFalse(ValidationUtils.IsValidUsername(null));
        }

        [TestMethod]
        public void ValidateUsernameFailJustSpace()
        {
            string name = "   ";
            Assert.IsFalse(ValidationUtils.IsValidUsername(name));
        }
    }
}