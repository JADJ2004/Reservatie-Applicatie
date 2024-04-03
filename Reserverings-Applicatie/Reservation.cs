using System;
using System.Collections.Generic;

namespace ReservationApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            HashSet<int> bookedDays = new HashSet<int> { 1, 3, 5 };

            Console.WriteLine("Voer een dag in om te reserveren (1-31):");
            if (int.TryParse(Console.ReadLine(), out int chosenDay) && chosenDay >= 1 && chosenDay <= 31)
            {
                if (bookedDays.Contains(chosenDay))
                {
                    Console.WriteLine($"De gekozen dag {chosenDay} is al volgeboekt.");

                    Console.WriteLine("Beschikbare dagen voor de volgende 5 dagen:");

                    for (int i = chosenDay + 1; i <= chosenDay + 5; i++)
                    {
                        if (!bookedDays.Contains(i))
                        {
                            Console.WriteLine(i);
                        }
                    }
                }
                else
                {
                    Console.WriteLine($"Dag {chosenDay} is succesvol gereserveerd.");
                }
            }
            else
            {
                Console.WriteLine("Ongeldige invoer.");
            }
        }
    }
}
