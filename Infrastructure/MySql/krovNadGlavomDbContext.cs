using Microsoft.EntityFrameworkCore;
using krov_nad_glavom_api.Domain.Entities;

namespace krov_nad_glavom_api.Infrastructure.MySql
{
    public class krovNadGlavomDbContext : DbContext
    {
        public krovNadGlavomDbContext(DbContextOptions<krovNadGlavomDbContext> options) : base(options) { }
        public DbSet<User> Users { get; set; }
        public DbSet<UserSession> UserSessions { get; set; }
        public DbSet<ConstructionCompany> ConstructionCompanies { get; set; }
        public DbSet<Building> Buildings { get; set; }
        public DbSet<PriceList> PriceLists { get; set; }
        public DbSet<Apartment> Apartments { get; set; }
        public DbSet<Garage> Garages { get; set; }
        public DbSet<Agency> Agencies { get; set; }
        public DbSet<AgencyRequest> AgencyRequests { get; set; }
        public DbSet<UserAgencyFollow> UserAgencyFollows { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<DiscountRequest> DiscountRequests { get; set; }
        public DbSet<Contract> Contracts { get; set; }
        public DbSet<Installment> Installments { get; set; }
        public DbSet<Notification> Notifications { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.UseCollation("utf8mb4_bin");

            builder.Entity<UserSession>()
               .HasOne<User>()
               .WithMany()
               .HasForeignKey(us => us.UserId)
               .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<User>()
                .HasOne<ConstructionCompany>()
                .WithMany()
                .HasForeignKey(u => u.ConstructionCompanyId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<User>()
                .HasOne<Agency>()
                .WithMany()
                .HasForeignKey(u => u.AgencyId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<ConstructionCompany>()
                .HasKey(c => c.Id);

            // Building
            builder.Entity<Building>()
                .HasKey(b => b.Id);
            builder.Entity<Building>()
                .HasOne<ConstructionCompany>()
                .WithMany()
                .HasForeignKey(b => b.CompanyId);

            // PriceList (1:1 with Building)
            builder.Entity<PriceList>()
                .HasKey(p => p.Id);
            builder.Entity<PriceList>()
                .HasOne<Building>()
                .WithOne()
                .HasForeignKey<PriceList>(p => p.BuildingId);

            // Apartment
            builder.Entity<Apartment>()
                .HasKey(a => a.Id);
            builder.Entity<Apartment>()
                .HasOne<Building>()
                .WithMany()
                .HasForeignKey(a => a.BuildingId);

            // Garage
            builder.Entity<Garage>()
                .HasKey(g => g.Id);
            builder.Entity<Garage>()
                .HasOne<Building>()
                .WithMany()
                .HasForeignKey(g => g.BuildingId);

            // Agency
            builder.Entity<Agency>()
                .HasKey(a => a.Id);

            // AgencyRequest
            builder.Entity<AgencyRequest>()
                .HasKey(ar => ar.Id);
            builder.Entity<AgencyRequest>()
                .HasOne<Agency>()
                .WithMany()
                .HasForeignKey(ar => ar.AgencyId);
            builder.Entity<AgencyRequest>()
                .HasOne<Building>()
                .WithMany()
                .HasForeignKey(ar => ar.BuildingId);

            // UserAgencyFollow
            builder.Entity<UserAgencyFollow>()
                .HasKey(uf => uf.Id);
            builder.Entity<UserAgencyFollow>()
                .HasOne<User>()
                .WithMany()
                .HasForeignKey(uf => uf.UserId);
            builder.Entity<UserAgencyFollow>()
                .HasOne<Agency>()
                .WithMany()
                .HasForeignKey(uf => uf.AgencyId);

            // Reservation
            builder.Entity<Reservation>()
                .HasKey(r => r.Id);
            builder.Entity<Reservation>()
                .HasOne<User>()
                .WithMany()
                .HasForeignKey(r => r.UserId);
            builder.Entity<Reservation>()
                .HasOne<Apartment>()
                .WithMany()
                .HasForeignKey(r => r.ApartmentId);

            // constraints: one reservation per user, one per apartment
            builder.Entity<Reservation>()
                .HasIndex(r => r.UserId).IsUnique();
            builder.Entity<Reservation>()
                .HasIndex(r => r.ApartmentId).IsUnique();

            // DiscountRequest
            builder.Entity<DiscountRequest>()
                .HasKey(dr => dr.Id);
            builder.Entity<DiscountRequest>()
                .HasOne<User>()
                .WithMany()
                .HasForeignKey(dr => dr.UserId);
            builder.Entity<DiscountRequest>()
                .HasOne<Agency>()
                .WithMany()
                .HasForeignKey(dr => dr.AgencyId);
            builder.Entity<DiscountRequest>()
                .HasOne<Apartment>()
                .WithMany()
                .HasForeignKey(dr => dr.ApartmentId);

            // Contract
            builder.Entity<Contract>()
                .HasKey(c => c.Id);
            builder.Entity<Contract>()
                .HasOne<User>()
                .WithMany()
                .HasForeignKey(c => c.UserId);
            builder.Entity<Contract>()
                .HasOne<Agency>()
                .WithMany()
                .HasForeignKey(c => c.AgencyId);
            builder.Entity<Contract>()
                .HasOne<Apartment>()
                .WithMany()
                .HasForeignKey(c => c.ApartmentId);

            // Installment
            builder.Entity<Installment>()
                .HasKey(i => i.Id);
            builder.Entity<Installment>()
                .HasOne<Contract>()
                .WithMany()
                .HasForeignKey(i => i.ContractId);
                
            // Notification
            builder.Entity<Notification>()
                .HasKey(a => a.Id);
            builder.Entity<Notification>()
                .HasOne<User>()
                .WithMany()
                .HasForeignKey(a => a.UserId);
        }
    }
}