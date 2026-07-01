using Audit.Application;
using Audit.Database;
using Audit.Database.Context;
using Audit.Messaging;
using Audit.Api.Middleware;
using Microsoft.EntityFrameworkCore;
using Audit.Domain;

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
builder.Services.AddDatabase(connectionString);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=audit.db")); // TODO add connection string variable not floating string.

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