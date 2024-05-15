using System;
using System.Text.RegularExpressions;

namespace ReservationApplication
{
    class TestApplicatie
    {
        private Database db = new Database();

        public void ReservationSystem()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("*******************************************");
            Console.WriteLine("* Welkom bij het reserveringsapplicatie! *");
            Console.WriteLine("*******************************************");
            Console.ResetColor();
            Console.WriteLine();

            bool reservationConfirmed = false;
            
            while (!reservationConfirmed)
            {
                bool date_checker = false;
                bool first_name_checker = false;
                bool last_name_checker = false;
                bool phoneNumber_checker = false;
                bool people_checker = false;
                bool mail_checker = false;
                
                string dateString = "";
                DateTime reservationDate = DateTime.MinValue;
                int numberOfPeople = 0;
                string firstName = "";
                string lastName = "";
                string phoneNumber = "";
                string email = "";
                string infix = "";

                Console.WriteLine();
                Console.WriteLine("********* Reserveringsgegevens ************");
                Console.WriteLine();

                // Datum invoeren
                while (!date_checker)
                {
                    Console.Write("Voer uw reserveringsdatum in (dd-MM-yyyy): ");
                    dateString = Console.ReadLine() ?? "";
                    if (DateTime.TryParseExact(dateString, "dd-MM-yyyy", null, System.Globalization.DateTimeStyles.None, out reservationDate))
                    {
                        break;
                    }
                    Console.WriteLine("Ongeldige invoer. Probeer: (dd-MM-yyyy)");
                }

                // Tijdslot selecteren
                string[] timeSlots = { "18:00-19:59", "20:00-21:59", "22:00-23:59" };
                string timeSlot = "";
                Console.WriteLine("Selecteer een tijdslot:");
                for (int i = 0; i < timeSlots.Length; i++)
                {
                    Console.WriteLine($"{i + 1}. {timeSlots[i]}");
                }
                while (true)
                {
                    Console.Write("Kies een optie (1-3): ");
                    if (int.TryParse(Console.ReadLine(), out int slot) && slot >= 1 && slot <= 3)
                    {
                        timeSlot = timeSlots[slot - 1];
                        break;
                    }
                    Console.WriteLine("Ongeldige invoer. Kies een optie tussen 1 en 3.");
                }

                // Aantal personen
                while (!people_checker)
                {
                    Console.Write("Aantal personen: ");
                    if (int.TryParse(Console.ReadLine(), out numberOfPeople) && numberOfPeople > 0 && numberOfPeople <= 6)
                    {
                        break;
                    }
                    Console.WriteLine("Ongeldig aantal personen.");
                }

                // Persoonsgegevens
                while (!first_name_checker)
                {
                    Console.WriteLine("\nGraag uw contactgegevens achterlaten:");
                    Console.Write("Voornaam: ");
                    firstName = Console.ReadLine() ?? "";
                    if (!string.IsNullOrWhiteSpace(firstName) && Regex.IsMatch(firstName, @"^[a-zA-Z]+$"))
                    {
                        first_name_checker = true;
                    }
                    else
                    {
                        Console.WriteLine("Invalid input. Probeer alleen letters te gebruiken.");
                    }
                }

                Console.Write("Tussenvoegsel (indien van toepassing, anders druk op Enter): ");
                infix = Console.ReadLine() ?? "";

                while (!last_name_checker)
                {
                    Console.Write("Achternaam: ");
                    lastName = Console.ReadLine() ?? "";
                    if (!string.IsNullOrWhiteSpace(lastName) && Regex.IsMatch(lastName, @"^[a-zA-Z]+$"))
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

                Console.WriteLine("Opmerkingen/verzoeken: ");
                string remarks = Console.ReadLine() ?? "";

                // Reservering maken
                ReservationSystem reservationSystem = new ReservationSystem();
                (int tableId, DateTime nextAvailableDate, string nextAvailableTimeSlot) = reservationSystem.ReserveTableForGroup(numberOfPeople, reservationDate, timeSlot);


                if (tableId == -1)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"\nGeen beschikbare tafels voor de opgegeven criteria op {reservationDate.ToShortDateString()} tijdens {timeSlot}.");
                    Console.WriteLine($"De eerstvolgende beschikbare datum en tijdslot zijn: {nextAvailableDate.ToShortDateString()} {nextAvailableTimeSlot}.");
                    Console.ResetColor();
                    continue;
                }

                // Reserveringsgegevens bevestigen
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("*******************************************");
                Console.WriteLine("* Reserveringsgegevens bevestigen          *");
                Console.WriteLine("*******************************************");
                Console.ResetColor();
                Console.WriteLine($"Datum: {reservationDate.ToShortDateString()}");
                Console.WriteLine($"Tijdslot: {timeSlot}");
                Console.WriteLine($"Tafelnummer: {tableId}");
                Console.WriteLine($"Aantal personen: {numberOfPeople}");
                Console.WriteLine($"Naam: {firstName} {infix} {lastName}");
                Console.WriteLine($"Telefoonnummer: {phoneNumber}");
                Console.WriteLine($"E-mail: {email}");
                Console.WriteLine($"Opmerkingen/verzoeken: {remarks}");
                Console.WriteLine();

                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("Is deze informatie correct? (ja/nee): ");
                string confirmation = Console.ReadLine()?.Trim().ToLower();
                Console.ResetColor();

                if (confirmation == "ja")
                {
                    var (success, suggestedDate, suggestedTimeSlot, ReservationId) = db.AddReservation(numberOfPeople, firstName, infix, lastName, phoneNumber, email, reservationDate, timeSlot, tableId, remarks);
                    if (success)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("\nUw reservering is succesvol verwerkt.");
                        Console.ResetColor();
                        reservationSystem.SendEmail(email, reservationDate, timeSlot, firstName, numberOfPeople, ReservationId);
                        reservationConfirmed = true;
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($"\nKon niet reserveren op {reservationDate.ToShortDateString()} tijdens {timeSlot}. Probeer op {suggestedDate.ToShortDateString()} tijdens {suggestedTimeSlot}.");
                        Console.ResetColor();
                    }
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("\nReservering niet bevestigd, start opnieuw.");
                    Console.ResetColor();
                }
            }
        }
    }
}
