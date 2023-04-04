using Common.Api.Configure;
using Common.Api.Configure.Jwt;
using Common.Api.Configure.Swagger;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddProblemDetails();

builder.Services.AddCommonVersioning();

builder.Services.AddCommonSwagger();

builder.Services.AddCommonJwtBearerAuth();

builder.Services.AddAuthorization();

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