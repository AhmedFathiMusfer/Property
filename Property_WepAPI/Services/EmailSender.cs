using Property_WepAPI.Services.IServices;
using System.Net;
using System.Net.Mail;

namespace Property_WepAPI.Services
{
    public class EmailSender : IEmailSender
    {
        public async Task SendEmailAsync(string email, string subject, string message)
        {
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress("ahmedfathimusfer@gmail.com");
            mail.To.Add(email);
            mail.Subject = subject;
            mail.Body = message;

            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587); // Port 465 if SSL is used
            smtpClient.Credentials = new NetworkCredential("ahmedfathimusfer@gmail.com", "bdvb nemp akvy xkeg");
            smtpClient.EnableSsl = true;

            smtpClient.Send(mail);
        }
    }
}
