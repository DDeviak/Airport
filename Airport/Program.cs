using Newtonsoft.Json;

namespace Airport
{
    internal class Manager
    {
        static JsonSerializer jsonSerializer = new JsonSerializer();
        public enum Month
        {
            NotSet = 0,
            January = 1,
            February = 2,
            March = 3,
            April = 4,
            May = 5,
            June = 6,
            July = 7,
            August = 8,
            September = 9,
            October = 10,
            November = 11,
            December = 12
        }
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
            jsonSerializer.DateFormatString = "dd/MM/yyyy hh:mm:ss";
            jsonSerializer.Formatting = Formatting.Indented;
            Pathfinder pathfinder = new Pathfinder(new GraphCollection());
            int key;
            string comand;
            while (true)
            {
                try
                {
                    Console.Write("Choose option: ");
                    comand = Console.ReadLine();
                    switch (comand)
                    {
                        case "read":
                            Console.WriteLine("Enter file name:");
                            using (StreamReader sr = File.OpenText(Console.ReadLine())) 
                            {
                                foreach (Flight temp in (Flight[])jsonSerializer.Deserialize(sr, typeof(Flight[])))
                                {
                                    try
                                    {
                                        pathfinder.Graph.Add(temp);
                                    }
                                    catch (Exception ex)
                                    {
                                        Console.WriteLine(ex.Message);
                                        Console.WriteLine(temp);
                                        Console.WriteLine("Record won`t be added to collection");
                                    }
                                } 
                            }
                            break;
                        case "write":
                            Console.WriteLine("Enter file name:");
                            using (StreamWriter sw = File.CreateText(Console.ReadLine())) { jsonSerializer.Serialize(sw, pathfinder.Graph); }
                            break;
                        case "add":
                            Flight t = FlightFactory.FlightConsoleInput();
                            if (t != null)
                                pathfinder.Graph.Add(t);
                            break;
                        case "delete":
                            Console.WriteLine("Enter Flight ID");
                            key = int.Parse(Console.ReadLine());
                            pathfinder.Graph.Remove(key);
                            break;
                        case "edit":
                            Console.WriteLine("Enter target ID");
                            key = int.Parse(Console.ReadLine());
                            Console.WriteLine("Enter property name");
                            string pName = Console.ReadLine();
                            Console.WriteLine("Enter property value");
                            string pVal = Console.ReadLine();
                            pathfinder.Graph.Modify(key, pName, pVal);
                            break;
                        case "search":
                            Console.WriteLine("Input Flight Departure City:");
                            City departureCity = Enum.Parse<City>(Console.ReadLine());
                            Console.WriteLine("Input Flight Arrival City:");
                            City arrivalCity = Enum.Parse<City>(Console.ReadLine());
                            Console.WriteLine("Input Flight Departure Datetime:");
                            DateTime departureDatetime = DateTime.Parse(Console.ReadLine());
                            pathfinder.GetPath(departureCity, arrivalCity, departureDatetime).ToList().ForEach(t => Console.WriteLine(t));
                            break;
                        case "by month":
                            Console.WriteLine("Input Flight Departure City:");
                            departureCity = Enum.Parse<City>(Console.ReadLine());
                            Console.WriteLine("Input Flight Arrival City:");
                            arrivalCity = Enum.Parse<City>(Console.ReadLine());
                            Console.WriteLine("Input Flight Departure Year:");
                            int year = int.Parse(Console.ReadLine());
                            Console.WriteLine("Input Flight Departure Month:");
                            Month month = Enum.Parse<Month>(Console.ReadLine());
                            pathfinder.GetFlightsByMonth(departureCity, arrivalCity, year, (int)month).ToList().ForEach(fbd =>
                            {
                                Console.WriteLine(fbd.Key.ToShortDateString());
                                if (fbd.Value == null)
                                {
                                    Console.WriteLine("\tNo flights this day");
                                    return;
                                }
                                fbd.Value.ToList().ForEach(t => Console.WriteLine("\t" + t));
                            });
                            break;
                        case "by country":
                            Console.WriteLine("Input Flight Departure City:");
                            departureCity = Enum.Parse<City>(Console.ReadLine());
                            Console.WriteLine("Input Flight Arrival Country:");
                            string arrivalCountry = Console.ReadLine();
                            Console.WriteLine("Input Flight Departure Datetime:");
                            departureDatetime = DateTime.Parse(Console.ReadLine());

                            pathfinder.GetFlightsToCountry(departureCity,arrivalCountry,departureDatetime).ToList().ForEach(fbc =>
                            {
                                Console.WriteLine(fbc.Key);
                                fbc.Value.ToList().ForEach(t => Console.WriteLine("\t" + t));
                            });
                            break;
                        case "output":
                            foreach (City city in pathfinder.Graph.GetCities())
                            {
                                pathfinder.Graph.GetFlightsByCity(city).ToList().ForEach(t => Console.WriteLine(t));
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