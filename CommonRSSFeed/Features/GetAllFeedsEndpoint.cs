using CommonRSSFeed.DB;
using CommonRSSFeed.Models;
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
            var feeds = await _context.Feeds.Include(a=>a.Posts).ToListAsync();
            var feedDtos = feeds.Select(a => new FeedDto()
            {
                Id = a.Id,
                Name = a.Name,
                
            }).ToList();
            return new GetFeedsResponse(feedDtos);
        }
    }
    public record GetFeedsResponse(List<FeedDto> feeds);
}
