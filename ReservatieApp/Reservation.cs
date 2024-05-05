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

                // Aan het raam zitten?
                bool wantsWindow;
                while (true)
                {
                    Console.Write("Wilt u aan het raam zitten? (ja/nee): ");
                    string input = Console.ReadLine()?.Trim().ToLower();
                    if (input == "ja")
                    {
                        wantsWindow = true;
                        break;
                    }
                    else if (input == "nee")
                    {
                        wantsWindow = false;
                        break;
                    }
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
                var (tableId, nextAvailableDate) = reservationSystem.ReserveTableForGroup(numberOfPeople, wantsWindow, reservationDate);
                if (tableId == -1)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"\nGeen beschikbare tafels voor de opgegeven criteria op {reservationDate.ToShortDateString()}.");
                    Console.WriteLine($"De eerstvolgende beschikbare datum is: {nextAvailableDate.ToShortDateString()}.");
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
                    var (success, suggestedDate) = db.AddReservation(numberOfPeople, firstName, infix, lastName, phoneNumber, email, reservationDate, tableId, remarks);
                    if (success)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("\nUw reservering is succesvol verwerkt.");
                        Console.ResetColor();
                        reservationSystem.SendEmail(email, reservationDate, firstName, numberOfPeople);
                        reservationConfirmed = true;
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($"\nKon niet reserveren op {reservationDate.ToShortDateString()}. Probeer op {suggestedDate.ToShortDateString()}.");
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
