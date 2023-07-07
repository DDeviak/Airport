using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Airport
{
    class Flight
    {
        public City DepartureCity { get; private set; }
        public City ArrivalCity { get; private set; }
        public DateTime DepartureDatetime { get; private set; }
        public DateTime ArrivalDatetime { get; private set; }
        public Airline Airline { get; private set; }
        public double Price { get; private set; }

        public Flight(City departureCity, City arrivalCity, DateTime departureDatetime, DateTime arrivalDatetime, Airline airline, double price)
        {
            DepartureCity = departureCity;
            ArrivalCity = arrivalCity;
            DepartureDatetime = departureDatetime;
            ArrivalDatetime = arrivalDatetime;
            Airline = airline;
            Price = price;
        }

        public override string ToString()
        {
            return String.Format("{0} - {1}, {2} - {3}, {4}, ${5}", DepartureCity, ArrivalCity, DepartureDatetime, ArrivalDatetime, Airline, Price);
        }
    }
}