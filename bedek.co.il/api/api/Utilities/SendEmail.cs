using System;
using System.Net.Mail;

namespace Model.Utilities
{
    public static class SendEmail
    {
       
        public static bool SendEmailWithGmail(string emailContent, string emailRecipients, string emailSubject, bool addBcc)
        {

            var _emailSendFrom = "bedeksystem@gmail.com";
            var _emailDisplayName = "Bedek.co.il";
            var _emailGmailPsw = "Bedek123QWE";
            var _emailBccRecipients = "eyal.ank@gmail.com";

            var @from = _emailSendFrom;  //Replace this with your own correct Gmail Address

            var to = emailRecipients;
            // //Replace this with the Email Address to whom you want to send the mail

            var mail = new System.Net.Mail.MailMessage();
            mail.To.Add(to);
            mail.From = new System.Net.Mail.MailAddress(@from, _emailDisplayName, System.Text.Encoding.UTF8);


            if (addBcc)
            {
                mail.Bcc.Add(_emailBccRecipients);
            }

            mail.Subject = emailSubject;
            mail.SubjectEncoding = System.Text.Encoding.UTF8;
            mail.Body = emailContent;
            mail.BodyEncoding = System.Text.Encoding.UTF8;
            mail.IsBodyHtml = true;
            mail.Priority = System.Net.Mail.MailPriority.High;


            var client = new SmtpClient
            {
                Credentials = new System.Net.NetworkCredential(@from, _emailGmailPsw),
                Port = 587,
                Host = "smtp.gmail.com",
                EnableSsl = true
            };

            //Add the Creddentials- use  email id and password

            //// Gmail works on this port

            // //Gmail works on Server Secured Layer

            try
            {
                client.Send(mail);
                return true;
            }
            catch (Exception e)
            {
                e.ToString();
                return false;
            }

        }
    }
}
