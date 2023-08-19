using Pathfinding;

namespace Airport
{
    public class Pathfinder : PathfinderBase<City, Flight>
    {
        const int _maxTimeBetweenFlights = 7;
        const int _minTimeBetweenFlights = 1;

        public Pathfinder(GraphCollection graph) : base(graph) { }

        protected Dictionary<City, Flags> Dijkstra(City from, DateTime date)
        {
            return Dijkstra(from, (current, t) =>
            {
                if (t.DepartureCity == from)
                {
                    double deltaTime = (t.DepartureDatetime - date).TotalHours;
                    if ((deltaTime < 0) || (deltaTime > 24)) return false;
                }
                if (current != null)
                {
                    double deltaTime = (t.DepartureDatetime - current.ArrivalDatetime).TotalHours;
                    if ((_minTimeBetweenFlights > deltaTime) || (deltaTime > _maxTimeBetweenFlights)) return false;
                }
                return true;
            });
        }
        public IEnumerable<Flight>? GetPath(City departureCity, City arrivalCity, DateTime date)
        {
            if (departureCity == arrivalCity) throw new ArgumentException("DepartureCity and ArrivalCity can`t be equal");
            return MakePath(Dijkstra(departureCity, date), departureCity, arrivalCity);
        }

        public IDictionary<DateTime, IEnumerable<Flight>?> GetFlightsByMonth(City departureCity, City arrivalCity, int year, int month)
        {
            if (departureCity == arrivalCity) throw new ArgumentException("DepartureCity and ArrivalCity can`t be equal");

            Dictionary<DateTime, IEnumerable<Flight>?> flightsByDays = new Dictionary<DateTime, IEnumerable<Flight>?>();

            for (int i = 1; i <= DateTime.DaysInMonth(year, month); i++)
            {
                DateTime date = new DateTime(year, month, i);
                flightsByDays[date] = GetPath(departureCity, arrivalCity, date);
            }

            return flightsByDays;
        }

        public IDictionary<City, IEnumerable<Flight>?> GetFlightsToCountry(City departureCity, string arrivalCountry, DateTime date)
        {
            if (!Country.IsExist(arrivalCountry)) throw new ArgumentException($"Country named \"{arrivalCountry}\" doesn`t exist");

            Dictionary<City, Flags> flags = Dijkstra(departureCity, date);

            Dictionary<City, IEnumerable<Flight>?> flightsByCity = new Dictionary<City, IEnumerable<Flight>?>();

            IEnumerable<Flight>? path;
            foreach (City arrivalCity in Country.GetCitiesByCountry(arrivalCountry))
            {
                try
                {
                    path = MakePath(flags, departureCity, arrivalCity);
                    if (path != null)
                        flightsByCity.Add(arrivalCity, path);
                }
                catch { }
            }
            return flightsByCity;
        }
    }
}
