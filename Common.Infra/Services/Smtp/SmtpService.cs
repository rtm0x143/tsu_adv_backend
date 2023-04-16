using System.Net;
using System.Net.Mail;
using Common.App.Services;
using Microsoft.Extensions.Options;

namespace Common.Infra.Services.SMTP;

public class SmtpService : IMailSender
{
    private readonly SmtpOptions _options;
    private SmtpClient _smtpClient;

    public SmtpService(IOptions<SmtpOptions> options)
    {
        _options = options.Value;
        _smtpClient = new SmtpClient(_options.Host, _options.Port)
        {
            Credentials = new NetworkCredential(_options.UserName, _options.Password),
            EnableSsl = true
        };
    }


    public Task Send(MailMessage message)
    {
        message.From ??= new MailAddress(_options.UserName);
        return _smtpClient.SendMailAsync(message);
    }
}