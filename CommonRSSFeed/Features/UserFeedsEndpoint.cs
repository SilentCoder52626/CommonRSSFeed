using CommonRSSFeed.DB;
using CommonRSSFeed.Models;
using Microsoft.EntityFrameworkCore;

namespace CommonRSSFeed.Features
{


    public class UserFeedsEndpoint : EndpointWithoutRequest< UserFeedsResponse>
    {
        private readonly AppDBContext _context;

        public UserFeedsEndpoint(AppDBContext context)
        {
            _context = context;
        }

        public override void Configure()
        {
            Get("/api/feed/me");
        }

        public override async Task<UserFeedsResponse> ExecuteAsync( CancellationToken ct)
        {

            var userId = User.ToTokenUser().Id;
            var user = await _context.AppUsers.Include(c => c.Feeds).FirstAsync(a => a.Id == userId);

            var feedDtos = user.Feeds.Select(a => new FeedDto()
            {
                Id = a.Id,
                Name = a.Name
            }).ToList();
            return new UserFeedsResponse { Feeds = feedDtos };
        }
    }

    public class UserFeedsResponse
    {
        public List<FeedDto> Feeds { get; set; } = new List<FeedDto>();
    }

}
