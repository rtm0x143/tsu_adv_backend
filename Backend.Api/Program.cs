using Common.App.Configure;
using Common.App.Configure.Jwt;
using Common.App.Configure.Swagger;

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