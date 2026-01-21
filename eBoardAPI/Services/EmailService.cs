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
            Host = _config["Email:Host"]!,
            Port = int.Parse(_config["Email:Port"]!),
            EnableSsl = true,
            Credentials = new NetworkCredential(
                _config["Email:Username"],
                _config["Email:Password"]
            )
        };

        var mail = new MailMessage
        {
            From = new MailAddress(_config["Email:From"]!),
            Subject = subject,
            Body = htmlBody,
            IsBodyHtml = true
        };

        mail.To.Add(to);

        await smtpClient.SendMailAsync(mail);
    }
}
