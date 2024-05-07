using System;

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
                Console.WriteLine();
                Console.WriteLine("********* Reserveringsgegevens ************");
                Console.WriteLine();

                // Datum invoeren
                DateTime reservationDate;
                while (true)
                {
                    Console.Write("Voer uw reserveringsdatum in (dd-MM-yyyy): ");
                    string dateString = Console.ReadLine() ?? "";
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
                int numberOfPeople;
                while (true)
                {
                    Console.Write("Aantal personen: ");
                    if (int.TryParse(Console.ReadLine(), out numberOfPeople) && numberOfPeople > 0)
                    {
                        break;
                    }
                    Console.WriteLine("Ongeldig aantal personen.");
                }

                // Persoonsgegevens
                Console.WriteLine("\nVoer uw persoonsgegevens in:");
                Console.Write("Voornaam: ");
                string firstName = Console.ReadLine() ?? "";

                Console.Write("Tussenvoegsel (indien van toepassing, anders druk op Enter): ");
                string infix = Console.ReadLine() ?? "";

                Console.Write("Achternaam: ");
                string lastName = Console.ReadLine() ?? "";

                Console.Write("Telefoonnummer: ");
                string phoneNumber = Console.ReadLine() ?? "";

                Console.Write("E-mail: ");
                string email = Console.ReadLine() ?? "";

                Console.Write("Opmerkingen/verzoeken: ");
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
                    var (success, suggestedDate, suggestedTimeSlot) = db.AddReservation(numberOfPeople, firstName, infix, lastName, phoneNumber, email, reservationDate, timeSlot, tableId, remarks);
                    if (success)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("\nUw reservering is succesvol verwerkt.");
                        Console.ResetColor();
                        reservationSystem.SendEmail(email, reservationDate, timeSlot, firstName, numberOfPeople);
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
