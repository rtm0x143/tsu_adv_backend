using AdminPanel.Infra.Http;
using AdminPanel.Services;
using Common.Infra.HttpClients;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

var mvcBuilder = builder.Services.AddRazorPages();
if (builder.Environment.IsDevelopment()) 
    mvcBuilder.AddRazorRuntimeCompilation();

builder.Services.AddCommonHttpClientConfiguration(builder.Configuration)
    .AddScoped<IAuthService, AuthHttpClient>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
