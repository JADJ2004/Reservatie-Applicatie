using System;

namespace ReservationApplication
{
    class TestAplicatie
    {
        Database db = new Database();

        public void ReservationSystem()
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
            string addition = "";

            while (date_checker != true)
            {
                Console.WriteLine("Welkom bij het reserveringsapplicatie van YES!");
                Console.Write("Voer uw reserveringsdatum in (dd-MM-yyyy): ");
                date = Console.ReadLine() ?? "";
                DateTime parsedDate;
                if (DateTime.TryParseExact(date, "dd-MM-yyyy", null, System.Globalization.DateTimeStyles.None, out parsedDate))
                {
                    date_checker = true;
                }
                else
                {
                    Console.WriteLine("Incorrecte formaat. Probeer: (dd-MM-yyyy)");
                }
            }

            Console.WriteLine("Wilt u aan het raam? (ja/nee):");
            bool wantWindow = Console.ReadLine()?.Trim().ToLower() == "ja";

            while (people_checker != true)
            {
                Console.Write("Aantal personen: ");
                if (int.TryParse(Console.ReadLine(), out numOfPeople) && numOfPeople > 0 && numOfPeople < 48)
                {
                    people_checker = true;
                }
                else
                {
                    Console.WriteLine("Invalid aantal personen. Het aantal personen moet tussen 1 en 48 zijn.");
                }
            }

            ReservationSystem rs = new ReservationSystem();
            rs.ReserveTableForGroup(numOfPeople, wantWindow, DateTime.ParseExact(date, "dd-MM-yyyy", null));

            while (!first_name_checker)
            {
                Console.WriteLine("\nGraag uw contactgegevens achterlaten:");
                Console.Write("Voornaam: ");
                name = Console.ReadLine() ?? "";
                if (!string.IsNullOrWhiteSpace(name))
                {
                    first_name_checker = true;
                }
                else
                {
                    Console.WriteLine("Invalid input. Probeer alleen letters te gebruiken.");
                }
            }

            Console.Write("Toevoeging: ");
            addition = Console.ReadLine() ?? "";

            while (!last_name_checker)
            {
                Console.Write("Achternaam: ");
                surname = Console.ReadLine() ?? "";
                if (!string.IsNullOrWhiteSpace(surname))
                {
                    last_name_checker = true;
                }
                else
                {
                    Console.WriteLine("Invalid input. Probeer alleen letters te gebruiken.");
                }
            }

            while (!phoneNumber_checker)
            {
                Console.Write("Telefoonnummer: ");
                phoneNumber = Console.ReadLine() ?? "";
                if (phoneNumber.Length == 10 && long.TryParse(phoneNumber, out _))
                {
                    phoneNumber_checker = true;
                }
                else
                {
                    Console.WriteLine("Telefoonnummer moet 10 cijfers lang zijn.");
                }
            }

            while (!mail_checker)
            {
                Console.Write("E-mail: ");
                email = Console.ReadLine() ?? "";
                if (email.Contains("@") && email.Contains("."))
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

            DateTime reservationDate;
            if(DateTime.TryParseExact(date, "dd-MM-yyyy", null, System.Globalization.DateTimeStyles.None, out reservationDate))
            {
                db.AddReservation(numOfPeople, name, addition, surname, int.Parse(phoneNumber), email, reservationDate);
            }

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
