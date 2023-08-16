using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Airport
{
    [JsonConverter(typeof(GraphCollectionConverter))]
    [JsonObject(MemberSerialization.Fields)]
    public class GraphCollection
    {
        [JsonProperty()]
        Dictionary<City, Dictionary<int, Flight>> _flights = new Dictionary<City, Dictionary<int, Flight>>();

        public void Add(Flight item)
        {
            if (ContainsID(item.ID)) throw new ArgumentException($"Item with ID:{item.ID} already exist");
            if (!_flights.ContainsKey(item.DepartureCity)) _flights[item.DepartureCity] = new Dictionary<int, Flight>();
            _flights[item.DepartureCity][item.ID] = item;
        }

        public void Remove(int id)
        {
            foreach (KeyValuePair<City, Dictionary<int, Flight>> t in _flights)
            {
                if (t.Value.Remove(id))
                {
                    if (t.Value.Count == 0) _flights.Remove(t.Key);
                    return;
                }
            }
            throw new KeyNotFoundException($"Item with ID:{id} doesn`t exist");
        }

        public Flight Get(int id)
        {
            Flight? result;
            foreach (Dictionary<int, Flight> t in _flights.Values)
            {
                if (t.TryGetValue(id, out result)) return result;
            }
            throw new KeyNotFoundException($"Item with ID:{id} doesn`t exist");
        }

        public void Replace(int id, Flight value)
        {
            Remove(id);
            Add(value);
        }

        public void Modify(int id, string propertyName, object propertyValue)
        {
            foreach (Dictionary<int, Flight> t in _flights.Values)
            {
                if (t.ContainsKey(id))
                {
                    t[id].SetProperty(propertyName, propertyValue);
                    if (propertyName == "DepartureCity")
                    {
                        Add(t[id]);
                        t.Remove(id);
                    }
                    break;
                }
            }
            throw new KeyNotFoundException($"Item with ID:{id} doesn`t exist");
        }

        public IEnumerable<City> GetCities()
        {
            return _flights.Keys;
        }

        public IEnumerable<Flight> GetFlightsByCity(City city)
        {
            if (_flights.ContainsKey(city))
                return _flights[city].Values;
            return new List<Flight>();
        }

        public bool ContainsID(int id)
        {
            foreach (Dictionary<int, Flight> t in _flights.Values)
            {
                if (t.ContainsKey(id)) return true;
            }
            return false;
        }

        public IEnumerable<Flight> AsEnumerable()
        {
            List<Flight> flights = new List<Flight>();
            foreach (Dictionary<int, Flight> t in _flights.Values)
            {
                flights.AddRange(t.Values);
            }
            return flights;
        }
    }
}