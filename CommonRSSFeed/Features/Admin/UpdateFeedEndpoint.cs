using CommonRSSFeed.DB;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;

namespace CommonRSSFeed.Features.Admin
{
    public class UpdateFeedEndpoint : Endpoint<UpdateFeedRequest>
    {
        private readonly AppDBContext _context;

        public UpdateFeedEndpoint(AppDBContext context)
        {
            _context = context;
        }

        public override void Configure()
        {
            Post("admin/updateFeed");
            Roles("Admin");

        }
        public override async Task HandleAsync(UpdateFeedRequest req, CancellationToken ct)
        {
            try
            {
                var feed = await _context.Feeds.FirstOrDefaultAsync(x => x.Id == req.Id);
                if(feed is null) ThrowError("Feed not found",StatusCodes.Status404NotFound);
               
                feed.Name = req.Name;
                feed.Url = req.Url;

                _context.Feeds.Update(feed);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                ThrowError(ex.Message, StatusCodes.Status500InternalServerError);
            }
        }
    }

    public record UpdateFeedRequest(Guid Id,string Name, string Url);
}
