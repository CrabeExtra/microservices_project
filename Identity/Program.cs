using Identity.Application;
using Identity.Database;
using Identity.Database.Context;
using Identity.Messaging;
using Microsoft.EntityFrameworkCore;
using Identity.Domain;

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
    options.UseSqlite("Data Source=app.db"));

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