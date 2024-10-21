using MimeKit;

namespace HolaHousing_BE.Models
{
    public class EmailServices
    {
        private readonly string _smtpServer = "smtp.gmail.com";
        private readonly int _smtpPort = 587;
        private readonly string _username = "hola.housing.1306@gmail.com";
        private readonly string _password = "vaap gqqy pvta qklp";

        public async Task SendEmailAsync(string toEmail, string subject, string message)
        {
            var email = new MimeMessage();
            email.From.Add(new MailboxAddress("Hola Housing", _username));
            email.To.Add(new MailboxAddress("", toEmail));
            email.Subject = subject;

            var body = new TextPart("plain")
            {
                Text = message
            };

            email.Body = body;

            using (var client = new MailKit.Net.Smtp.SmtpClient())
            {
                await client.ConnectAsync(_smtpServer, _smtpPort, MailKit.Security.SecureSocketOptions.StartTls);
                await client.AuthenticateAsync(_username, _password);

                await client.SendAsync(email);
                await client.DisconnectAsync(true);
            }
        }
    }
}
