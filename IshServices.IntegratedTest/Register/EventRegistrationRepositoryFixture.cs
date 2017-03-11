using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using IshServices.Data;
using MySql.Data.MySqlClient;
using System.Configuration;
using IshServices.Models;
using IshServices.Validators;

namespace IshServices.IntegratedTest.Register
{
    [TestClass]
    public class EventRegistrationRepositoryFixture
    {
        private UnitOfWork _unitOfWork;
        [TestInitialize]
        public void Setup()
        {
            _unitOfWork = new UnitOfWork(ConfigurationManager.ConnectionStrings["IshDb"].ConnectionString);
        }

        [TestCleanup]
        public void TearDown()
        {
            _unitOfWork.Dispose();
        }

        [TestMethod]
        public void SaveCustomerRegistration()
        {
            ClassRegistrationRepository repo = new ClassRegistrationRepository(_unitOfWork);

            RegistrationRequest request = new RegistrationRequest()
            {
                Name = "Joe Smith",
                SpecialInstructions = "Really needy",
                EventDescription = "Event Description"
            };

            var classRegistration = ClassRegistration.Process(request, new Validator<RegistrationRequest>[] { });

            EventRegistration newRegistration = new EventRegistration()
            {
                RegistrationId = DateTime.Now.ToFileTime(),
                Registration = classRegistration
            };

            repo.Save(newRegistration);

            EventRegistration savedRegistration = repo.Get(newRegistration.RegistrationId);
            Assert.IsNotNull(savedRegistration.Registration);
            Assert.AreEqual("Really needy", savedRegistration.Registration.SpecialInstructions);
            Assert.AreEqual("Event Description", savedRegistration.Registration.EventDescription);
        }
    }
}
