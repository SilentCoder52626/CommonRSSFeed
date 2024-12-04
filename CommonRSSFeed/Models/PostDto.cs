namespace CommonRSSFeed.Models
{
    public class PostDto
    {
        public required string Title { get; set; }
        public required string Content { get; set; }
        public required string FeedName { get; set; }
        public required string Url { get; set; }
        public required DateTime PublishedAt { get; set; }
    }

}
