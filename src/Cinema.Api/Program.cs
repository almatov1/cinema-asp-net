using Cinema.Api.Extensions;
using Cinema.Api.Middleware;
using Cinema.Application;
using Cinema.Infrastructure;
using Cinema.Infrastructure.Data;

// Env
DotNetEnv.Env.Load("../../.env");
var port = Environment.GetEnvironmentVariable("POSTGRESQL_PORT");
var db = Environment.GetEnvironmentVariable("POSTGRESQL_DB");
var user = Environment.GetEnvironmentVariable("POSTGRESQL_USER");
var pass = Environment.GetEnvironmentVariable("POSTGRESQL_PASSWORD");
var connectionString = $"Host=localhost;Port={port};Database={db};Username={user};Password={pass}";

// Builder
var builder = WebApplication.CreateBuilder(args);

// Dapper snake_case mapping
Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;

// Services
builder.Services.AddInfrastructure(connectionString);
builder.Services.AddApplication();
builder.Services.AddWebAuth();
builder.Services.AddControllers();

// Build app
var app = builder.Build();

// Migration
MigrationRunner.Run(connectionString);

// Middleware
app.UseMiddleware<ExceptionMiddleware>();

// Swagger
if (app.Environment.IsDevelopment())
    app.MapOpenApi();

app.UseHttpsRedirection();

// JWT
app.UseMiddleware<ExceptionMiddleware>();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();
