using System.Runtime.Serialization;

namespace Airport
{
    [DataContract()]
    class Flight
    {
        [DataMember()]
        public int ID { get; protected set; }
        [DataMember()]
        protected City departureCity;
        public City DepartureCity {
            get { return departureCity; }
            set 
            {
                if (!Enum.IsDefined(typeof(City), value)) throw new ArgumentException("DepartureCity can`t be equal " + value);
                if (value == arrivalCity) throw new ArgumentException("DepartureCity and ArrivalCity can`t be equal");
                departureCity = value;
            }
        }
        [DataMember()]
        protected City arrivalCity;
        public City ArrivalCity
        {
            get { return arrivalCity; }
            set
            {
                if (!Enum.IsDefined(typeof(City), value)) throw new ArgumentException("ArrivalCity can`t be equal " + value);
                if (departureCity == value) throw new ArgumentException("DepartureCity and ArrivalCity can`t be equal");
                arrivalCity = value;
            }
        }
        [DataMember()]
        protected DateTime departureDatetime;
        public DateTime DepartureDatetime
        {
            get { return departureDatetime; }
            set
            {
                if (arrivalDatetime < value) throw new ArgumentException("DepartureDatetime can`t be later than ArrivalDatetime");
                departureDatetime = value;
            }
        }
        [DataMember()]
        protected DateTime arrivalDatetime;
        public DateTime ArrivalDatetime
        {
            get { return departureDatetime; }
            set
            {
                if (value < departureDatetime) throw new ArgumentException("ArrivalDatetime can`t be sooner than DepartureDatetime");
                arrivalDatetime = value;
            }
        }
        [DataMember()]
        protected Airline airline;
        public Airline Airline
        {
            get { return airline; }
            set
            {
                if (!Enum.IsDefined(typeof(Airline), value)) throw new ArgumentException("Airline can`t be equal " + value);
                airline = value;
            }
        }
        [DataMember()]
        protected double price;
        public double Price
        {
            get { return price; }
            set 
            {
                if (value < 0) throw new ArgumentException("Price can`t be less than zero");
                price = double.Round(value, 2);
            }
        }

        public Flight(int id, City departureCity, City arrivalCity, DateTime departureDatetime, DateTime arrivalDatetime, Airline airline, double price)
        {
            this.ID = id;
            this.DepartureCity = departureCity;
            this.ArrivalCity = arrivalCity;
            this.departureDatetime = departureDatetime;
            this.ArrivalDatetime = arrivalDatetime;
            this.Airline = airline;
            this.Price = price;
        }

        public override string ToString()
        {
            return String.Format("{0}, {1} - {2}, {3} - {4}, {5}, ${6}", ID, DepartureCity, ArrivalCity, DepartureDatetime, ArrivalDatetime, Airline, Price);
        }
    }
}