using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.ComponentModel;
using System.Reflection;

namespace Airport
{
    [JsonObject(MemberSerialization.Fields)]
    class Flight
    {
        readonly public int ID;
        [JsonConverter(typeof(StringEnumConverter))]
        protected City departureCity;
        public City DepartureCity
        {
            get { return departureCity; }
            set
            {
                if (!Enum.IsDefined(typeof(City), value)) throw new ArgumentException("DepartureCity can`t be equal " + value);
                if (value == arrivalCity) throw new ArgumentException("DepartureCity and ArrivalCity can`t be equal");
                departureCity = value;
            }
        }
        [JsonConverter(typeof(StringEnumConverter))]
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
        protected DateTime arrivalDatetime;
        public DateTime ArrivalDatetime
        {
            get { return arrivalDatetime; }
            set
            {
                if (value < departureDatetime) throw new ArgumentException("ArrivalDatetime can`t be sooner than DepartureDatetime");
                arrivalDatetime = value;
            }
        }
        [JsonConverter(typeof(StringEnumConverter))]
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
        [JsonConstructor()]
        public Flight(int id, City departureCity, City arrivalCity, DateTime departureDatetime, DateTime arrivalDatetime, Airline airline, double price)
        {
            this.ID = id;
            this.DepartureCity = departureCity;
            this.ArrivalCity = arrivalCity;
            this.ArrivalDatetime = arrivalDatetime;
            this.DepartureDatetime = departureDatetime;
            this.Airline = airline;
            this.Price = price;
        }
        public Flight(int id)
        {
            this.ID = id;
            this.departureCity = City.Undefined;
            this.arrivalCity = City.Undefined;
            this.departureDatetime = DateTime.MinValue;
            this.arrivalDatetime = DateTime.MaxValue;
            this.airline = Airline.Undefined;
            this.price = 0;
        }
        public void SetProperty(string propertyName, object value)
        {
            try
            {
                PropertyInfo property = GetType().GetProperty(propertyName);
                property.SetValue(this, TypeDescriptor.GetConverter(property.PropertyType).ConvertFrom(value), null);
            }
            catch (TargetInvocationException ex)
            {
                throw ex.InnerException;
            }
            catch (NullReferenceException ex)
            {
                throw new KeyNotFoundException("Property with such name doesn`t exist");
            }
        }
        public void SetProperty(PropertyInfo property, object value)
        {
            try
            {
                property.SetValue(this, TypeDescriptor.GetConverter(property.PropertyType).ConvertFrom(value), null);
            }
            catch (TargetInvocationException ex)
            {
                throw ex.InnerException;
            }
        }

        public override string ToString()
        {
            return String.Format("{0}, {1} - {2}, {3} - {4}, {5}, ${6}", ID, DepartureCity, ArrivalCity, DepartureDatetime, ArrivalDatetime, Airline, Price);
        }
    }
}