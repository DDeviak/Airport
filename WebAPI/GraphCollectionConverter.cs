using Newtonsoft.Json;

namespace WebAPI
{
    public class GraphCollectionConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
        {
            GraphCollection gc = ((GraphCollection?)value ?? new GraphCollection());

            writer.WriteStartArray();
            foreach (City city in gc.GetNodes())
            {
                foreach (Flight flights in gc.GetOutcomingArcs(city))
                {
                    serializer.Serialize(writer, flights);
                }
            }
            writer.WriteEndArray();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
        {
            GraphCollection gc = new GraphCollection();

            foreach (Flight t in serializer.Deserialize<Flight[]>(reader) ?? new Flight[0])
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
