using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Airport
{
    [JsonConverter(typeof(GraphCollectionConverter))]
    [JsonObject(MemberSerialization.Fields)]
    class GraphCollection
    {
        [JsonProperty()]
        Dictionary<City,Dictionary<int,Flight>> _flights = new Dictionary<City, Dictionary<int, Flight>>();

        public void Add(Flight item)
        {
            if(ContainsID(item.ID)) throw new ArgumentException($"Item with ID:{item.ID} already exist");
            if(!_flights.ContainsKey(item.DepartureCity)) _flights[item.DepartureCity] = new Dictionary<int,Flight>();
            _flights[item.DepartureCity][item.ID]=item;
        }

        public void Remove(int id)
        {
            foreach (Dictionary<int, Flight> t in _flights.Values)
            {
                if (t.Remove(id)) break;
            }
            throw new ArgumentException($"Item with ID:{id} doesn`t exist");
        }

        public void Modify(int id, string propertyName, object propertyValue)
        {
            foreach (Dictionary<int, Flight> t in _flights.Values)
            {
                if (t.ContainsKey(id))
                {
                    t[id].SetProperty(propertyName, propertyValue);
                    if(propertyName == "DepartureCity")
                    {
                        Add(t[id]);
                        t.Remove(id);
                    }
                    break;
                }
            }
            throw new ArgumentException($"Item with ID:{id} doesn`t exist");
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
    }
}