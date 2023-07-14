using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Airport
{
    class GraphColection
    {
        Dictionary<City,Dictionary<int,Flight>> _flights = new Dictionary<City, Dictionary<int, Flight>>();

        public void Add(Flight item)
        {
            if(!_flights.ContainsKey(item.DepartureCity)) _flights[item.DepartureCity] = new Dictionary<int,Flight>();
            _flights[item.DepartureCity][item.ID]=item;
        }

        public void Remove(int id)
        {
            foreach (var t in _flights.Values)
            {
                if (t.Remove(id)) break;
            }
        }

        public void Modify(int id, Flight itemNew)
        {
            Remove(id);
            Add(itemNew);
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