using CommonRSSFeed.DB;
using FastEndpoints;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace CommonRSSFeed.Features
{
    public class SubscribeToFeedEndPoint : Endpoint <SubscribeToFeedRequest>
    {
        private readonly AppDBContext _context;

        public SubscribeToFeedEndPoint(AppDBContext context)
        {
            _context = context;
        }

        public override void Configure()
        {
            Post("api/feeds/subscribe");
        }
        public override async Task HandleAsync(SubscribeToFeedRequest req, CancellationToken ct)
        {
            try
            {
                var user = User.ToTokenUser();

                if (_context.Subscriptions.Any(a => a.FeedId == req.FeedId && a.UserId == user.Id))
                    ThrowError("Subscribtion already added.", StatusCodes.Status409Conflict);

                var subs = new Subscription()
                {
                    FeedId = req.FeedId,
                    UserId = user.Id,
                    SubscribedAt = DateTime.UtcNow
                };
                await _context.Subscriptions.AddAsync(subs);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                ThrowError(ex.Message, StatusCodes.Status500InternalServerError);
            }
        }
    }

    public record SubscribeToFeedRequest(Guid FeedId);
}
