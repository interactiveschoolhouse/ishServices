using IshServices.Models;
using IshServices.Validators;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace IshServices.Services
{
    public class ContactUsService
    {
        public ContactUsService(MailAdapter mailAdapter)
        {
            this.mailAdapter = mailAdapter;
        }
        private MailAdapter mailAdapter;

        public void Send(ContactUs data)
        {
            ValidateInput(data);

            StringBuilder body = new StringBuilder();
            body.AppendFormat("<div>Name: {0}</div>", data.Name);
            body.AppendFormat("<div>Address: {0}</div>", data.Address);
            body.AppendFormat("<div>Town: {0}</div>", data.Town);
            body.AppendFormat("<div>Phone: {0}</div>", data.Phone);
            body.AppendFormat("<div>Email: {0}</div>", data.Email);
            body.AppendFormat("<div>Comments:</div><p>{0}</p>", data.Comments);

            SiteMessage siteMessage = new SiteMessage()
            {
                From = data.Email,
                To = ConfigurationManager.AppSettings["ContactUsEmailAddress"],
                Subject = ConfigurationManager.AppSettings["ContactUsEmailSubject"],
                Body = body.ToString()
            };

            mailAdapter.Send(siteMessage);
        }

        private void ValidateInput(ContactUs data)
        {
            if (string.IsNullOrWhiteSpace(data.Email) || string.IsNullOrWhiteSpace(data.Comments) || string.IsNullOrWhiteSpace(data.Name))
            {
                throw new ValidationException("Input not valid");
            }

            if (!EmailValidator.IsValid(data.Email))
            {
                throw new ValidationException("Email not valid");
            }
        }

    }
}