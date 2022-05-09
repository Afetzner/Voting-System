using Microsoft.VisualStudio.TestTools.UnitTesting;
using VotingSystem.Utils;

namespace UnitTests.ControllerTests.ValidatorUtilsTests
{
    [TestClass]
    public class ValidateUsernameTest
    {
        [TestMethod]
        public void ValidateUsernameSuccess1()
        {
            string name = "uzername";
            Assert.IsTrue(Validation.IsValidUsername(name));
        }

        [TestMethod]
        public void ValidateUsernameSuccess2()
        {
            string name = "JoseJuanCarlos11";
            Assert.IsTrue(Validation.IsValidUsername(name));
        }

        [TestMethod]
        public void ValidateUsernameFailTooShort()
        {
            string name = "brie";
            Assert.IsFalse(Validation.IsValidUsername(name));
        }

        [TestMethod]
        public void ValidateUsernameFailContainsSpecialChar()
        {
            string name = "Deez_Almonds";
            Assert.IsFalse(Validation.IsValidUsername(name));
        }

        [TestMethod]
        public void ValidateUsernameFailContainsContainsSpace()
        {
            string name = "Deez Almonds";
            Assert.IsFalse(Validation.IsValidUsername(name));
        }

        [TestMethod]
        public void ValidateUsernameFailNull()
        {
            Assert.IsFalse(Validation.IsValidUsername(null));
        }

        [TestMethod]
        public void ValidateUsernameFailJustSpace()
        {
            string name = "   ";
            Assert.IsFalse(Validation.IsValidUsername(name));
        }
    }
}