using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Web;

namespace IshServices.Services
{
    public class SmtpMailAdapter : MailAdapter
    {
        public void Send(SiteMessage siteMessage)
        {
            using (SmtpClient client = new SmtpClient())
            {
                MailMessage message = new MailMessage();

                message.From = new MailAddress(siteMessage.From);
                message.To.Add(new MailAddress(siteMessage.To));

                message.Subject = siteMessage.Subject;

                message.IsBodyHtml = true;
                message.Body = siteMessage.Body;

                if (ConfigurationManager.AppSettings["TestMode"] == "true")
                {
                    System.IO.File.AppendAllText(ConfigurationManager.AppSettings["TestPath"] + "\\sentEmails.html",
                        string.Format("{0}{1}{0}From:{2}{0}Subject:{3}{0}{4}{0}",
                        "<br/>", "-------------------------------", siteMessage.From, siteMessage.Subject, siteMessage.Body));
                }
                else
                {
                    client.Send(message);
                }
            }
        }
    }
}