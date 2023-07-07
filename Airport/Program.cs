namespace Airport
{
    internal class Manager
    {
        static void Main(string[] args)
        {
            DateTime departureDatetime = new DateTime(2023, 7, 7, 10, 0, 0);
            DateTime arrivalDatetime = new DateTime(2023, 7, 7, 15, 0, 0);
            double price = 500.0;

            Flight flight = new Flight(City.Dallas, City.SanFrancisco, departureDatetime, arrivalDatetime, Airline.ANA, price);

            Console.WriteLine(flight.ToString());
        }
    }
}