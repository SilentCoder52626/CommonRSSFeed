global using FastEndpoints;
using CommonRSSFeed.DB;
using FastEndpoints.Security;
using FastEndpoints.Swagger;
using Microsoft.EntityFrameworkCore;
using Hangfire;
using CommonRSSFeed.Features.Admin;

var bld = WebApplication.CreateBuilder();
bld.Services
    .AddFastEndpoints()
    .SwaggerDocument();
bld.Services
   .AddAuthenticationJwtBearer(s => s.SigningKey = bld.Configuration["JWTSecret"])
   .AddAuthorization();
bld.Services
    .AddHangfire(x => x.UseInMemoryStorage());

bld.Services.AddHttpClient();

bld.Services.AddTransient<SyncFeedJob>();

bld.Services.AddDbContextFactory<AppDBContext>(options =>
{
    options.UseNpgsql("User ID=postgres;Password=Common@123;Host=localhost;Port=5432;Database=commonRSS;Connection Lifetime=0;");
});

var app = bld.Build();

app.UseAuthentication()
   .UseAuthorization()
   .UseFastEndpoints()
   .UseSwaggerGen();

var sp = app.Services.CreateScope().ServiceProvider;
var syncJob = sp.GetRequiredService<SyncFeedJob>();
var recurringJobs = sp.GetRequiredService<IRecurringJobManager>();

recurringJobs.AddOrUpdate("sync-feeds-concurrently", () => syncJob.FetchFeeds(), Cron.Hourly);


app.Run();
