// <copyright file="Flight.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace WebAPI
{
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using Newtonsoft.Json;
	using Newtonsoft.Json.Converters;
	using Pathfinding;

	[JsonObject]
	public class Flight : IArc<City>, IValidatableObject
	{
		[JsonIgnore]
		private DateTime departureDatetime;

		[JsonIgnore]
		private DateTime arrivalDatetime;

		public int ID { get; set; }

		[JsonConverter(typeof(StringEnumConverter))]
		public City DepartureCity { get; set; }

		[JsonConverter(typeof(StringEnumConverter))]
		public City ArrivalCity { get; set; }

		public DateTime DepartureDatetime
		{
			get { return departureDatetime; }
			set { departureDatetime = value.ToUniversalTime(); }
		}

		public DateTime ArrivalDatetime
		{
			get { return arrivalDatetime; }
			set { arrivalDatetime = value.ToUniversalTime(); }
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

		public override string ToString()
		{
			return string.Format($"{ID}, {DepartureCity} - {ArrivalCity}, {DepartureDatetime} - {ArrivalDatetime}, {Airline}, ${Price}");
		}

		public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
		{
			List<ValidationResult> errors = new List<ValidationResult>();

			if (DepartureCity == ArrivalCity)
			{
				errors.Add(new ValidationResult("DepartureCity and ArrivalCity can`t be equal"));
			}

			if (ArrivalDatetime < DepartureDatetime)
			{
				errors.Add(new ValidationResult("DepartureDatetime can`t be later than ArrivalDatetime"));
			}

			if (ArrivalDatetime == DepartureDatetime)
			{
				errors.Add(new ValidationResult("DepartureDatetime and ArrivalDatetime can`t be equal"));
			}

			if (Price < 0)
			{
				errors.Add(new ValidationResult("Price can`t be less than zero"));
			}

			return errors;
		}
	}
}