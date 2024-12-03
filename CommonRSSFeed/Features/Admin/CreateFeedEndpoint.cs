using CommonRSSFeed.DB;
using FastEndpoints;

namespace CommonRSSFeed.Features.Admin
{
    public class CreateFeedEndpoint : Endpoint<CreateFeedRequest>
    {
        private readonly AppDBContext _context;

        public CreateFeedEndpoint(AppDBContext context)
        {
            _context = context;
        }

        public override void Configure()
        {
            Post("admin/createFeed");
            Roles("Admin");

        }
        public override async Task HandleAsync(CreateFeedRequest req, CancellationToken ct)
        {
            try
            {
                var newFeed = new Feed() { Name = req.Name, Url = req.Url, LastFetchedAt = DateTime.UtcNow };
                await _context.Feeds.AddAsync(newFeed);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                ThrowError(ex.Message, StatusCodes.Status500InternalServerError);
            }
        }
    }

    public record CreateFeedRequest(string Name, string Url);
}
