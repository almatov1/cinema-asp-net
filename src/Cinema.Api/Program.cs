using Cinema.Api.DependencyInjection;
using Cinema.Api.Middleware;
using Cinema.Infrastructure.Data;

DotNetEnv.Env.Load("../../.env");

// Migration
var port = Environment.GetEnvironmentVariable("POSTGRESQL_PORT");
var db = Environment.GetEnvironmentVariable("POSTGRESQL_DB");
var user = Environment.GetEnvironmentVariable("POSTGRESQL_USER");
var pass = Environment.GetEnvironmentVariable("POSTGRESQL_PASSWORD");
var connectionString = $"Host=localhost;Port={port};Database={db};Username={user};Password={pass}";

// Builder
var builder = WebApplication.CreateBuilder(args);
Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
builder.Services.AddApplicationServices(connectionString);
builder.Services.AddInfrastructureServices();
builder.Services.AddControllers();

// Build app
var app = builder.Build();
MigrationRunner.Run(connectionString);
app.MapControllers();
app.UseMiddleware<ExceptionMiddleware>();
if (app.Environment.IsDevelopment())
    app.MapOpenApi();
app.UseHttpsRedirection();
app.Run();
