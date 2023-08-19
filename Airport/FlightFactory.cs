﻿using System.Reflection;

namespace Airport
{
    internal class FlightFactory
    {
        static public Flight? FlightConsoleInput()
        {
            Console.WriteLine("Input Flight ID:");
            Flight flight = new Flight(int.Parse(Console.ReadLine() ?? string.Empty));

            Console.WriteLine("Type \"cancel\" to stop");
            PropertyInfo[] props = flight.GetType().GetProperties();
            for (int i = 0; i < props.Length; i++)
            {
                try
                {
                    if (props[i].Name == "ID" || !props[i].CanWrite) continue;
                    Console.WriteLine("Input {0}:", props[i].Name);
                    string input = Console.ReadLine() ?? string.Empty;
                    if (input == "cancel") return null;
                    flight.SetProperty(props[i], input);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    i--;
                }
            }
            return flight;
        }
    }
}
