namespace Airport
{
    public class Pathfinder
    {
        public GraphCollection Graph;

        const int _maxTimeBetweenFlights = 7;
        const int _minTimeBetweenFlights = 1;

        public Pathfinder(GraphCollection graph)
        {
            Graph = graph;
        }

        protected Dictionary<City, ValueTuple<double, Flight?, bool>> Dijkstra(City from, DateTime date)
        {
            Dictionary<City, ValueTuple<double, Flight?, bool>> flags = new Dictionary<City, ValueTuple<double, Flight?, bool>>();

            foreach (City t in Graph.GetCities())
            {
                flags[t] = new ValueTuple<double, Flight?, bool>(double.PositiveInfinity, null, false);
            }
            flags[from] = new ValueTuple<double, Flight?, bool>(0, null, false);

            PriorityQueue<City, double> pq = new PriorityQueue<City, double>();

            pq.Enqueue(from, 0);

            City currentCity;
            while (pq.Count > 0)
            {
                currentCity = pq.Dequeue();
                if (flags[currentCity].Item3) continue;
                (double, Flight?, bool) cf = flags[currentCity];
                List<Flight> flights = Graph.GetFlightsByCity(currentCity).ToList();
                flights.ForEach((Flight t) =>
                {
                    if (currentCity == from)
                    {
                        double deltaTime = (t.DepartureDatetime - date).TotalHours;
                        if ((deltaTime < 0) || (deltaTime > 24)) return;
                    }
                    if (cf.Item2 != null)
                    {
                        double deltaTime = (t.DepartureDatetime - cf.Item2.ArrivalDatetime).TotalHours;
                        if ((_minTimeBetweenFlights > deltaTime) || (deltaTime > _maxTimeBetweenFlights)) return;
                    }
                    if (t.Price + cf.Item1 < flags[t.ArrivalCity].Item1)
                    {
                        (double, Flight?, bool) f = flags[t.ArrivalCity];
                        f.Item1 = t.Price + cf.Item1;
                        f.Item2 = t;
                        flags[t.ArrivalCity] = f;
                        pq.Enqueue(t.ArrivalCity, f.Item1);
                    }
                });
                cf.Item3 = true;
                flags[currentCity] = cf;
            }

            return flags;
        }

        protected IEnumerable<Flight>? MakePath(Dictionary<City, ValueTuple<double, Flight?, bool>> flags, City from, City to)
        {
            List<Flight> path = new List<Flight>();

            City currentCity = to;
            while (currentCity != from)
            {
                (double, Flight?, bool) f = flags[currentCity];
                if (f.Item2 == null) return null;
                path.Add(f.Item2);
                currentCity = f.Item2.DepartureCity;
            }

            path.Reverse();
            return path;
        }

        public IEnumerable<Flight>? GetPath(City from, City to, DateTime date)
        {
            return MakePath(Dijkstra(from, date), from, to);
        }

        public IEnumerable<KeyValuePair<DateTime, IEnumerable<Flight>?>> GetFlightsByMonth(City departureCity, City arrivalCity, int year, int month)
        {
            Dictionary<DateTime, IEnumerable<Flight>?> flightsByDays = new Dictionary<DateTime, IEnumerable<Flight>?>();

            for (int i = 1; i <= DateTime.DaysInMonth(year, month); i++)
            {
                DateTime date = new DateTime(year, month, i);
                flightsByDays[date] = GetPath(departureCity, arrivalCity, date);
            }

            return flightsByDays;
        }

        public IEnumerable<KeyValuePair<City, IEnumerable<Flight>?>> GetFlightsToCountry(City departureCity, string arrivalCountry, DateTime date)
        {
            Dictionary<City, ValueTuple<double, Flight?, bool>> flags = Dijkstra(departureCity, date);

            Dictionary<City, IEnumerable<Flight>?> flightsByCity = new Dictionary<City, IEnumerable<Flight>?>();

            foreach (City arrivalCity in Country.GetCitiesByCountry(arrivalCountry))
            {
                try
                {
                    flightsByCity.Add(arrivalCity, MakePath(flags, departureCity, arrivalCity));
                }
                catch { }
            }
            return flightsByCity;
        }
    }
}
