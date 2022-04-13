using Microsoft.VisualStudio.TestTools.UnitTesting;
using VotingSystem.Controller;

namespace UnitTests.ControllerTests.ValidatorUtilsTests
{
    [TestClass]
    public class ValidateDateTest
    {
        [TestMethod]
        public void ValidateDateSuccess1()
        {
            string date = "2000-01-01";
            Assert.IsTrue(ValidationUtils.IsValidDate(date));
        }

        [TestMethod]
        public void ValidateDateSuccess2()
        {
            string date = "2999-12-31";
            Assert.IsTrue(ValidationUtils.IsValidDate(date));
        }

        [TestMethod]
        public void ValidateDateSuccessLeapYear()
        {
            string date = "2004-02-29";
            Assert.IsTrue(ValidationUtils.IsValidDate(date));
        }

        [TestMethod]
        public void ValidateDateFailNonLeapYear()
        {
            string date = "2005-02-29";
            Assert.IsFalse(ValidationUtils.IsValidDate(date));
        }

        [TestMethod]
        public void ValidateDateFailDayTooHigh()
        {
            string date = "2019-11-31";
            Assert.IsFalse(ValidationUtils.IsValidDate(date));
        }

        [TestMethod]
        public void ValidateDateFailDayWayTooHigh()
        {
            string date = "2019-11-52";
            Assert.IsFalse(ValidationUtils.IsValidDate(date));
        }

        [TestMethod]
        public void ValidateDateFailDayTooLow()
        {
            string date = "2019-06-00";
            Assert.IsFalse(ValidationUtils.IsValidDate(date));
        }

        [TestMethod]
        public void ValidateDateFailMonthTooHigh()
        {
            string date = "2025-13-25";
            Assert.IsFalse(ValidationUtils.IsValidDate(date));
        }

        [TestMethod]
        public void ValidateDateFailMonthWayTooHigh()
        {
            string date = "2025-40-25";
            Assert.IsFalse(ValidationUtils.IsValidDate(date));
        }

        [TestMethod]
        public void ValidateDateFailMonthTooLow()
        {
            string date = "2019-00-15";
            Assert.IsFalse(ValidationUtils.IsValidDate(date));
        }

        [TestMethod]
        public void ValidateDateFailNull()
        {
            string date = null;
            Assert.IsFalse(ValidationUtils.IsValidDate(date));
        }

        [TestMethod]
        public void ValidateDateFailBadDelimiter()
        {
            string date = "2000/09/13";
            Assert.IsFalse(ValidationUtils.IsValidDate(date));
        }

        [TestMethod]
        public void ValidateDateYearTooShort()
        {
            string date = "200-13-25";
            Assert.IsFalse(ValidationUtils.IsValidDate(date));
        }

        [TestMethod]
        public void ValidateDateFailMonthTooShort()
        {
            string date = "2005-1-25";
            Assert.IsFalse(ValidationUtils.IsValidDate(date));
        }

        [TestMethod]
        public void ValidateDateFailDayTooShort()
        {
            string date = "2006-13-5";
            Assert.IsFalse(ValidationUtils.IsValidDate(date));
        }

        [TestMethod]
        public void ValidateDateFailNonsense()
        {
            string date = "-1232-31-1444";
            Assert.IsFalse(ValidationUtils.IsValidDate(date));
        }
    }
}
