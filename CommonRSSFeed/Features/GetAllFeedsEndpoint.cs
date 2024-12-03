using CommonRSSFeed.DB;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;

namespace CommonRSSFeed.Features
{
    public class GetAllFeedsEndpoint : EndpointWithoutRequest<GetFeedsResponse>
    {
        private readonly AppDBContext _context;

        public GetAllFeedsEndpoint(AppDBContext context)
        {
            _context = context;
        }
        public override void Configure()
        {
            Get("api/feeds");
        }
        public override async Task<GetFeedsResponse> ExecuteAsync(CancellationToken ct)
        {
            var feeds = await _context.Feeds.ToListAsync();
            var feedDtos = feeds.Select(c => new FeedDto(c.Id, c.Name)).ToList();
            return new GetFeedsResponse(feedDtos);
        }
    }
    public record GetFeedsResponse(List<FeedDto> feeds);

    public record FeedDto(Guid Id, string Name);
}
