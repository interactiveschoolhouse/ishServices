using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace IshServices.Services
{
    public class SendGridMailAdapter : MailAdapter
    {
        public void Send(SiteMessage siteMessage)
        {
            SendGridMessage sendGridMessage = new SendGridMessage();
            sendGridMessage.SetFrom(new EmailAddress(siteMessage.From));
            sendGridMessage.AddTo(siteMessage.To);
            sendGridMessage.SetSubject(siteMessage.Subject);

            sendGridMessage.AddContent(MimeType.Html, siteMessage.Body);


            if (ConfigurationManager.AppSettings["TestMode"] == "true")
            {
                System.IO.File.AppendAllText(ConfigurationManager.AppSettings["TestPath"] + "\\sentEmails.html",
                    string.Format("{0}{1}{0}From:{2}{0}Subject:{3}{0}{4}{0}",
                    "<br/>", "-------------------------------", siteMessage.From, siteMessage.Subject, siteMessage.Body));
            }
            else
            {
                string apiKey = System.Environment.GetEnvironmentVariable("SENDGRID_APIKEY");
                if (string.IsNullOrEmpty(apiKey))
                {
                    apiKey = ConfigurationManager.AppSettings["SENDGRID_APIKEY"];
                }
                SendGridClient client = new SendGridClient(apiKey);
                Response emailResponse = client.SendEmailAsync(sendGridMessage).Result;
            }

        }
    }
}