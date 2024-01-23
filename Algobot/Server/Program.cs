using Algo.Bot.Infrastructure;
using Algo.Bot.Application;
using BlazorAlgoBot.Server.Authorization;
using System.Security.Claims;
using Algo.Bot.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Hangfire;
using BlazorAlgoBot.Server.BackgroundJobs;
using Algobot.Server.BackgroundJobs;

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

builder.Services.AddSingleton<ICoinIntervalJob, CoinIntervalJob>();
builder.Services.AddHostedService<CoinDataProviderJob>();

builder.Services.AddDbContextFactory<AlgobotDbContext>(
    options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddDbContext<AlgobotDbContext>(
    options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddHangfire(configuration => configuration
            .UseSimpleAssemblyNameTypeSerializer()
            .UseRecommendedSerializerSettings()
            .UseSqlServerStorage("Server=localhost,8002; Database=AlgobotDatabase; User Id=sa; Password=2c1b06b2-b286-47b5-ad77-60bf0aa6a13a;"));

// Add the processing server as IHostedService
builder.Services.AddHangfireServer();

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

app.UseHangfireDashboard();
app.UseHttpsRedirection();
app.UseBlazorFrameworkFiles();
app.UseStaticFiles();
app.UseRouting();
app.UseCors();
app.UseAuthentication();
app.UseAuthorization();

app.MapHangfireDashboard();
app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();
