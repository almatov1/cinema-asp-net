using Cinema.Application.Services;
using Cinema.Domain.Interfaces;
using Cinema.Infrastructure.Data;
using Cinema.Infrastructure.Repositories;

DotNetEnv.Env.Load("../../.env");

// Migration
var port = Environment.GetEnvironmentVariable("POSTGRESQL_PORT");
var db = Environment.GetEnvironmentVariable("POSTGRESQL_DB");
var user = Environment.GetEnvironmentVariable("POSTGRESQL_USER");
var pass = Environment.GetEnvironmentVariable("POSTGRESQL_PASSWORD");
var connectionString = $"Host=localhost;Port={port};Database={db};Username={user};Password={pass}";

// Builder
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton(new DbConnectionFactory(connectionString));
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<UserService>();
builder.Services.AddControllers();

// Build app
var app = builder.Build();
MigrationRunner.Run(connectionString);
app.MapControllers();
if (app.Environment.IsDevelopment())
    app.MapOpenApi();
app.UseHttpsRedirection();
app.Run();
