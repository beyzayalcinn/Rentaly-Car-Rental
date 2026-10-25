using Microsoft.EntityFrameworkCore;
using Rentaly.EntityLayer.Entities;

namespace Rentaly.DataAccessLayer.Context
{
    public class RentalyContext : DbContext
    {
        public RentalyContext(DbContextOptions<RentalyContext> options)
            : base(options)
        {
        }

        public DbSet<Branch> Branches { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Car> Cars { get; set; }
        public DbSet<CarModel> CarModels { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Rental> Rentals { get; set; }

        // Ana sayfa içerikleri
        public DbSet<About> Abouts { get; set; }
        public DbSet<Process> Processes { get; set; }
        public DbSet<Feature> Features { get; set; }
        public DbSet<WhyChooseUs> WhyChooseUses { get; set; }
        public DbSet<Testimonial> Testimonials { get; set; }
        public DbSet<Faq> Faqs { get; set; }
        public DbSet<OurFuture> OurFutures { get; set; }
        public DbSet<Award> Awards { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Car>()
                .Property(x => x.DailyPrice)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Car>()
                .Property(x => x.DepositAmount)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Rental>()
                .Property(x => x.TotalPrice)
                .HasPrecision(18, 2);
        }
    }
}