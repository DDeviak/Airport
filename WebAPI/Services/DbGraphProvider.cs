using Pathfinding;

namespace WebAPI
{
    public class DbGraphProvider : IGraphProvider<City, Flight>
    {
        protected FlightsDbContext db;

        public DbGraphProvider(FlightsDbContext db)
        {
            this.db = db;
        }

        public IEnumerable<City> GetNodes()
        {
            HashSet<City> nodes = new HashSet<City>();
            foreach (Flight t in db.Flights)
            {
                nodes.Add(t.From);
                nodes.Add(t.To);
            }
            return nodes;
        }

        public IEnumerable<Flight> GetOutcomingArcs(City node)
        {
            List<Flight> outcomingArcs = new List<Flight>();
            foreach (Flight t in db.Flights)
            {
                if (t.From == node)
                {
                    outcomingArcs.Add(t);
                }
            }
            return outcomingArcs;
        }
    }
}
