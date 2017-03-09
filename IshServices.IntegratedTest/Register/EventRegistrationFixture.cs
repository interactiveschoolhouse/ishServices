using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using IshServices.Data;
using MySql.Data.MySqlClient;
using System.Configuration;
using IshServices.Models;

namespace IshServices.IntegratedTest.Register
{
    [TestClass]
    public class EventRegistrationFixture
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
            EventRegistrationRepository repo = new EventRegistrationRepository(_unitOfWork);

            EventRegistration newRegistration = new EventRegistration()
            {
                RegistrationId = DateTime.Now.ToFileTime(),
                CustomerInfo = new RegistrationRequest()
                {
                    EventName = "Event Name",
                    EventDescription = "Event Description",
                    Name = "Guy Smiley",
                    SpecialInstructions = "Really needy"
                }
            };

            repo.Save(newRegistration);

            EventRegistration savedRegistration = repo.Get(newRegistration.RegistrationId);
            Assert.IsNotNull(savedRegistration.CustomerInfo);
            Assert.AreEqual("Really needy", savedRegistration.CustomerInfo.SpecialInstructions);
            Assert.AreEqual("Event Description", savedRegistration.CustomerInfo.EventDescription);
        }
    }
}
