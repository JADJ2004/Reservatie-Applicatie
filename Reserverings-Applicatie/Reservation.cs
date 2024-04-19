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
                Console.Write("Voer uw reserveringsdatum in (dd-MM-yyyy): ");
                string dateString = Console.ReadLine() ?? "";
                DateTime reservationDate;
                if (!DateTime.TryParseExact(dateString, "dd-MM-yyyy", null, System.Globalization.DateTimeStyles.None, out reservationDate))
                {
                    Console.WriteLine("Incorrecte formaat. Probeer: (dd-MM-yyyy)");
                    continue;
                }

                Console.WriteLine("Wilt u aan het raam zitten? (ja/nee): ");
                bool wantsWindow = Console.ReadLine()?.Trim().ToLower() == "ja";

                Console.Write("Aantal personen: ");
                int numberOfPeople;
                if (!int.TryParse(Console.ReadLine(), out numberOfPeople) || numberOfPeople <= 0)
                {
                    Console.WriteLine("Ongeldig aantal personen.");
                    continue;
                }
                Console.Write("Voornaam: ");
                string firstName = Console.ReadLine() ?? "";
                if (string.IsNullOrWhiteSpace(firstName))
                {
                    Console.WriteLine("Voornaam mag niet leeg zijn.");
                    continue;
                }

                Console.Write("Tussenvoegsel (indien van toepassing, anders druk op Enter): ");
                string infix = Console.ReadLine() ?? "";

                Console.Write("Achternaam: ");
                string lastName = Console.ReadLine() ?? "";
                if (string.IsNullOrWhiteSpace(lastName))
                {
                    Console.WriteLine("Achternaam mag niet leeg zijn.");
                    continue;
                }

                Console.Write("Telefoonnummer: ");
                string phoneNumber = Console.ReadLine() ?? "";
                if (phoneNumber.Length != 10 || !long.TryParse(phoneNumber, out _))
                {
                    Console.WriteLine("Een geldig telefoonnummer moet uit 10 cijfers bestaan.");
                    continue;
                }

                Console.Write("E-mail: ");
                string email = Console.ReadLine() ?? "";
                if (!email.Contains("@") || !email.Contains("."))
                {
                    Console.WriteLine("Ongeldig e-mailadres.");
                    continue;
                }
                
                Console.Write("Eventuele opmerkingen/verzoeken: (mag ook leeg zijn) ");
                string remarks = Console.ReadLine() ?? "";

                ReservationSystem reservationSystem = new ReservationSystem();
                var (tableId, nextAvailableDate) = reservationSystem.ReserveTableForGroup(numberOfPeople, wantsWindow, reservationDate);
                if (tableId == -1)
                {
                    Console.WriteLine($"Geen beschikbare tafels voor de opgegeven criteria op {reservationDate.ToShortDateString()}.");
                    Console.WriteLine($"De eerstvolgende beschikbare datum is: {nextAvailableDate.ToShortDateString()}.");
                    continue;
                }

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