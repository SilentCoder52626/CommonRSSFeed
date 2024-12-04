using CommonRSSFeed.DB;
using Hangfire;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;

namespace CommonRSSFeed.Features.Admin
{
    public class SyncFeedJob
    {
        private readonly AppDBContext _context;
        private readonly IHttpClientFactory _httpFactory;
        public SyncFeedJob(AppDBContext context, IHttpClientFactory httpFactory)
        {
            _context = context;
            _httpFactory = httpFactory;
        }

        public async Task FetchFeeds()
        {
            var dbFeeds = await _context.Feeds.ToListAsync();

            foreach(var feed in dbFeeds)
            {
                await FetchSingleFeed(feed.Id);
            }


        }
        public async Task FetchSingleFeed(Guid id)
        {
            var dbFeed = await _context.Feeds.Include(a => a.Posts).Where(c => c.Id == id).FirstOrDefaultAsync();

            var httpClient = _httpFactory.CreateClient();

            var xmlString = await httpClient.GetStringAsync(dbFeed.Url);

            var serializer = new XmlSerializer(typeof(RssRoot));
            using var reader = new StringReader(xmlString);

            var feedFromInternet = (RssRoot)serializer.Deserialize(reader);

            var posts = feedFromInternet.channel.items
                .Where(a => dbFeed.Posts.All(x => x.Url != a.link))
                .Select(scrappedPost => new Post()
                {
                    Title = scrappedPost.title,
                    Url = scrappedPost.link,
                    Description = scrappedPost.description,
                    PublishedAt = DateTime.Parse(scrappedPost.pubDate).ToUniversalTime(),
                    FeedId = dbFeed.Id
                }).ToList();

            dbFeed.Posts.AddRange(posts);
            dbFeed.LastFetchedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
        }
    }
    [XmlRoot("rss")]
    public class RssRoot
    {
        public RssFeed channel { get; set; }
    }
    public class RssFeed
    {
        public string title { get; set; }
        public string link { get; set; }
        public string description { get; set; }
        public string langauge { get; set; }

        [XmlElement("item")] public List<RssItem> items { get; set; }
    }

    public class RssItem
    {
        public string title { get; set; }
        public string link { get; set; }
        public string description { get; set; }
        public string pubDate { get; set; }
    }
}
