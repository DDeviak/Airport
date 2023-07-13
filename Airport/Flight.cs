﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Airport
{
    class Flight
    {
        public int ID { get; private set; }
        private City departureCity;
        public City DepartureCity {
            get { return departureCity; }
            private set 
            { 
                if (value == arrivalCity) throw new ArgumentException("DepartureCity and ArrivalCity can`t be equal");
                departureCity = value;
            }
        }
        private City arrivalCity;
        public City ArrivalCity
        {
            get { return arrivalCity; }
            private set
            {
                if (departureCity == value) throw new ArgumentException("DepartureCity and ArrivalCity can`t be equal");
                arrivalCity = value;
            }
        }
        private DateTime departureDatetime;
        public DateTime DepartureDatetime
        {
            get { return departureDatetime; }
            private set 
            {
                if (arrivalDatetime < value) throw new ArgumentException("ArrivalDatetime can`t be sooner than DepartureDatetime");
                departureDatetime = value; 
            }
        }
        private DateTime arrivalDatetime;
        public DateTime ArrivalDatetime
        {
            get { return departureDatetime; }
            private set
            {
                if (value < departureDatetime) throw new ArgumentException("ArrivalDatetime can`t be sooner than DepartureDatetime");
                arrivalDatetime = value;
            }
        }
        public Airline Airline { get; private set; }
        private double price;
        public double Price
        {
            get { return price; }
            set 
            {
                if (value < 0) throw new ArgumentException("Price can`t be less than zero");
                price = value; 
            }
        }

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
            return String.Format("{6}, {0} - {1}, {2} - {3}, {4}, ${5}", DepartureCity, ArrivalCity, DepartureDatetime, ArrivalDatetime, Airline, Price, ID);
        }
    }
}