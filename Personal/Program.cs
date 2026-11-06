using Personal.Application;
using Personal.Database;
using Personal.Database.Context;
using Personal.Messaging;
using Personal.Api.Middleware;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

//env
DotNetEnv.Env.Load();

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options => options.AddPolicy("AllowSpecificOrigins", policy =>
    policy.WithOrigins(
        "https://judebradbury.com",
        "https://www.judebradbury.com",
        "http://localhost:3001"
    )
    .AllowAnyMethod()
    .AllowAnyHeader()
));

// API layer

builder.Services
    .AddControllers()
    .AddJsonOptions(o =>
        {
            o.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        });
builder.Services.AddEndpointsApiExplorer();

// Authorisation

builder.Services.AddAuthorization();

// Application layer
builder.Services.AddApplication();

// Database layer
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? "";
builder.Services.AddDatabase(connectionString);

// Messaging layer
builder.Services.AddMessaging();

// Domain layer
//builder.Services.AddDomain();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await db.Database.MigrateAsync();
}

// middlewares

app.UseMiddleware<ExceptionMiddleware>();

app.UseHttpsRedirection();

app.UseCors("AllowSpecificOrigins");

app.UseAuthorization();

app.MapControllers();

app.Run();