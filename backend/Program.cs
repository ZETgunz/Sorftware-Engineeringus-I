using backend.Models;
using backend.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.Features;
using backend.Interfaces;
using backend.Repositories;
using backend.Services;
using Serilog;
using Serilog.Events;

var builder = WebApplication.CreateBuilder(args);


Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("logs/log.txt", rollingInterval: RollingInterval.Hour, restrictedToMinimumLevel: LogEventLevel.Warning)
    .CreateLogger();

builder.Host.UseSerilog();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontendApp",
        policy =>
        {
            policy.WithOrigins("http://localhost:5173")
                  .AllowAnyHeader()
                  .AllowAnyMethod()
                  .AllowCredentials();
        });
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddSingleton<SessionManager>();
builder.Services.AddHostedService<SessionCleanupService>();


// add database context
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// add repositories
builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<IGameRepository, GameRepository>();
builder.Services.AddScoped<ILeaderboardRepository, LeaderboardRepository>();
builder.Services.AddScoped<IValidator<Account>, Validator<Account>>();
builder.Services.AddScoped<IValidator<Game>, Validator<Game>>();

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowFrontendApp");
app.MapControllers();

app.Run();

public partial class Program { }