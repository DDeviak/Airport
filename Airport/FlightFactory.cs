using System.ComponentModel;
using System.Reflection;
using System.Reflection.Emit;

namespace Airport
{
    class FlightFactory
    {
        static public Flight FlightConsoleInput()
        {
            Console.WriteLine("Input Flight ID:");
            Flight flight = new Flight(int.Parse(Console.ReadLine()));

            Console.WriteLine("Type \"cancel\" to stop");
            PropertyInfo[] props = flight.GetType().GetProperties();
            for (int i = 0; i < props.Length; i++)
            {
                try
                {
                    if (props[i].Name == "ID") continue;
                    Console.WriteLine("Input {0}:", props[i].Name);
                    string input = Console.ReadLine();
                    if (input == "cancel") return null;
                    props[i].SetValue(flight, TypeDescriptor.GetConverter(props[i].PropertyType).ConvertFrom(input), null);
                }
                catch (TargetInvocationException ex)
                {
                    Console.WriteLine(ex.InnerException.Message);
                    i--;
                }
            }
            return flight;
        }
    }
}
