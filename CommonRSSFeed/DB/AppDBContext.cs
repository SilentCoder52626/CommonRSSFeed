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
        public DbSet<Subscription> Subscriptions { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AppUser>()
                .HasMany(user => user.Feeds)
                .WithMany(feed => feed.Users)
                .UsingEntity<Subscription>();

            modelBuilder.Entity<Subscription>()
                .HasIndex(x=>new {x.FeedId,x.UserId}).IsUnique();

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
        public List<Subscription> Subscriptions { get; set; } = new List<Subscription>();
        public List<Feed> Feeds { get; set; } = new List<Feed>();
    }
    public class Feed
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public required string Url { get; set; }
        public DateTime? LastFetchedAt { get; set; }
        public List<Post> Posts { get; set; } = new List<Post>();
        public List<Subscription> Subscriptions { get; set; } = new List<Subscription>();
        public List<AppUser> Users { get; set; } = new List<AppUser>();

    }
    public class Post
    {
        public Guid Id { get; set; }
        public required string Title { get; set; }
        public required string Description { get; set; }
        public required string Url { get; set; }
        public required DateTime PublishedAt { get; set; }

        public required Guid FeedId { get; set; }
         public Feed Feed { get; set; }

    }

    public class Subscription
    {
        public Guid Id { get; set; }
        public required DateTime SubscribedAt { get; set; } = DateTime.UtcNow;
        public Guid UserId { get; set; }
        public AppUser User { get; set; }
        public Guid FeedId { get; set; }
        public Feed Feed { get; set; }

    }
}
