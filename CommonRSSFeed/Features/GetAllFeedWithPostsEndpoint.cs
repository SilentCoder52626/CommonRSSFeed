using CommonRSSFeed.DB;
using CommonRSSFeed.Models;
using Microsoft.EntityFrameworkCore;

namespace CommonRSSFeed.Features
{
    public class GetAllFeedWithPostsEndpoint : EndpointWithoutRequest<GetFeedWithPostsResponse>
    {
        private readonly AppDBContext _context;

        public GetAllFeedWithPostsEndpoint(AppDBContext context)
        {
            _context = context;
        }
        public override void Configure()
        {
            Get("api/feed-with-posts");
        }
        public override async Task<GetFeedWithPostsResponse> ExecuteAsync(CancellationToken ct)
        {
            var feeds = await _context.Feeds.Include(a=>a.Posts).ToListAsync();
            var feedDtos = feeds.Select(a => new FeedDto()
            {
                Id = a.Id,
                Name = a.Name,
                Posts = a.Posts.Select(c => new PostDto()
                {
                    Title = c.Title,
                    Content = c.Description,
                    PublishedAt = c.PublishedAt,
                    FeedName = a.Name,
                    Url = c.Url
                }).ToList()
            }).ToList();
            return new GetFeedWithPostsResponse(feedDtos);
        }
    }
    public record GetFeedWithPostsResponse(List<FeedDto> feeds);
}
