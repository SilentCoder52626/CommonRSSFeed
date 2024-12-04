using CommonRSSFeed.DB;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;

namespace CommonRSSFeed.Features
{
    public class UnSubscribeToFeedEndPoint : Endpoint <UnSubscribeToFeedRequest>
    {
        private readonly AppDBContext _context;

        public UnSubscribeToFeedEndPoint(AppDBContext context)
        {
            _context = context;
        }

        public override void Configure()
        {
            Post("api/feeds/unsubscribe");
        }
        public override async Task HandleAsync(UnSubscribeToFeedRequest req, CancellationToken ct)
        {
            try
            {
                await _context.Subscriptions.Where(a=>a.UserId == User.ToTokenUser().Id && a.FeedId == req.FeedId).ExecuteDeleteAsync();

            }
            catch (Exception ex)
            {
                ThrowError(ex.Message, StatusCodes.Status500InternalServerError);
            }
        }
    }

    public record UnSubscribeToFeedRequest(Guid FeedId);
}
