namespace Airport
{
    class FlightFactory
    {
        static public Flight FlightConsoleInput()
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
    }
}
