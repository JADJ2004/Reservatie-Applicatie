using System;

namespace ReservationApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welkom bij het reserveringsapplicatie van YES!");
            Console.Write("Voer uw reserveringsdatum in (MM/DD/YYYY): ");
            string date = Console.ReadLine();

            Console.Write("Aantal personen: ");
            int AmountOfPeople = int.Parse(Console.ReadLine());

            Console.WriteLine("\nGraag uw contactgegevens achterlaten:");
            Console.Write("Voornaam: ");
            string name = Console.ReadLine();

            Console.Write("Toevoeging: ");
            string addition = Console.ReadLine();

            Console.Write("Achternaam: ");
            string surname = Console.ReadLine();

            Console.Write("Telefoonnummer: ");
            string phoneNumber = Console.ReadLine();

            Console.Write("E-mail: ");
            string email = Console.ReadLine();

            Console.WriteLine("\nHeeft u nog opmerkingen of verzoeken?");
            string comments = Console.ReadLine();

            Console.WriteLine("\nReserveringsgegevens:");
            Console.WriteLine("Datum: " + date);
            Console.WriteLine("Aantal Personen: " + numOfPeople);
            Console.WriteLine("Voornaam: " + name);
            Console.WriteLine("Toevoering: " + name);
            Console.WriteLine("Achternaam: " + surname);
            Console.WriteLine("Telefoonnummer: " + phoneNumber);
            Console.WriteLine("E-mail: " + email);
            Console.WriteLine("Opmerkingen: " + comments);

            Console.WriteLine("\nDank u wel! We hopen u gauw te zien bij YES!");
        }
    }
}

