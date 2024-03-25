using System;

namespace ReservationApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            bool date_checker = false;
            bool first_name_checker = false;
            bool last_name_checker = false;
            bool phoneNumber_checker = false;
            bool people_checker = false;
            bool mail_checker = false;
            
            string date = "";
            int numOfPeople = 0;
            string name = "";
            string surname = "";
            string phoneNumber = "";
            string email = "";

            while (date_checker != true)
            {
                Console.WriteLine("Welkom bij het reserveringsapplicatie van YES!");
                Console.Write("Voer uw reserveringsdatum in (MM/DD/YYYY): ");
                date = Console.ReadLine() ?? "";
                var date_splitter = date.Split("/");
                if (date_splitter[0].Length == 2 && date_splitter[1].Length == 2 && date_splitter[2].Length == 4) // Kijkt of de correcte format is gebruikt
                {
                    date_checker = true;
                }
                else
                {
                    Console.WriteLine("Incorrecte formaat. Probeer: (MM/DD/YYYY)");
                }
            }

            while (people_checker != true)
            {
                Console.Write("Aantal personen: ");
                numOfPeople = int.Parse(Console.ReadLine() ?? "");
                if (numOfPeople < 1 || numOfPeople > 48) //placeholder
                {
                    people_checker = true;
                }
                else
                {
                    Console.WriteLine("Invalid hoeveelheid mensen. Hoe de hoeveelheid mensen tussen 1 en 48");
                }
            }

            while (first_name_checker != true)
            {
                Console.WriteLine("\nGraag uw contactgegevens achterlaten:");
                Console.Write("Voornaam: ");
                name = Console.ReadLine() ?? "";
                if (name is string) // kijkt of de naam alleen letters heeft
                {
                    first_name_checker = true;
                }
                else
                {
                    Console.WriteLine("Invalid input. Probeer alleen letters te gebruiken.");
                }
            }

            Console.Write("Toevoeging: ");
            string addition = Console.ReadLine() ?? "";

            while (last_name_checker != true)
            {
                Console.WriteLine("\nGraag uw contactgegevens achterlaten:");
                Console.Write("Achternaam: ");
                surname = Console.ReadLine() ?? "";
                if (surname is string) // kijkt of de naam alleen letters heeft
                {
                    last_name_checker = true;
                }
                {
                    Console.WriteLine("Invalid input. Probeer alleen letters te gebruiken.");
                }
            }

            while (phoneNumber_checker != true)
                {
                Console.Write("Telefoonnummer: ");
                phoneNumber = Console.ReadLine() ?? "";
                if (phoneNumber.Length == 10)
                {
                    phoneNumber_checker = true;
                }
                else
                {
                    Console.WriteLine("Telefoonnummer moet 10 cijfers lang zijn.");
                }
            }

            while (mail_checker != true)
            {
                Console.Write("E-mail: ");
                email = Console.ReadLine() ?? "";
                if (email.Contains("@") && email.Contains(".")) // Kijkt of de variabel bepaalde tekens erin heeft
                {
                    mail_checker = true;
                }
                else
                {
                    Console.WriteLine("Invalid email. Probeer een echt email in te vullen.");
                }
            }

            Console.WriteLine("\nHeeft u nog opmerkingen of verzoeken?");
            string comments = Console.ReadLine() ?? "";

            Console.WriteLine("\nReserveringsgegevens:");
            Console.WriteLine("Datum: " + date);
            Console.WriteLine("Aantal Personen: " + numOfPeople);
            Console.WriteLine("Voornaam: " + name);
            Console.WriteLine("Toevoering: " + addition);
            Console.WriteLine("Achternaam: " + surname);
            Console.WriteLine("Telefoonnummer: " + phoneNumber);
            Console.WriteLine("E-mail: " + email);
            Console.WriteLine("Opmerkingen: " + comments);

            Console.WriteLine("\nDank u wel! We hopen u gauw te zien bij YES!");
        }
    }
}