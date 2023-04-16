namespace Common.Infra.Services.SMTP;

public class SmtpOptions
{
    public const string ConfigurationSectionName = "Smtp";
    
    public required string UserName { get; set; }
    public required string Password { get; set; }
    public required string Host { get; set; }
    public required int Port { get; set; }
}

