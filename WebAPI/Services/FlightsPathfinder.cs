﻿// <copyright file="FlightsPathfinder.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace WebAPI
{
	using Pathfinding;

	public class FlightsPathfinder : PathfinderBase<City, Flight>
	{
		protected const int MaxTimeBetweenFlights = 7;
		protected const int MinTimeBetweenFlights = 1;

		public FlightsPathfinder(IGraphProvider<City, Flight> graph)
			: base(graph)
		{
		}

		public IEnumerable<Flight>? GetPath(City departureCity, City arrivalCity, DateTime date)
		{
			ValidateArguments(departureCity, arrivalCity);
			return MakePath(Dijkstra(departureCity, date), departureCity, arrivalCity);
		}

		public IDictionary<DateTime, IEnumerable<Flight>?> GetFlightsByMonth(City departureCity, City arrivalCity, int year, int month)
		{
			ValidateArguments(departureCity, arrivalCity);

			Dictionary<DateTime, IEnumerable<Flight>?> flightsByDays = new Dictionary<DateTime, IEnumerable<Flight>?>();

			for (int i = 1; i <= DateTime.DaysInMonth(year, month); i++)
			{
				DateTime date = new DateTime(year, month, i, 0, 0, 0, DateTimeKind.Utc);
				flightsByDays[date] = GetPath(departureCity, arrivalCity, date);
			}

			return flightsByDays;
		}

		public IDictionary<City, IEnumerable<Flight>> GetFlightsToCountry(City departureCity, string arrivalCountry, DateTime date)
		{
			if (!Country.IsExist(arrivalCountry))
			{
				throw new ArgumentException($"Country named \"{arrivalCountry}\" doesn`t exist");
			}

			Dictionary<City, Flags> flags = Dijkstra(departureCity, date);

			Dictionary<City, IEnumerable<Flight>> flightsByCity = new Dictionary<City, IEnumerable<Flight>>();

			IEnumerable<Flight>? path;
			foreach (City arrivalCity in Country.GetCitiesByCountry(arrivalCountry))
			{
				path = MakePath(flags, departureCity, arrivalCity);
				if (path != null && path.Count() != 0)
				{
					flightsByCity.Add(arrivalCity, path);
				}
			}

			return flightsByCity;
		}

		protected void ValidateArguments(City departureCity, City arrivalCity)
		{
			if (departureCity == arrivalCity)
			{
				throw new ArgumentException("DepartureCity and ArrivalCity can`t be equal");
			}
		}

		protected Dictionary<City, Flags> Dijkstra(City from, DateTime date)
		{
			return Dijkstra(from, (current, t) =>
			{
				if (t.DepartureCity == from)
				{
					double deltaTime = (t.DepartureDatetime - date).TotalHours;
					if ((deltaTime < 0) || (deltaTime > 24))
					{
						return false;
					}
				}

				if (current != null)
				{
					double deltaTime = (t.DepartureDatetime - current.ArrivalDatetime).TotalHours;
					if ((deltaTime < MinTimeBetweenFlights) || (deltaTime > MaxTimeBetweenFlights))
					{
						return false;
					}
				}

				return true;
			});
		}
	}
}
