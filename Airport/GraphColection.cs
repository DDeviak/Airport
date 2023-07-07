using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Airport
{
    class GraphColection
    {
        Dictionary<City,List<Flight>> _flights = new Dictionary<City, List<Flight>>();

        public void Add(Flight item)
        {
            List<Flight> tempFlightsByCity;
            if(_flights.TryGetValue(item.DepartureCity,out tempFlightsByCity)) 
            {
                tempFlightsByCity.Add(item);
            }
            else
            {
                tempFlightsByCity = new List<Flight>();
                _flights.Add(item.DepartureCity, tempFlightsByCity);
            }
        }

        public void Remove(Flight item)
        {
            List<Flight> tempFlightsByCity;
            if (_flights.TryGetValue(item.DepartureCity, out tempFlightsByCity))
            {
                tempFlightsByCity.Remove(item);
            }
        }

        public void Modify(Flight itemOld,Flight itemNew)
        {
            List<Flight> tempFlightsByCity;
            if (_flights.TryGetValue(itemOld.DepartureCity, out tempFlightsByCity))
            {
                tempFlightsByCity.Remove(itemOld);
            }
            if (_flights.TryGetValue(itemNew.DepartureCity, out tempFlightsByCity))
            {
                tempFlightsByCity.Add(itemNew);
            }
            else
            {
                tempFlightsByCity = new List<Flight>();
                _flights.Add(itemNew.DepartureCity, tempFlightsByCity);
            }
        }

        public IEnumerable<City> GetCities()
        {
            return _flights.Keys;
        }

        public IEnumerable<Flight> GetFlightsByCity(City city) 
        {
            List<Flight> tempFlightsByCity;
            if (_flights.TryGetValue(city, out tempFlightsByCity))
            {
                return tempFlightsByCity;
            }
            return new List<Flight>();
        }
    }
}