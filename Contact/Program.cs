using Contact.Application;
using Contact.Database;
using Contact.Database.Context;
using Contact.Messaging;
using Contact.Api.Middleware;
using Microsoft.EntityFrameworkCore;
using Contact.Domain;

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
if(app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseMiddleware<ExceptionMiddleware>();

//env
DotNetEnv.Env.Load();

app.UseHttpsRedirection();

app.UseCors("AllowSpecificOrigins");

app.UseAuthorization();

app.MapControllers();

app.Run();