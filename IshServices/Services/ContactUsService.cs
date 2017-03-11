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
        public void Send(ContactUs data)
        {
            ValidateInput(data);

            using(SmtpClient client = new SmtpClient())
            {
                MailMessage message = new MailMessage();

                message.From = new MailAddress(data.Email);
                message.To.Add(new MailAddress(ConfigurationManager.AppSettings["ContactUsEmailAddress"]));

                message.Subject = ConfigurationManager.AppSettings["ContactUsEmailSubject"];

                message.IsBodyHtml = true;

                StringBuilder body = new StringBuilder();
                body.AppendFormat("<div>Name: {0}</div>", data.Name);
                body.AppendFormat("<div>Address: {0}</div>", data.Address);
                body.AppendFormat("<div>Town: {0}</div>", data.Town);
                body.AppendFormat("<div>Phone: {0}</div>", data.Phone);
                body.AppendFormat("<div>Email: {0}</div>", data.Email);
                body.AppendFormat("<div>Comments:</div><p>{0}</p>", data.Comments);

                message.Body = body.ToString();

                if (ConfigurationManager.AppSettings["TestMode"] == "true")
                {
                    System.IO.File.AppendAllText(ConfigurationManager.AppSettings["TestPath"] + "\\sentEmails.html", 
                        string.Format("{0}{1}{0}From:{2}{0}Subject:{3}{0}{4}{0}", 
                        "<br/>", "-------------------------------", data.Email, message.Subject, body.ToString()));
                }
                else
                {
                    client.Send(message);
                }

            }
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