using DotNetTemplate.ConfigureServices;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.IO.Compression;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// load .env file
DotNetEnv.Env.Load();

builder.Services.RegisterServiceLayerDi();
builder.Services.ConfigJWT();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Add MongoDB Connection
builder.Services.ConfigMongoDb();

//Add Response Compress with BrotilCompression
builder.Services.AddResponseCompression();
builder.Services.Configure<BrotliCompressionProviderOptions>(options =>
{
    options.Level = CompressionLevel.Fastest;
});

// Config Swagger Custom Template
builder.Services.ConfigSwagger();

// Add Health Check + DB Health Check
builder.Services.AddHealthChecks()
       .AddMongoDb(
            name: "mongodb",
            mongodbConnectionString: Environment.GetEnvironmentVariable("MongoConnection") ?? string.Empty,
            failureStatus: HealthStatus.Unhealthy, // Set the status for a failed health check
            tags: new[] { "mongodb" }); // Optional tags for organizing health checks

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapHealthChecks("/hc", new HealthCheckOptions
{
    // Use the custom response writer
    ResponseWriter = MultiHealthCheckResponseWriter.WriteResponse,
});

await app.RunAsync();