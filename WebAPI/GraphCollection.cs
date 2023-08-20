using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.ComponentModel;
using System.Reflection;

namespace WebAPI
{
    [JsonConverter(typeof(GraphCollectionConverter))]
    [JsonObject(MemberSerialization.Fields)]
    public class GraphCollection : Pathfinding.IGraphProvider<City, Flight>
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
                    try
                    {
                        PropertyInfo? property = typeof(Flight).GetProperty(propertyName);
                        property?.SetValue(t[id], TypeDescriptor.GetConverter(property.PropertyType).ConvertFrom(propertyValue), null);
                    }
                    catch (TargetInvocationException ex)
                    {
                        throw ex.InnerException ?? ex;
                    }
                    catch (NullReferenceException)
                    {
                        throw new KeyNotFoundException("Property with such name doesn`t exist");
                    }
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

        public IEnumerable<City> GetNodes()
        {
            return _flights.Keys;
        }

        public IEnumerable<Flight> GetOutcomingArcs(City city)
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