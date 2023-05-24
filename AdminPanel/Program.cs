using AdminPanel.Infra.Http;
using AdminPanel.Infra.Http.Configuration;
using AdminPanel.Infra.Jwt;
using AdminPanel.PresentationServices;
using AdminPanel.Services;
using Common.Infra.Auth.Configure;
using Common.Infra.HttpClients;
using Common.Infra.Services.Jwt;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
var mvcBuilder = builder.Services.AddRazorPages();

if (builder.Environment.IsDevelopment())
    mvcBuilder.AddRazorRuntimeCompilation();

builder.Services.AddPresentationServices();

builder.Services.Configure<CookieParametersOptions>(options =>
{
    options.AccessTokenParameterName = "access_token";
    options.RefreshTokenParameterName = "refresh_token";
});

builder.Services.AddSingleton<IPostConfigureOptions<JwtBearerOptions>, ConfigureJwtFromCookie>();
builder.Services.AddCommonJwtServices(builder.Configuration)
    .AddCommonJwtBearerAuth();

builder.Services.AddHttpContextAccessor();

builder.Services.AddCommonHttpClientConfiguration(builder.Configuration,
        AuthHttpClient.Configuration,
        BackendHttpClient.Configuration)
    .AddScoped<IAuthService, AuthHttpClient>()
    .AddScoped<IProfileRepository, AuthHttpClient>()
    .AddScoped<IRestaurantRepository, BackendHttpClient>()
    .AddScoped<IUserRepository, AuthHttpClient>()
    .AddScoped<IUserService, AuthHttpClient>();

var app = builder.Build();

app.UseExceptionHandler(new ExceptionHandlerOptions()
{
    AllowStatusCode404Response = true,
    ExceptionHandlingPath = "/Home/Error"
});

if (!app.Environment.IsDevelopment())
{
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();