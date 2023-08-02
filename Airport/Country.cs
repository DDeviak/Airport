using Newtonsoft.Json;
using System.Reflection;

namespace Airport
{
    [JsonObject]
    class Country
    {
        static public Dictionary<string, Country> Countries { get; protected set; }
        static public void Initialize()
        {
            if (Countries == null)
                using (StreamReader sr = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("Airport.CountriesConfig.json")))
                {
                    JsonSerializer jsonSerializer = new JsonSerializer();
                    jsonSerializer.Formatting = Formatting.Indented;
                    Countries = (Dictionary<string, Country>)jsonSerializer.Deserialize(sr, typeof(Dictionary<string, Country>));
                }
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
