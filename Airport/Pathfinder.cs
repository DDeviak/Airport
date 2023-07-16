namespace Airport
{
    class Pathfinder
    {
        GraphCollection _graph;

        const int _maxTimeBetweenFlights = 7;
        const int _minTimeBetweenFlights = 1;

        public Pathfinder(ref GraphCollection graph)
        {
            _graph = graph;
        }

        public IEnumerable<Flight> GetPath(City from, City to, DateTime date)
        {
            Dictionary<City, ValueTuple<double, Flight, bool>> flags = new Dictionary<City, ValueTuple<double, Flight, bool>>();

            foreach (City t in _graph.GetCities())
            {
                flags[t] = new ValueTuple<double, Flight, bool>(double.PositiveInfinity, null, false);
            }
            flags[from] = new ValueTuple<double, Flight, bool>(0, null, false);

            PriorityQueue<City,double> pq = new PriorityQueue<City, double> ();

            pq.Enqueue(from, 0);

            City currentCity;
            while (pq.Count > 0)
            {
                currentCity = pq.Dequeue();
                if (flags[currentCity].Item3) continue;
                (double, Flight, bool) cf = flags[currentCity];
                List<Flight> flights = _graph.GetFlightsByCity(currentCity).ToList();
                flights.ForEach((Flight t) =>
                {
                    if (cf.Item2 != null)
                    {
                        double deltaTime = (t.DepartureDatetime - cf.Item2.ArrivalDatetime).TotalHours;
                        if ((_minTimeBetweenFlights >= deltaTime) || (deltaTime >= _maxTimeBetweenFlights)) return;
                    }
                    if (t.DepartureDatetime.Date <= date) return;
                        if (t.Price + cf.Item1 < flags[t.ArrivalCity].Item1)
                    {
                        (double, Flight, bool) f = flags[t.ArrivalCity];
                        f.Item1 = t.Price + cf.Item1;
                        f.Item2 = t;
                        flags[t.ArrivalCity] = f;
                        pq.Enqueue(t.ArrivalCity, f.Item1);
                    }
                });
                cf.Item3 = true;
                flags[currentCity] = cf;
            }

            List<Flight> path = new List<Flight>();

            currentCity = to;
            while (currentCity!=from)
            {
                (double, Flight, bool) f = flags[currentCity];
                if(f.Item2 == null) { throw new Exception("Path not found"); }
                path.Add(f.Item2);
                currentCity = f.Item2.DepartureCity;
            }

            path.Reverse();

            return path;
        }

    }
}
