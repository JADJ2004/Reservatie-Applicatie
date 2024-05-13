using System;

namespace ReservationApplication
{
    class TestAplicatie
    {
        Database db = new Database();

        public void ReservationSystem()
        {
            bool reservationConfirmed = false;
            while (!reservationConfirmed)
            {
                Console.WriteLine("Welkom bij het reserveringsapplicatie van YES!");

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
                    Console.WriteLine("Incorrecte formaat. Probeer: (dd-MM-yyyy)");
                }

                // Aan het raam zitten?
                bool wantsWindow;
                while (true)
                {
                    Console.WriteLine("Wilt u aan het raam zitten? (ja/nee): ");
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

                // Voornaam
                string firstName;
                while (true)
                {
                    Console.Write("Voornaam: ");
                    firstName = Console.ReadLine() ?? "";
                    if (!string.IsNullOrWhiteSpace(firstName))
                    {
                        break;
                    }
                    Console.WriteLine("Voornaam mag niet leeg zijn.");
                }

                // Tussenvoegsel
                Console.Write("Tussenvoegsel (indien van toepassing, anders druk op Enter): ");
                string infix = Console.ReadLine() ?? "";

                // Achternaam
                string lastName;
                while (true)
                {
                    Console.Write("Achternaam: ");
                    lastName = Console.ReadLine() ?? "";
                    if (!string.IsNullOrWhiteSpace(lastName))
                    {
                        break;
                    }
                    Console.WriteLine("Achternaam mag niet leeg zijn.");
                }

                // Telefoonnummer
                string phoneNumber;
                while (true)
                {
                    Console.Write("Telefoonnummer: ");
                    phoneNumber = Console.ReadLine() ?? "";
                    if (phoneNumber.Length == 10 && long.TryParse(phoneNumber, out _))
                    {
                        break;
                    }
                    Console.WriteLine("Een geldig telefoonnummer moet uit 10 cijfers bestaan.");
                }

                // E-mail
                string email;
                while (true)
                {
                    Console.Write("E-mail: ");
                    email = Console.ReadLine() ?? "";
                    if (email.Contains("@") && email.Contains("."))
                    {
                        break;
                    }
                    Console.WriteLine("Ongeldig e-mailadres.");
                }

                // Opmerkingen/verzoeken
                Console.Write("Eventuele opmerkingen/verzoeken: (mag ook leeg zijn) ");
                string remarks = Console.ReadLine() ?? "";

                // Reservering maken
                ReservationSystem reservationSystem = new ReservationSystem();
                var (tableId, nextAvailableDate) = reservationSystem.ReserveTableForGroup(numberOfPeople, wantsWindow, reservationDate);
                if (tableId == -1)
                {
                    Console.WriteLine($"Geen beschikbare tafels voor de opgegeven criteria op {reservationDate.ToShortDateString()}.");
                    Console.WriteLine($"De eerstvolgende beschikbare datum is: {nextAvailableDate.ToShortDateString()}.");
                    continue;
                }

                // Reserveringsgegevens bevestigen
                Console.WriteLine("\nReserveringsgegevens:");
                Console.WriteLine($"Datum: {reservationDate.ToShortDateString()}");
                Console.WriteLine($"Tafelnummer: {tableId}");
                Console.WriteLine($"Aantal personen: {numberOfPeople}");
                Console.WriteLine($"Voornaam: {firstName}");
                Console.WriteLine($"Tussenvoegsel: {infix}");
                Console.WriteLine($"Achternaam: {lastName}");
                Console.WriteLine($"Telefoonnummer: {phoneNumber}");
                Console.WriteLine($"E-mail: {email}");
                Console.WriteLine($"Opmerkingen/verzoeken: {remarks}");
                Console.WriteLine("Is deze informatie correct? (ja/nee)");
                string confirmation = Console.ReadLine()?.Trim().ToLower();
                if (confirmation == "ja")
                {
                    var (success, suggestedDate) = db.AddReservation(numberOfPeople, firstName, infix, lastName, phoneNumber, email, reservationDate, tableId, remarks);
                    if (success)
                    {
                        Console.WriteLine("\nUw reservering is succesvol verwerkt.");
                        reservationConfirmed = true;
                    }
                    else
                    {
                        Console.WriteLine($"\nKon niet reserveren op {reservationDate.ToShortDateString()}. Probeer op {suggestedDate.ToShortDateString()}.");
                    }
                }
                else
                {
                    Console.WriteLine("\nReservering niet bevestigd, start opnieuw.");
                }
            }
        }
    }
}