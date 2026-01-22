using eBoardAPI.Consts;
using eBoardAPI.Interfaces.Services;
using System.Net;
using System.Net.Mail;

public class EmailService : IEmailService
{
    private readonly IConfiguration _config;

    public EmailService(IConfiguration config)
    {
        _config = config;
    }

    public async Task SendAsync(string to, string subject, string htmlBody)
    {
        var smtpClient = new SmtpClient
        {
            Host = Environment.GetEnvironmentVariable(EnvKey.EMAIL_HOST) ?? "",
            Port = int.Parse(Environment.GetEnvironmentVariable(EnvKey.EMAIL_PORT)!),
            EnableSsl = bool.Parse(Environment.GetEnvironmentVariable(EnvKey.EMAIL_ENABLE_SSL)!),
            Credentials = new NetworkCredential(
                Environment.GetEnvironmentVariable(EnvKey.EMAIL_USERNAME)!,
                Environment.GetEnvironmentVariable(EnvKey.EMAIL_PASSWORD)!
            )
        };

        var mail = new MailMessage
        {
            From = new MailAddress(Environment.GetEnvironmentVariable(EnvKey.EMAIL_FROM)!),
            Subject = subject,
            Body = htmlBody,
            IsBodyHtml = true
        };

        mail.To.Add(to);

        await smtpClient.SendMailAsync(mail);
    }
}
