using Activity.Application;
using Activity.Database;
using Activity.Database.Context;
using Activity.Messaging;
using Microsoft.EntityFrameworkCore;
using Activity.Domain;

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
builder.Services.AddOpenApi();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Application layer
builder.Services.AddApplication();

// Database layer
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? "";
builder.Services.AddDatabase(connectionString);
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=activity.db"));

// Messaging layer
builder.Services.AddMessaging();

// Domain layer
builder.Services.AddDomain();


var app = builder.Build();

app.UseHttpsRedirection();

app.UseCors("AllowSpecificOrigins");

app.UseAuthorization();

app.MapControllers();

app.Run();