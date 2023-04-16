using System.Net.Mail;

namespace Common.App.Services;

public interface IMailSender
{
    public Task Send(MailMessage message);
} 