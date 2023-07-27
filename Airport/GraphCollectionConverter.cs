using Newtonsoft.Json;

namespace Airport
{
    public class GraphCollectionConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            GraphCollection gc = (GraphCollection)value;

            writer.WriteStartArray();
            foreach (City city in gc.GetCities())
            {
                foreach (Flight flights in gc.GetFlightsByCity(city))
                {
                    serializer.Serialize(writer, flights);
                }
            }
            writer.WriteEndArray();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            GraphCollection gc = new GraphCollection();

            foreach (Flight t in serializer.Deserialize<Flight[]>(reader))
            {
                gc.Add(t);
            }

            return gc;
        }

        public override bool CanRead
        {
            get { return true; }
        }

        public override bool CanConvert(Type objectType)
        {
            return typeof(GraphCollection) == objectType;
        }
    }
}
