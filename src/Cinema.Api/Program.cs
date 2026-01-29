using Cinema.Api.Extensions;
using Cinema.Api.Middleware;
using Cinema.Application;
using Cinema.Infrastructure;
using Cinema.Infrastructure.Data;
using Microsoft.OpenApi;
using StackExchange.Redis;

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

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("bearer", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        Description = "JWT Authorization header using the Bearer scheme."
    });
    options.AddSecurityRequirement(document => new OpenApiSecurityRequirement
    {
        [new OpenApiSecuritySchemeReference("bearer", document)] = []
    });
});

// Redis
var redisPort = Environment.GetEnvironmentVariable("REDIS_PORT") ?? "6379";
builder.Services.AddStackExchangeRedisCache(opt =>
{
    opt.Configuration = $"localhost:{redisPort}";
    opt.InstanceName = "cinema:";
});
builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
{
    var configuration = $"localhost:{redisPort}";
    return ConnectionMultiplexer.Connect(configuration);
});

// Build app
var app = builder.Build();

// Migration
MigrationRunner.Run(connectionString);

// Middleware
app.UseMiddleware<ExceptionMiddleware>();

// OpenApi
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// JWT
app.UseMiddleware<ExceptionMiddleware>();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();
