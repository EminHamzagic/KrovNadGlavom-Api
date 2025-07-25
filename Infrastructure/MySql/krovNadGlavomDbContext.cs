using Microsoft.EntityFrameworkCore;
using krov_nad_glavom_api.Domain.Entities;

namespace krov_nad_glavom_api.Infrastructure.MySql
{
    public class krovNadGlavomDbContext : DbContext
    {
        public krovNadGlavomDbContext(DbContextOptions<krovNadGlavomDbContext> options) : base(options) { }
        public DbSet<User> Users { get; set; }
        public DbSet<UserSession> UserSessions { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.UseCollation("utf8mb4_bin");

             builder.Entity<UserSession>()
                .HasOne<User>()
                .WithMany()
                .HasForeignKey(us => us.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}