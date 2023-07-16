using System;
using System.IO;
using System.Runtime.Serialization.Json;

namespace Airport
{
	internal class Manager
	{
		static DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(typeof(GraphCollection));
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
		static void Main(string[] args)
		{
			Menu();
			FileStream fileStream;
			GraphCollection graphCollection = new GraphCollection();
			Pathfinder pathfinder = new Pathfinder(ref graphCollection);
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
							fileStream = File.Open(Console.ReadLine(), FileMode.OpenOrCreate);
							graphCollection = (GraphCollection)jsonSerializer.ReadObject(fileStream);
							break;
						case "write":
							Console.WriteLine("Enter file path");
                            fileStream = File.Open(Console.ReadLine(), FileMode.OpenOrCreate);
                            jsonSerializer.WriteObject(fileStream, graphCollection);
                            break;
						case "add":
							graphCollection.Add(FlightFactory.FlightConsoleInput());
							break;
						case "delete":
							Console.WriteLine("Enter Flight ID");
							key = int.Parse(Console.ReadLine());
							graphCollection.Remove(key);
							break;
						case "edit":
							Console.WriteLine("Enter target ID");
							key = int.Parse(Console.ReadLine());
							graphCollection.Modify(key, FlightFactory.FlightConsoleInput());
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
							foreach (City city in graphCollection.GetCities())
							{
								graphCollection.GetFlightsByCity(city).ToList().ForEach(t => Console.WriteLine(t));
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