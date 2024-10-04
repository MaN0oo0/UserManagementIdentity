using Microsoft.AspNetCore.Identity.UI.Services;
using System.Net;
using System.Net.Mail;

namespace UserManagmentWithIdentity.Helpers.EmailServices
{
    public class EmailSender : IEmailSender
    {
        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var fromMail = "";
            var fromPassword = "";
            var message = new MailMessage();
            message.From = new MailAddress(fromMail);
            message.Subject = subject;
            message.To.Add(new MailAddress(email));


            message.Body = $"<html>" +
                                  $"<body>" +
                                    $" {htmlMessage}" +
                         $"</body>" +
                      $"</html>";



            message.IsBodyHtml = true;
            var smtpClient=new SmtpClient();
            smtpClient.Port = 0;
            smtpClient.Host = "";
            smtpClient.Credentials=new NetworkCredential(fromMail,fromPassword);
            smtpClient.EnableSsl = true;

           await smtpClient.SendMailAsync(message);
        }
    }
}
