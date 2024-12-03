using CommonRSSFeed.DB;
using FastEndpoints;
using FastEndpoints.Security;
using FastEndpoints.Swagger;
using Microsoft.EntityFrameworkCore;

var bld = WebApplication.CreateBuilder();
bld.Services
    .AddFastEndpoints()
    .SwaggerDocument();
bld.Services
   .AddAuthenticationJwtBearer(s => s.SigningKey = bld.Configuration["JWTSecret"])
   .AddAuthorization();
bld.Services.AddDbContextFactory<AppDBContext>(options =>
{
    options.UseNpgsql("User ID=postgres;Password=Common@123;Host=localhost;Port=5432;Database=commonRSS;Connection Lifetime=0;");
});

var app = bld.Build();

app.UseAuthentication()
   .UseAuthorization()
   .UseFastEndpoints()
   .UseSwaggerGen();
app.Run();
