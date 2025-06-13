using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using MailKit.Security;

public class EmailService
{
    public async Task SendEmailAsync(string accessToken, string fromEmail, string toEmail, string subject, string body)
    {
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress(fromEmail, fromEmail));
        message.To.Add(new MailboxAddress(toEmail, toEmail));
        message.Subject = subject;
        message.Body = new TextPart("plain") { Text = body };

        using var client = new SmtpClient();
        await client.ConnectAsync("smtp.gmail.com", 587, SecureSocketOptions.StartTls);

        var oauth2 = new SaslMechanismOAuth2(fromEmail, accessToken);
        await client.AuthenticateAsync(oauth2);

        await client.SendAsync(message);
        await client.DisconnectAsync(true);
    }
}
