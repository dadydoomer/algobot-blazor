using Algo.Bot.Infrastructure;
using Algo.Bot.Application;
using BlazorAlgoBot.Server.Authorization;
using System.Security.Claims;
using Algo.Bot.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Hangfire;
using BlazorAlgoBot.Server.BackgroundJobs;
using Algobot.Server.BackgroundJobs;
using Algobot.Server.Authorization;

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
            .UseSqlServerStorage(builder.Configuration.GetConnectionString("DefaultConnection")));

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

app.UseHangfireDashboard("/hangfire", new DashboardOptions 
{
    Authorization = new[] { new DashboardAuthorizationFilter() }
});
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
