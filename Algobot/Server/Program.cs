using Algo.Bot.Infrastructure;
using Algo.Bot.Application;
using Algo.Bot.Application.Ports.Storage;
using Algo.Bot.Domain.Models;
using BlazorAlgoBot.Server.Authorization;
using System.Security.Claims;
using BlazorAlgoBot.Server.BackgroundJobs;
using Algo.Bot.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using System;
using Algo.Bot.Domain.ValueObject;
using Microsoft.AspNetCore.Builder;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddHttpContextAccessor();
builder.Services.AddAuthentication(ApiKeySchemeOptions.SchemaName).AddScheme<ApiKeySchemeOptions, ApiKeyHandler>(ApiKeySchemeOptions.SchemaName, (options) =>
{
    builder.Configuration.GetRequiredSection(ApiKeySchemeOptions.Section).Bind(options);
});
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(ApiKeySchemeOptions.PolicyName, policy => policy.RequireClaim(ClaimTypes.NameIdentifier));
});

builder.Services.AddHostedService<TimedHostedService>();
builder.Services.AddDbContextFactory<AlgobotDbContext>(
    options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddDbContext<AlgobotDbContext>(
    options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

using var scope = app.Services.CreateScope();
await using var dbContext = scope.ServiceProvider.GetRequiredService<AlgobotDbContext>();
await dbContext.Database.MigrateAsync();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();
app.UseRouting();

app.UseCors();
app.UseAuthentication();
app.UseAuthorization();

var coins = new List<Coin> 
{
    new Coin("BTCUSDT", Interval.FifteenMinutes, DateTime.UtcNow, new List<Candle>
    {
        new Candle 
        {
            DateTime = DateTime.UtcNow.AddSeconds((int)Interval.FifteenMinutes),
            Interval = Interval.FifteenMinutes,
            Symbol = "BTCUSDT",
            High = 2200,
            Close = 2100,
            Open = 2000,
            Low = 1900
        },
        new Candle
        {
            DateTime = DateTime.UtcNow.AddSeconds((int)Interval.FifteenMinutes * 2),
            Interval = Interval.FifteenMinutes,
            Symbol = "BTCUSDT",
            High = 2200,
            Close = 2100,
            Open = 2000,
            Low = 1900
        },
    }),
    new Coin("ETHUSDT", Interval.FifteenMinutes, DateTime.UtcNow, new List<Candle>
    {
        new Candle
        {
            DateTime = DateTime.UtcNow.AddSeconds((int)Interval.FifteenMinutes),
            Interval = Interval.FifteenMinutes,
            Symbol = "ETHUSDT",
            High = 220,
            Close = 210,
            Open = 200,
            Low = 190
        },
        new Candle
        {
            DateTime = DateTime.UtcNow.AddSeconds((int)Interval.FifteenMinutes * 2),
            Interval = Interval.FifteenMinutes,
            Symbol = "BTCUSDT",
            High = 320,
            Close = 210,
            Open = 200,
            Low = 190
        },
    }),
};

app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();
