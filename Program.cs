using CodebridgeTest.Models;
using CodebridgeTest.Services.Interfaces;
using CodebridgeTest.Services;
using Microsoft.EntityFrameworkCore;
using CodebridgeTest.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IDogsService, DogsService>();

builder.Services.AddDbContext<DogDBContext>(options =>
{
    options.UseSqlServer("Server=DESKTOP-PDNGHNS\\SQLEXPRESS;Database=DogsDB;" +
        "Trusted_Connection=True;TrustServerCertificate=True");
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<RateLimitMiddleware>();

app.UseAuthorization();

app.MapControllers();

app.Run();