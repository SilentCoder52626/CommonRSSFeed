namespace CommonRSSFeed.Models
{
    public class FeedDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public List<PostDto> Posts { get; set; } = new List<PostDto>();

    }
}
