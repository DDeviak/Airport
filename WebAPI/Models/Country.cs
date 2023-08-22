// <copyright file="Country.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace WebAPI
{
	using System.Reflection;
	using Newtonsoft.Json;

	[JsonObject]
	public class Country
	{
		private static Dictionary<string, Country> countries = null!;

		public Country(string name, HashSet<City> cities)
		{
			Name = name;
			Cities = cities;
		}

		public static Dictionary<string, Country> Countries
		{
			get
			{
				if (countries == null)
				{
					using (Stream? stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("WebAPI.CountriesConfig.json"))
					using (StreamReader sr = new StreamReader(stream ?? Stream.Null))
					{
						JsonSerializer jsonSerializer = new JsonSerializer
						{
							Formatting = Formatting.Indented,
						};
						countries = (Dictionary<string, Country>?)jsonSerializer.Deserialize(sr, typeof(Dictionary<string, Country>)) ?? new Dictionary<string, Country>();
					}
				}

				return countries ?? new ();
			}
		}

		[JsonProperty]
		public string Name { get; protected set; }

		[JsonProperty]
		public HashSet<City> Cities { get; protected set; }

		public static IEnumerable<Country> GetAll()
		{
			return Countries.Values.ToList();
		}

		public static Country Get(string country)
		{
			return Countries[country];
		}

		public static IEnumerable<City> GetCitiesByCountry(string country)
		{
			if (!IsExist(country))
			{
				throw new ArgumentException($"Country named \"{country}\" doesn`t exist");
			}

			return Countries[country].Cities.AsEnumerable();
		}

		public static bool IsExist(string country)
		{
			return Countries.ContainsKey(country);
		}
	}
}
