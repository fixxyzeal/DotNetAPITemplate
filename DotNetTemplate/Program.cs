using DotNetTemplate.ConfigureServices;
using Microsoft.AspNetCore.ResponseCompression;
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

app.Run();