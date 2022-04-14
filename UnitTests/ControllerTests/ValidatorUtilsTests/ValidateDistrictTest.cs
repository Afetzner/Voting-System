using Microsoft.VisualStudio.TestTools.UnitTesting;
using VotingSystem.Controller;

namespace UnitTests.ControllerTests.ValidatorUtilsTests
{
    [TestClass]
    public class ValidateDistrictTest
    {
        [TestMethod]
        public void ValidateDistrictSuccess1()
        {
            int districtNo = 50;
            Assert.IsTrue(ValidationUtils.IsValidDistrict(districtNo));
        }

        [TestMethod]
        public void ValidateDistrictSuccess2()
        {
            int districtNo = 100;
            Assert.IsTrue(ValidationUtils.IsValidDistrict(districtNo));
        }

        [TestMethod]
        public void ValidateDistrictFailNegativeNumber()
        {
            int districtNo = -1;
            Assert.IsFalse(ValidationUtils.IsValidDistrict(districtNo));
        }

        [TestMethod]
        public void ValidateDistrictFailExceedMax()
        {
            int districtNo = 200;
            Assert.IsFalse(ValidationUtils.IsValidDistrict(districtNo));
        }
    }
}
