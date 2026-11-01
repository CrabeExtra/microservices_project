using Personal.Application;
using Personal.Database;
using Personal.Database.Context;
using Personal.Messaging;
using Personal.Api.Middleware;
using Microsoft.EntityFrameworkCore;

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
    options.UseSqlite("Data Source=contact.db")); // TODO add connection string variable not floating string.

// Messaging layer
builder.Services.AddMessaging();

// Domain layer
//builder.Services.AddDomain();

var app = builder.Build();

// middlewares

app.UseMiddleware<ExceptionMiddleware>();

//env
DotNetEnv.Env.Load();

app.UseHttpsRedirection();

app.UseCors("AllowSpecificOrigins");

app.UseAuthorization();

app.MapControllers();

app.Run();