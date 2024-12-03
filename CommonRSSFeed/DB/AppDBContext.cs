using Microsoft.EntityFrameworkCore;

namespace CommonRSSFeed.DB
{
    public class AppDBContext : DbContext
    {
        public AppDBContext(DbContextOptions<AppDBContext> options) : base(options) 
        {
            
        }
        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<Feed> Feeds { get; set; }
        public DbSet<Post> Posts { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }
    }

    public class AppUser
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
        public required string Role { get; set; }
    }
    public class Feed
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public required string Url { get; set; }
        public DateTime? LastFetchedAt { get; set; }
        public List<Post> Posts { get; set; } = new List<Post>();
    }
    public class Post
    {
        public Guid Id { get; set; }
        public required string Title { get; set; }
        public required string Description { get; set; }
        public required string Url { get; set; }
        public required DateTime PublishedAt { get; set; }

        public required Guid FeedId { get; set; }
         public required Feed Feed { get; set; }

    }
}
