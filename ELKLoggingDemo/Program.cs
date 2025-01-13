using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Sinks.Elasticsearch;
using System;

var builder = WebApplication.CreateBuilder(args);

// ✅ Configure Serilog to Log to Console & Elasticsearch
Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .WriteTo.Console()  // ✅ Logs to console
    .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri("http://localhost:9200"))
    {
        AutoRegisterTemplate = true,
        IndexFormat = "dotnet-logs-{0:yyyy.MM.dd}"
    })
    .CreateLogger();

builder.Host.UseSerilog();

var app = builder.Build();

// ✅ Middleware to Log Requests & Responses
app.Use(async (context, next) =>
{
    Log.Information("➡️ Received request: {Method} {Path}", context.Request.Method, context.Request.Path);

    await next.Invoke();

    Log.Information("⬅️ Response sent: {StatusCode}", context.Response.StatusCode);
});

app.UseHttpsRedirection();

// ✅ Sample Weather Forecast API
var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateTime.Now.AddDays(index),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();

    // ✅ Log API Call
    Log.Information("🌦 Weather forecast requested. Returning {Count} records.", forecast.Length);

    return forecast;
});

// ✅ Error Simulation for ELK Testing
app.MapGet("/error", () =>
{
    Log.Error("❌ Simulated error occurred!");
    throw new Exception("Sample Exception for ELK Logging");
});

app.Run();

internal record WeatherForecast(DateTime Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
