using IshServices.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Web;

namespace IshServices.Services
{
    public class ContactUsService
    {
        public void Send(ContactUs data)
        {
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

                client.Send(message);
            }
        }

    }
}