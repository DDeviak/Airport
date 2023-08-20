using Microsoft.EntityFrameworkCore;
using Pathfinding;

namespace WebAPI
{
    public class FlightsDbContext : DbContext, IGraphProvider<City, Flight>
    {
        public DbSet<Flight> Flights { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=flightsdb;Username=postgres;Password=root");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Flight>().Property(e => e.DepartureCity).HasConversion<string>();
            modelBuilder.Entity<Flight>().Property(e => e.ArrivalCity).HasConversion<string>();
            modelBuilder.Entity<Flight>().Property(e => e.Airline).HasConversion<string>();

            base.OnModelCreating(modelBuilder);
        }

        public IEnumerable<City> GetNodes()
        {
            HashSet<City> nodes = new HashSet<City>();
            foreach (var t in Flights)
            {
                nodes.Add(t.From);
            }
            return nodes;
        }

        public IEnumerable<Flight> GetOutcomingArcs(City node)
        {
            List<Flight> outcomingArcs = new List<Flight>();
            foreach (var t in Flights)
            {
                if (t.From == node) outcomingArcs.Add(t);
            }
            return outcomingArcs;
        }
    }
}
