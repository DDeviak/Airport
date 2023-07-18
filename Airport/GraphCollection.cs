using System.Runtime.Serialization;

namespace Airport
{
    [DataContract()]
    class GraphCollection
    {
        [DataMember()]
        Dictionary<City,Dictionary<int,Flight>> _flights = new Dictionary<City, Dictionary<int, Flight>>();

        public void Add(Flight item)
        {
            if(!_flights.ContainsKey(item.DepartureCity)) _flights[item.DepartureCity] = new Dictionary<int,Flight>();
            _flights[item.DepartureCity][item.ID]=item;
        }

        public void Remove(int id)
        {
            foreach (Dictionary<int, Flight> t in _flights.Values)
            {
                if (t.Remove(id)) break;
            }
        }

        public void Modify(int id, string propertyName, object propertyValue)
        {
            foreach (Dictionary<int, Flight> t in _flights.Values)
            {
                if (t.ContainsKey(id))
                {
                    t[id][propertyName] = propertyValue;
                    break;
                }
            }
        }

        public IEnumerable<City> GetCities()
        {
            return _flights.Keys;
        }

        public IEnumerable<Flight> GetFlightsByCity(City city) 
        {
            return _flights[city].Values;
        }
    }
}