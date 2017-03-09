using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using IshServices.Models;
using System.Linq;
using IshServices.Validators;

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
                Name = null,
            };

            RegistrationResult result = RegistrationResult.Process(request, null);

            Assert.IsFalse(result.IsValid);

            Assert.AreEqual("Name is required", result.GetError("Name"));
        }

        [TestMethod]
        public void CreateRegistrationResultWithNameFilled()
        {
            RegistrationRequest request = new RegistrationRequest()
            {
                Name = "Shelly Von Trapp"
            };

            RegistrationResult result = RegistrationResult.Process(request, null);

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

            RegistrationResult result = RegistrationResult.Process(request, null);

            Assert.AreEqual("Shelly", result.FirstName);
        }

        [TestMethod]
        public void CreateRegistrationResultWithPaymentAmountOutsideAllowableRange()
        {
            RegistrationRequest request = new RegistrationRequest()
            {
                PaymentAmount = 600
            };

            RegistrationResult result = RegistrationResult.Process(request, new[] { new PaymentAmountValidator(5, 500) });

            Assert.AreEqual("Payment amount must be between $5 and $500", result.GetError("PaymentAmount"));
        }

        [TestMethod]
        public void CreateRegistrationResultWithInvalidPhone()
        {
            RegistrationRequest request = new RegistrationRequest()
            {
                Phone = "123"
            };

            RegistrationResult result = RegistrationResult.Process(request, new[] { new PhoneValidator() });

            Assert.AreEqual("A phone number is required", result.GetError("Phone"));
        }

        [TestMethod]
        public void CreateRegistrationResultWithShortPhone()
        {
            RegistrationRequest request = new RegistrationRequest()
            {
                Name = "Joe",
                Phone = "283-5266"
            };

            RegistrationResult result = RegistrationResult.Process(request, new[] { new PhoneValidator() });

            Assert.AreEqual("413", result.PhoneAreaCode);
            Assert.AreEqual("283", result.PhonePrefix);
            Assert.AreEqual("5266", result.PhoneSuffix);
        }

        [TestMethod]
        public void CreateRegistrationResultWithFullPhone()
        {
            RegistrationRequest request = new RegistrationRequest()
            {
                Name = "Joe",
                Phone = "(508) 283-5266"
            };

            RegistrationResult result = RegistrationResult.Process(request, new[] { new PhoneValidator() });

            Assert.AreEqual("508", result.PhoneAreaCode);
            Assert.AreEqual("283", result.PhonePrefix);
            Assert.AreEqual("5266", result.PhoneSuffix);
        }

    }
}
