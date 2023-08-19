﻿using Newtonsoft.Json;
using System.Reflection;

namespace WebAPI
{
    [JsonObject]
    public class Country
    {
        static internal Dictionary<string, Country> countries = null!;
        static public Dictionary<string, Country> Countries
        {
            get
            {
                if (countries == null) Initialize();
                return countries ?? new();
            }
        }
        static public void Initialize()
        {
            using (Stream? stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("WebAPI.CountriesConfig.json"))
            using (StreamReader sr = new StreamReader(stream))
            {
                JsonSerializer jsonSerializer = new JsonSerializer();
                jsonSerializer.Formatting = Formatting.Indented;
                countries = ((Dictionary<string, Country>?)jsonSerializer.Deserialize(sr, typeof(Dictionary<string, Country>)) ?? new Dictionary<string, Country>());
            }
        }
        static public IEnumerable<Country> GetAll()
        {
            return Countries.Values.ToList();
        }
        static public Country Get(string country)
        {
            return Countries[country];
        }
        static public IEnumerable<City> GetCitiesByCountry(string country)
        {
            if (!IsExist(country)) throw new ArgumentException($"Country named \"{country}\" doesn`t exist");
            return Countries[country].Cities.AsEnumerable();
        }
        static public bool IsExist(string country)
        {
            return Countries.ContainsKey(country);
        }

        public Country(string name, HashSet<City> cities)
        {
            Name = name;
            Cities = cities;
        }
        [JsonProperty]
        public string Name { get; protected set; }
        [JsonProperty]
        public HashSet<City> Cities { get; protected set; }
    }
}