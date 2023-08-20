using Microsoft.EntityFrameworkCore;

namespace WebAPI
{
    public class FlightsDbContext : DbContext
    {
        public DbSet<Flight> Flights { get; set; } = null!;

        public FlightsDbContext(DbContextOptions<FlightsDbContext> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Flight>().Property(e => e.DepartureCity).HasConversion<string>();
            modelBuilder.Entity<Flight>().Property(e => e.ArrivalCity).HasConversion<string>();
            modelBuilder.Entity<Flight>().Property(e => e.Airline).HasConversion<string>();

            base.OnModelCreating(modelBuilder);
        }
    }
}
