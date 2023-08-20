using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Pathfinding;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAPI
{
    [JsonObject]
    public class Flight : IArc<City>, IValidatableObject
    {
        public int ID { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public City DepartureCity { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public City ArrivalCity { get; set; }
        [JsonIgnore]
        protected DateTime departureDatetime;
        public DateTime DepartureDatetime
        {
            get { return departureDatetime; }
            set { departureDatetime = value.Kind == DateTimeKind.Utc ? value : value.ToUniversalTime(); }
        }
        [JsonIgnore]
        protected DateTime arrivalDatetime;
        public DateTime ArrivalDatetime
        {
            get { return arrivalDatetime; }
            set { arrivalDatetime = value.Kind == DateTimeKind.Utc ? value : value.ToUniversalTime(); }
        }
        [JsonConverter(typeof(StringEnumConverter))]
        public Airline Airline { get; set; }
        public double Price { get; set; }

        [NotMapped]
        [JsonIgnore]
        public double Length => Price;
        [NotMapped]
        [JsonIgnore]
        public City From => DepartureCity;
        [NotMapped]
        [JsonIgnore]
        public City To => ArrivalCity;

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
        public Flight()
        {
            this.ID = 0;
            this.DepartureCity = City.Undefined;
            this.ArrivalCity = City.Undefined;
            this.DepartureDatetime = DateTime.MinValue;
            this.ArrivalDatetime = DateTime.MaxValue;
            this.Airline = Airline.Undefined;
            this.Price = 0;
        }

        public override string ToString()
        {
            return string.Format($"{ID}, {DepartureCity} - {ArrivalCity}, {DepartureDatetime} - {ArrivalDatetime}, {Airline}, ${Price}");
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> errors = new List<ValidationResult>();

            if (DepartureCity == ArrivalCity) errors.Add(new ValidationResult("DepartureCity and ArrivalCity can`t be equal"));
            if (ArrivalDatetime < DepartureDatetime) errors.Add(new ValidationResult("DepartureDatetime can`t be later than ArrivalDatetime"));
            if (ArrivalDatetime == DepartureDatetime) errors.Add(new ValidationResult("DepartureDatetime and ArrivalDatetime can`t be equal"));
            if (Price < 0) errors.Add(new ValidationResult("Price can`t be less than zero"));

            return errors;
        }
    }
}