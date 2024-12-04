
using CommonRSSFeed.DB;
using CommonRSSFeed.Models;
using Microsoft.EntityFrameworkCore;

namespace CommonRSSFeed.Features
{
    public class GetNewestPostEndpoint : Endpoint<GetNewestPostRequest, GetNewestPostResponse>
    {
        private readonly AppDBContext _context;

        public GetNewestPostEndpoint(AppDBContext context)
        {
            _context = context;
        }

        public override void Configure()
        {
            Post("/api/posts/new");
        }

        public override async Task<GetNewestPostResponse> ExecuteAsync(GetNewestPostRequest req, CancellationToken ct)
        {
            var posts = await _context.Posts.Where(a => a.Feed.Users.Any(c => c.Id == User.ToTokenUser().Id)).OrderByDescending(a => a.PublishedAt).Take(req.Limit ?? 100)
                .Select(a=> new PostDto()
                {
                    Title = a.Title,
                    Content = a.Description,
                    PublishedAt = a.PublishedAt,
                    FeedName = a.Feed.Name,
                    Url = a.Url
                }).ToListAsync();

            return new GetNewestPostResponse()
            {
                Posts = posts
            };
        }
    }

    public class GetNewestPostRequest
    {
        public int? Limit { get; set; }
    }

    public class GetNewestPostResponse
    {
        public List<PostDto> Posts { get; set; } = new List<PostDto>();
    }

}
