using FastEndpoints;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace CommonRSSFeed.Features
{
    public class HealthEndPoint : Endpoint<HealthRequest,HealthResponse>
    {
        public override void Configure()
        {
            Get("/api/health");
            AllowAnonymous();
        }
        public override Task<HealthResponse> ExecuteAsync(HealthRequest req, CancellationToken ct)
        {
            return Task.FromResult( new HealthResponse
            {
                AllCaps = req.Check.ToUpper()
            });
        }
    }

    public record HealthRequest
    {
        public string Check { get; set; } = "";
    }

    public class HealthResponse
    {
        public string AllCaps { get; set; } = "";

    }
}
