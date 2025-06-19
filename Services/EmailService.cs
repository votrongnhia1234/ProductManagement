using MimeKit;
using MailKit.Net.Smtp;
namespace ProductManagement.Services
{
    public class EmailService
    {
        public async Task SendEmailAsync(string to, string subject, string body)
        {
            var host = Environment.GetEnvironmentVariable("SMTP_HOST");
            var port = int.Parse(Environment.GetEnvironmentVariable("SMTP_PORT") ?? "587");
            var user = Environment.GetEnvironmentVariable("SMTP_USER");
            var pass = Environment.GetEnvironmentVariable("SMTP_PASS");

            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(user));
            email.To.Add(MailboxAddress.Parse(to));
            email.Subject = subject;
            email.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = body };

            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(host, port, MailKit.Security.SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(user, pass);
            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);
        }
    }
}
