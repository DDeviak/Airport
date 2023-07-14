using System.Runtime.Serialization.Json;

namespace Airport
{
	internal class Manager
	{
		static DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(typeof(GraphColection));
		static void Menu()
		{
			Console.WriteLine(
				"Choose the option:\r\n" +
				"1. read -> to read data from the file.\r\n" +
				"2. write -> to write data to the file.\r\n" +
				"3. add -> to write element into the graph.\r\n" +
				"4. delete -> to delete element from collection.\r\n" +
				"5. edit -> to edit element in collection.\r\n" +
				"6. search -> to perform Dijkstra algorithm.\r\n" +
				"7. by month -> to perform Dijkstra algorithm by month.\r\n" +
				"8. by country -> to perform Dijkstra algorithm by country.\r\n" +
				"9. output -> to print the collection.\r\n" +
				"10. exit -> to exit.\r\n");
		}
		static GraphColection ReadFromFile(string path)
		{
			FileStream fileStream = File.Open(path, FileMode.OpenOrCreate);
			return (GraphColection)jsonSerializer.ReadObject(fileStream);
		}
		static void WriteToFile(string path, GraphColection gc)
		{
			FileStream fileStream = File.Open(path, FileMode.OpenOrCreate);
			jsonSerializer.WriteObject(fileStream, gc);
		}
		static Flight FlightConsoleInput()
		{
			Console.WriteLine("Input Flight ID:");
			int id = int.Parse(Console.ReadLine());
            Console.WriteLine("Input Flight Departure City:");
            City departureCity = Enum.Parse<City>(Console.ReadLine());
            Console.WriteLine("Input Flight Arrival City:");
            City arrivalCity = Enum.Parse<City>(Console.ReadLine());
            Console.WriteLine("Input Flight Departure Datetime:");
            DateTime departureDatetime = DateTime.Parse(Console.ReadLine());
            Console.WriteLine("Input Flight Arrival Datetime:");
            DateTime arrivalDatetime = DateTime.Parse(Console.ReadLine());
            Console.WriteLine("Input Flight Airline:");
            Airline airline = Enum.Parse<Airline>(Console.ReadLine());
            Console.WriteLine("Input Flight Price:");
            double price = double.Parse(Console.ReadLine());
			return new Flight(id, departureCity, arrivalCity, departureDatetime, arrivalDatetime, airline, price);
		}
		static void Main(string[] args)
		{
			Menu();
			GraphColection graphColection = new GraphColection();
			Pathfinder pathfinder = new Pathfinder(ref graphColection);
			int key;
			string comand;
			while (true)
			{
				try
				{
					comand = Console.ReadLine();
					switch (comand)
					{
						case "read":
							Console.WriteLine("Enter file path");
							graphColection = ReadFromFile(Console.ReadLine());
							break;
						case "write":
							Console.WriteLine("Enter file path");
							WriteToFile(Console.ReadLine(), graphColection);
							break;
						case "add":
							graphColection.Add(FlightConsoleInput());
							break;
						case "delete":
							Console.WriteLine("Enter Flight ID");
							key = int.Parse(Console.ReadLine());
							graphColection.Remove(key);
							break;
						case "edit":
							Console.WriteLine("Enter target ID");
							key = int.Parse(Console.ReadLine());
							graphColection.Modify(key, FlightConsoleInput());
							break;
						case "search":
							Console.WriteLine("Input Flight Departure City:");
							City departureCity = Enum.Parse<City>(Console.ReadLine());
							Console.WriteLine("Input Flight Arrival City:");
							City arrivalCity = Enum.Parse<City>(Console.ReadLine());
							Console.WriteLine("Input Flight Departure Datetime:");
							DateTime departureDatetime = DateTime.Parse(Console.ReadLine());
							List<Flight> flights = pathfinder.GetPath(departureCity, arrivalCity, departureDatetime).ToList<Flight>();
							flights.ForEach(t => Console.WriteLine(t));
							break;
						case "by month":
							//TODO
							break;
						case "by country":
							//TODO
							break;
						case "output":
							foreach (var city in graphColection.GetCities())
							{
								graphColection.GetFlightsByCity(city).ToList().ForEach(t => Console.WriteLine(t));
							}
							break;
						case "exit":
							return;
							break;
						default:
							Console.WriteLine("Invalid comand");
							break;
					}
				}
				catch (Exception ex) { Console.WriteLine(ex.Message); }
			}
		}
	}
}