using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using IshServices.Models;
using System.Linq;

namespace IshServices.UnitTest.Register
{
    [TestClass]
    public class RegistrationResultFixture
    {
        [TestMethod]
        public void CreateRegistrationResultNameIsEmpty()
        {
            RegistrationRequest request = new RegistrationRequest()
            {
                Name = null
            };

            RegistrationResult result = RegistrationResult.Process(request);

            Assert.IsFalse(result.IsValid);

            AssertResultHasError(result, "Name");
        }

        [TestMethod]
        public void CreateRegistrationResultWithNameFilled()
        {
            RegistrationRequest request = new RegistrationRequest()
            {
                Name = "Shelly Von Trapp"
            };

            RegistrationResult result = RegistrationResult.Process(request);

            Assert.AreEqual("Shelly", result.FirstName);
            Assert.AreEqual("Von Trapp", result.LastName);
        }

        [TestMethod]
        public void CreateRegistrationWithOnlyFirstNameFilled()
        {
            RegistrationRequest request = new RegistrationRequest()
            {
                Name = "Shelly"
            };

            RegistrationResult result = RegistrationResult.Process(request);

            Assert.AreEqual("Shelly", result.FirstName);
        }

        [TestMethod]
        public void CreateRegistrationResultWithoutPaymentAmount()
        {
            RegistrationRequest request = new RegistrationRequest()
            {
                PaymentAmount = 0
            };

            RegistrationResult result = RegistrationResult.Process(request);

            AssertResultHasError(result, "PaymentAmount");
        }

        [TestMethod]
        public void CreateRegistrationResultWithPaymentAmountOutsideAllowableRange()
        {
            RegistrationConfig regConfig = new RegistrationConfig(5, 500);

            RegistrationRequest request = new RegistrationRequest()
            {
                PaymentAmount = 600
            };

            RegistrationResult result = RegistrationResult.Process(request);

            Assert.AreEqual("Payment amount must be between $5 and $500", result.GetError("PaymentAmount"));
        }

        private static void AssertResultHasError(RegistrationResult result, string errorKey)
        {
            if (!result.HasError(errorKey))
            {
                Assert.Fail($"Expected result to have error with key: {errorKey}");
            }
        }

    }
}
