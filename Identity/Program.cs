using Identity.Application;
using Identity.Database;
using Identity.Database.Context;
using Identity.Messaging;
using Identity.Api.Middleware;
using Microsoft.EntityFrameworkCore;
using Identity.Domain;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

// API layer

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Authorisation

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            RoleClaimType = ClaimTypes.Role
        };
    });

builder.Services.AddAuthorization();

// Application layer
builder.Services.AddApplication();

// Database layer
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? "";
builder.Services.AddDatabase(connectionString);
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=identity.db"));

// Messaging layer
builder.Services.AddMessaging();

// Domain layer
builder.Services.AddDomain();

var app = builder.Build();

// middlewares
if(app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}
app.UseMiddleware<ExceptionMiddleware>();

//env
DotNetEnv.Env.Load();


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();