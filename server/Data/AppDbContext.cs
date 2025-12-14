using Microsoft.EntityFrameworkCore;
using server.Models;

namespace server.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Chat> Chats { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<AuthToken> AuthTokens { get; set; }


        public AppDbContext(DbContextOptions<AppDbContext> options) 
            : base(options) { }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Message>()
                .HasOne(m => m.Author)
                .WithMany(u => u.Messages)
                .HasForeignKey(m => m.AuthorId);

            modelBuilder.Entity<Message>()
                .HasOne(m => m.Chat)
                .WithMany(c => c.Messages)
                .HasForeignKey(m => m.ChatId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<User>()
                .HasMany(u => u.Chats)
                .WithMany(c => c.Users)
                .UsingEntity(j => j.ToTable("UserChats"));

            modelBuilder.Entity<AuthToken>()
                .HasOne(at => at.User)
                .WithMany(u => u.AuthTokens)
                .HasForeignKey(at => at.UserId);

            base.OnModelCreating(modelBuilder);
        }
    }
}
