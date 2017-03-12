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

            repo.Save(classRegistration);

            ClassRegistration savedRegistration = repo.Get(classRegistration.RegistrationId);
            Assert.IsNotNull(savedRegistration.RegistrationId);
            Assert.AreEqual("Really needy", savedRegistration.SpecialInstructions);
            Assert.AreEqual("Event Description", savedRegistration.EventDescription);
        }
    }
}
