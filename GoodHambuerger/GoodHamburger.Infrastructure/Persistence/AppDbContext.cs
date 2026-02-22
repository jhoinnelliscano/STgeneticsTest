using GoodHamburger.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace GoodHamburger.Infrastructure.Persistence
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<SandwichEntity> Sandwiches { get; set; }
        public DbSet<ExtraEntity> Extras { get; set; }
        public DbSet<OrderEntity> Orders { get; set; }
        public DbSet<OrderDetailEntity> OrderDetails { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<SandwichEntity>().HasKey(s => s.Id);
            modelBuilder.Entity<ExtraEntity>().HasKey(e => e.Id);
            modelBuilder.Entity<OrderEntity>().HasKey(o => o.Id);
            modelBuilder.Entity<OrderDetailEntity>().HasKey(od => od.Id);

            modelBuilder.Entity<OrderEntity>()
                .HasOne(o => o.Sandwich)
                .WithMany()
                .HasForeignKey(o => o.IdSandwich);

            modelBuilder.Entity<OrderDetailEntity>()
                .HasOne(od => od.Order)
                .WithMany(o => o.Details)
                .HasForeignKey(od => od.IdOrder);

            modelBuilder.Entity<OrderDetailEntity>()
                .HasOne(od => od.Extra)
                .WithMany()
                .HasForeignKey(od => od.IdExtra);

            // Seed Data
            modelBuilder.Entity<SandwichEntity>().HasData(
                new SandwichEntity { Id = 1, Name = "Burger", Price = 5.00m },
                new SandwichEntity { Id = 2, Name = "Egg", Price = 4.50m },
                new SandwichEntity { Id = 3, Name = "Bacon", Price = 7.00m }
            );

            modelBuilder.Entity<ExtraEntity>().HasData(
                new ExtraEntity { Id = 1, Name = "Fries", Price = 2.00m },
                new ExtraEntity { Id = 2, Name = "Soft drink", Price = 2.50m }
            );
        }
    }
}
