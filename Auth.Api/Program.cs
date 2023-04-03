using Microsoft.AspNetCore.Identity;
using Auth.Infra;
using Auth.Infra.Entities;
using Common.Api.Configure;
using Common.Api.Configure.Jwt;
using Common.Api.Configure.Swagger;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddProblemDetails();

builder.Services.AddCommonVersioning();

builder.Services.AddCommonSwagger();

builder.Services.AddDbContext<AppUserDbContext>();

builder.Services.AddIdentity<AppUser, RoleEntity>()
    .AddEntityFrameworkStores<AppUserDbContext>()
    .AddSignInManager<SignInManager<AppUser>>()
    .AddUserManager<UserManager<AppUser>>()
    .AddRoleManager<RoleManager<RoleEntity>>();

builder.Services.AddJwtBearerAuthentication(builder.Configuration);

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
