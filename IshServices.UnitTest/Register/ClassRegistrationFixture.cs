using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using IshServices.Models;
using System.Linq;
using IshServices.Validators;

namespace IshServices.UnitTest.Register
{
    [TestClass]
    public class ClassRegistrationFixture
    {
        [TestMethod]
        public void RegisterWithNameMissing()
        {
            RegistrationRequest request = new RegistrationRequest()
            {
                Name = null,
            };

            ClassRegistration result = ClassRegistration.Process(request, null);

            Assert.IsFalse(result.IsValid);

            Assert.AreEqual("Name is required", result.GetError("Name"));
        }

        [TestMethod]
        public void RegisterWithFullName()
        {
            RegistrationRequest request = new RegistrationRequest()
            {
                Name = "Shelly Von Trapp"
            };

            ClassRegistration result = ClassRegistration.Process(request, null);

            Assert.AreEqual("Shelly", result.FirstName);
            Assert.AreEqual("Von Trapp", result.LastName);
        }

        [TestMethod]
        public void RegisterWithOnlyFirstNameFilled()
        {
            RegistrationRequest request = new RegistrationRequest()
            {
                Name = "Shelly"
            };

            ClassRegistration result = ClassRegistration.Process(request, null);

            Assert.AreEqual("Shelly", result.FirstName);
        }

        [TestMethod]
        public void RegisterWithPaymentAmountOutsideAllowableRange()
        {
            RegistrationRequest request = new RegistrationRequest()
            {
                PaymentAmount = 600
            };

            ClassRegistration result = ClassRegistration.Process(request, new[] { new PaymentAmountValidator(5, 500) });

            Assert.AreEqual("Payment amount must be between $5 and $500", result.GetError("PaymentAmount"));
        }

        [TestMethod]
        public void RegisterWithValidPaymentAmount()
        {
            RegistrationRequest request = new RegistrationRequest()
            {
                Name = "Sweet Ride",
                PaymentAmount = 300
            };

            ClassRegistration result = ClassRegistration.Process(request, new[] { new PaymentAmountValidator(5, 500) });

            Assert.AreEqual(300, result.PaymentAmount);
        }

        [TestMethod]
        public void RegisterWithInvalidPhoneNumber()
        {
            RegistrationRequest request = new RegistrationRequest()
            {
                Phone = "123"
            };

            ClassRegistration result = ClassRegistration.Process(request, new[] { new PhoneValidator() });

            Assert.AreEqual("A phone number is required", result.GetError("Phone"));
        }

        [TestMethod]
        public void RegisterWithValidShortPhoneNumber()
        {
            RegistrationRequest request = new RegistrationRequest()
            {
                Name = "Joe",
                Phone = "283-5266"
            };

            ClassRegistration result = ClassRegistration.Process(request, new[] { new PhoneValidator() });

            Assert.AreEqual("413", result.PhoneAreaCode);
            Assert.AreEqual("283", result.PhonePrefix);
            Assert.AreEqual("5266", result.PhoneSuffix);
        }

        [TestMethod]
        public void RegisterWithValidFullLengthPhoneNumber()
        {
            RegistrationRequest request = new RegistrationRequest()
            {
                Name = "Joe",
                Phone = "(508) 283-5266"
            };

            ClassRegistration result = ClassRegistration.Process(request, new[] { new PhoneValidator() });

            Assert.AreEqual("508", result.PhoneAreaCode);
            Assert.AreEqual("283", result.PhonePrefix);
            Assert.AreEqual("5266", result.PhoneSuffix);
        }

        [TestMethod]
        public void RegisterWithInvalidEmailAddress()
        {
            RegistrationRequest request = new RegistrationRequest()
            {
                Name = "Joe",
                Email = "Smith@ish"
            };

            ClassRegistration result = ClassRegistration.Process(request, new[] { new EmailValidator() });

            Assert.AreEqual("A valid email address is required", result.GetError("Email"));
        }

        [TestMethod]
        public void RegisterWithValidEmailAddress()
        {
            RegistrationRequest request = new RegistrationRequest()
            {
                Name = "Joe",
                Email = "Smith@ish.com"
            };

            ClassRegistration result = ClassRegistration.Process(request, new[] { new EmailValidator() });

            Assert.AreEqual("Smith@ish.com", result.Email);
        }

        [TestMethod]
        public void RegisterWithOptionalFieldsFilled()
        {
            RegistrationRequest request = new RegistrationRequest()
            {
                Name = "Joe",
                Address = "Address 1",
                Town = "Townie",
                SpecialInstructions = "Environment needs to be quiet.",
                EventName = "Event name",
                EventDescription = "Event description"
            };

            ClassRegistration result = ClassRegistration.Process(request, new Validator<RegistrationRequest>[] { });

            Assert.AreEqual("Address 1", result.Address1);
            Assert.AreEqual("Townie", result.City);
            Assert.AreEqual("Environment needs to be quiet.", result.SpecialInstructions);
            Assert.AreEqual("Event name", result.EventName);
            Assert.AreEqual("Event description", result.EventDescription);
        }
    }
}
