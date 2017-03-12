using IshServices.Auth;
using IshServices.Data;
using IshServices.Models;
using IshServices.Validators;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace IshServices.Controllers
{
    public class RegistrationController : ApiController
    {
        // POST api/<controller>
        public IHttpActionResult Post([FromBody]RegistrationRequest request)
        {
            var validators = new Validator<RegistrationRequest>[]
            {
                new PaymentAmountValidator(decimal.Parse(ConfigurationManager.AppSettings["MinPaymentAmount"]),
                decimal.Parse(ConfigurationManager.AppSettings["MaxPaymentAmount"])),
                new PhoneValidator(),
                new EmailValidator()
            };

            ClassRegistration registration = ClassRegistration.Process(request, validators);
            if (registration.IsValid)
            {
                using(UnitOfWork uow = new UnitOfWork(ConfigurationManager.ConnectionStrings["IshDb"].ConnectionString))
                {
                    ClassRegistrationRepository registrationRepo = new ClassRegistrationRepository(uow);

                    registrationRepo.Save(registration);

                    uow.Commit();
                }
            }

            return Ok(registration);
        }

    }
}