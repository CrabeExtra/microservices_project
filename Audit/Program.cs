using Audit.Application;
using Audit.Database;
using Audit.Database.Context;
using Audit.Messaging;
using Audit.Api.Middleware;
using Microsoft.EntityFrameworkCore;
using Audit.Domain;

//env
DotNetEnv.Env.Load();

var builder = WebApplication.CreateBuilder(args);

// API layer

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Authorisation

builder.Services.AddAuthorization();

// Application layer
builder.Services.AddApplication();

// Database layer
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? "";
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(connectionString));

// Messaging layer
builder.Services.AddMessaging();

// Domain layer
builder.Services.AddDomain();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await db.Database.MigrateAsync();
}

app.UseMiddleware<ExceptionMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();