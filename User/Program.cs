using User.Application;
using User.Database;

var builder = WebApplication.CreateBuilder(args);

// API layer
builder.Services.AddOpenApi();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Application layer
builder.Services.AddApplication();

// Database layer
builder.Services.AddDatabase();

// Messaging layer

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