using Activity.Application;
using Activity.Database;
using Activity.Database.Context;
using Activity.Messaging;
using Microsoft.EntityFrameworkCore;
using Activity.Domain;

var builder = WebApplication.CreateBuilder(args);

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

// middlewares
if(app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();