using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using IshServices.Data;
using MySql.Data.MySqlClient;
using System.Configuration;
using IshServices.Models;

namespace IshServices.UnitTest.Calendar
{
    [TestClass]
    public class EventRegistrationFixture
    {
        private MySqlConnection _cn;
        [TestInitialize]
        public void Setup()
        {
            _cn = new MySqlConnection(ConfigurationManager.ConnectionStrings["IshDb"].ConnectionString);
            _cn.Open();
        }

        [TestCleanup]
        public void TearDown()
        {
            _cn.Dispose();
        }

        [TestMethod]
        public void SaveCustomerRegistration()
        {
            EventRegistrationRepository repo = new EventRegistrationRepository(_cn);

            EventRegistration newRegistration = new EventRegistration()
            {
                RegistrationId = DateTime.Now.ToFileTime(),
                CustomerInfo = new CustomerRegistration()
                {
                    Name = "Guy Smiley",
                    SpecialInstructions = "Really needy"
                }
            };

            repo.Save(newRegistration);

            EventRegistration savedRegistration = repo.Get(newRegistration.RegistrationId);
            Assert.IsNotNull(savedRegistration.CustomerInfo);
            Assert.AreEqual("Really needy", savedRegistration.CustomerInfo.SpecialInstructions);
        }
    }
}
