using IshServices.Auth;
using IshServices.Data;
using IshServices.Models;
using IshServices.Services;
using IshServices.Validators;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Text;
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
                StringBuilder body = new StringBuilder();
                body.AppendFormat("<p>This e-mail is a notification that the class registration process has started.  This notification <strong>does not indicate</strong> that the customer has successfully completed payment process through PayPal.  The Registration ID and other contact information can be used to track payment status in PayPal.</p>");
                body.AppendFormat("<div>Registration ID: {0}</div>", registration.RegistrationId);
                body.AppendFormat("<div>Event: {0}</div>", registration.EventName);
                body.AppendFormat("<div>Name: {0}</div>", registration.FullName);
                body.AppendFormat("<div>Payment Amount: {0}</div>", registration.PaymentAmount);
                body.AppendFormat("<div>Address: {0}</div>", registration.Address1);
                body.AppendFormat("<div>Town: {0}</div>", registration.City);
                body.AppendFormat("<div>Phone: {0}</div>", registration.FormattedPhone);
                body.AppendFormat("<div>Email: {0}</div>", registration.Email);
                body.AppendFormat("<div>Special Instructions:</div><p>{0}</p>", registration.SpecialInstructions);

                SiteMessage siteMessage = new SiteMessage()
                {
                    From = registration.Email,
                    To = ConfigurationManager.AppSettings["ContactUsEmailAddress"],
                    Subject = "Class Registration: Interactive School House",
                    Body = body.ToString()
                };

                SendGridMailAdapter sendGridMailAdapter = new SendGridMailAdapter();
                sendGridMailAdapter.Send(siteMessage);
            }

            return Ok(registration);
        }

    }
}