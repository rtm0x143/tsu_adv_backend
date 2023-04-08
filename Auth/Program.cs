using System.Reflection;
using Auth.Features.RegisterUser;
using Auth.Infra.Data.Configuration;
using Auth.Infra.Services;
using Common.App.Configure;
using Common.App.Configure.Jwt;
using Common.App.Configure.Swagger;
using Common.App.UseCases;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddProblemDetails();

builder.Services.AddCommonVersioning();
builder.Services.AddCommonSwagger();

builder.Services.AddIdentityServices(builder.Configuration);
builder.Services.AddInfraServices(builder.Configuration);

builder.Services.AddCommonJwtBearerAuth();
builder.Services.AddAuthorization();

builder.Services.AddUseCasesFrom(Assembly.GetExecutingAssembly());

var app = builder.Build();

app.UseRouting();

if (app.Environment.IsDevelopment())
{
    app.UseCommonSwagger();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();