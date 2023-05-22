namespace AdminPanel.PresentationServices;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddPresentationServices(this IServiceCollection services)
    {
        return services.AddScoped<IMvcErrorHandler, UnauthorizedMvcErrorHandler>();
    }
}