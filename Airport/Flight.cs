using System.Runtime.Serialization;

namespace Airport
{
    [DataContract()]
    class Flight
    {
        [DataMember()]
        public int ID { get; private set; }
        [DataMember()]
        private City departureCity;
        public City DepartureCity {
            get { return departureCity; }
            private set 
            { 
                if (value == arrivalCity) throw new ArgumentException("DepartureCity and ArrivalCity can`t be equal");
                departureCity = value;
            }
        }
        [DataMember()]
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
        [DataMember()]
        public DateTime DepartureDatetime { get; private set; }
        [DataMember()]
        public DateTime ArrivalDatetime { get; private set; }
        [DataMember()]
        public Airline Airline { get; private set; }
        [DataMember()]
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

        public Flight(int id, City departureCity, City arrivalCity, DateTime departureDatetime, DateTime arrivalDatetime, Airline airline, double price)
        {
            if (arrivalDatetime < departureDatetime) throw new ArgumentException("ArrivalDatetime can`t be sooner than DepartureDatetime");
            ID = id;
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