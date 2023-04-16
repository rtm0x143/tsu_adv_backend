using Common.App.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Common.Infra.Services.SMTP;

public static class SmtpServicesExtension
{
    public static IServiceCollection AddCommonSmtpServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        return services.Configure<SmtpOptions>(configuration.GetSection(SmtpOptions.ConfigurationSectionName))
            .AddScoped<IMailSender, SmtpService>();
    }
}