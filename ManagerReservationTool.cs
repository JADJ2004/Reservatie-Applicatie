using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Text;

namespace ReservationApplication
{
    public class LargeReservationManager
    {
        private Database db = new Database();
        private ReservationSystem reservationSystem = new ReservationSystem();

        public void MakeLargeReservation()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("************************************************************************************************/");
            Console.WriteLine("█▀█ █▀▀ █▀ █▀▀ █▀█ █░█ █▀▀ █▀█ █ █▄░█ █▀▀   ▄▀█ ▄▀█ █▄░█ █▀█ ▄▀█ █▀ █▀ █▀▀ █▄░█");
            Console.WriteLine("█▀▄ ██▄ ▄█ ██▄ █▀▄ ▀▄▀ ██▄ █▀▄ █ █░▀█ █▄█   █▀█ █▀█ █░▀█ █▀▀ █▀█ ▄█ ▄█ ██▄ █░▀█");
            Console.WriteLine("************************************************************************************************/");
            Console.ResetColor();
            Console.WriteLine();

            DateTime reservationDate = DateTime.MinValue;
            string timeSlot = "";
            int numberOfPeople = 0;
            List<int> selectedTableIds = new List<int>();
            string firstName = "";
            string lastName = "";
            string phoneNumber = "";
            string email = "";
            string infix = "";
            string remarks = "";

            DateTime today = DateTime.Now.Date;

            // Datum invoeren
            bool dateChecker = false;
            while (!dateChecker)
            {
                Console.Write("Voer uw reserveringsdatum in (dd-MM-yyyy): ");
                string dateString = ReadInputWithEscape() ?? "";
                if (DateTime.TryParseExact(dateString, "dd-MM-yyyy", null, System.Globalization.DateTimeStyles.None, out reservationDate) && today < reservationDate)
                {
                    dateChecker = true;
                }
                else if (!DateTime.TryParseExact(dateString, "dd-MM-yyyy", null, System.Globalization.DateTimeStyles.None, out reservationDate))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Ongeldige invoer. Probeer: (dd-MM-yyyy)");
                    Console.WriteLine("");
                    Console.ResetColor();
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Je kan geen tafel reserveren voor een datum in het verleden");
                    Console.WriteLine("");
                    Console.ResetColor();
                }
            }

            // Tijdslot selecteren
            string[] timeSlots = { "18:00-19:59", "20:00-21:59", "22:00-23:59" };
            Console.WriteLine("Selecteer een tijdslot:");
            for (int i = 0; i < timeSlots.Length; i++)
            {
                Console.WriteLine($"{i + 1}. {timeSlots[i]}");
            }
            while (true)
            {
                Console.Write("Kies een optie (1-3): ");
                string slotInput = ReadInputWithEscape();
                if (int.TryParse(slotInput, out int slot) && slot >= 1 && slot <= 3)
                {
                    timeSlot = timeSlots[slot - 1];
                    break;
                }
                Console.WriteLine("Ongeldige invoer. Kies een optie tussen 1 en 3.");
            }

            // Aantal personen
            while (true)
            {
                Console.Write("Aantal personen: ");
                string peopleInput = ReadInputWithEscape();
                if (int.TryParse(peopleInput, out numberOfPeople) && numberOfPeople > 6)
                {
                    break;
                }
                Console.WriteLine("Ongeldig aantal personen. Moet groter zijn dan 6.");
            }

            // Beschikbare tafels tonen en selecteren
            DisplayAvailableTables(reservationDate, timeSlot, selectedTableIds, numberOfPeople);

            // Persoonsgegevens
            Checker check = new Checker();
            while (true)
            {
                if (check.Persoonsgegevens(out firstName))
                {
                    break;
                }
            }

            Console.Write("Tussenvoegsel (indien van toepassing, anders druk op Enter): ");
            infix = ReadInputWithEscape() ?? "";

            while (true)
            {
                if (check.Achternaam(out lastName))
                {
                    break;
                }
            }

            while (true)
            {
                if (check.phoneChecker(out phoneNumber))
                {
                    break;
                }
            }

            while (true)
            {
                Console.Write("E-mail: ");
                email = ReadInputWithEscape() ?? "";
                if (email.Contains("@") && email.Contains("."))
                {
                    break;
                }
                Console.WriteLine("Ongeldige e-mail. Probeer een echt e-mail in te vullen.");
            }

            Console.WriteLine("Opmerkingen:");
            remarks = ReadInputWithEscape() ?? "";

            // Bevestig de reservering
            bool reservationConfirmed = ConfirmReservation(reservationDate, timeSlot, numberOfPeople, selectedTableIds, firstName, infix, lastName, phoneNumber, email, remarks);

            if (reservationConfirmed)
            {
                var (success, suggestedDate, suggestedTimeSlot, reservationId) = db.AddLargeReservation(selectedTableIds, numberOfPeople, firstName, infix, lastName, phoneNumber, email, reservationDate, timeSlot, remarks);

                if (success)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("\nUw grote reservering is succesvol verwerkt.");
                    Console.ResetColor();
                    reservationSystem.SendEmail(email, reservationDate, timeSlot, firstName, numberOfPeople, reservationId);
                    Menus.StartUp();
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"\nKon niet reserveren op {reservationDate.ToShortDateString()} tijdens {timeSlot}. Probeer op {suggestedDate.ToShortDateString()} tijdens {suggestedTimeSlot}.");
                    Console.ResetColor();
                }
            }
        }

        private void DisplayAvailableTables(DateTime date, string timeSlot, List<int> selectedTableIds, int numOfPeople)
        {
            var availableTables = db.GetAvailableTablesForLargeReservation(date, timeSlot);
            Console.WriteLine("Beschikbare tafels:");
            foreach (var table in availableTables)
            {
                Console.WriteLine($"Tafel {table.TableId} - Capaciteit: {table.Capacity}");
            }

            while (true)
            {
                Console.Write("Selecteer een tafel-ID (of druk Enter om te bevestigen): ");
                string input = ReadInputWithEscape();
                if (string.IsNullOrWhiteSpace(input))
                {
                    break;
                }
                if (int.TryParse(input, out int tableId) && availableTables.Exists(t => t.TableId == tableId))
                {
                    selectedTableIds.Add(tableId);
                    numOfPeople -= availableTables.Find(t => t.TableId == tableId).Capacity;
                    if (numOfPeople <= 0)
                    {
                        break;
                    }
                }
                else
                {
                    Console.WriteLine("Ongeldige tafel-ID.");
                }
            }
        }

        private bool ConfirmReservation(DateTime date, string timeSlot, int numOfPeople, List<int> tableIds, string firstName, string infix, string lastName, string phoneNumber, string email, string remarks)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("*******************************************");
            Console.WriteLine("* Reserveringsgegevens bevestigen          *");
            Console.WriteLine("*******************************************");
            Console.ResetColor();
            Console.WriteLine($"Datum: {date.ToShortDateString()}");
            Console.WriteLine($"Tijdslot: {timeSlot}");
            Console.WriteLine($"Aantal personen: {numOfPeople}");
            Console.WriteLine($"Geselecteerde tafels: {string.Join(", ", tableIds)}");
            Console.WriteLine($"Naam: {firstName} {infix} {lastName}");
            Console.WriteLine($"Telefoonnummer: {phoneNumber}");
            Console.WriteLine($"E-mail: {email}");
            Console.WriteLine($"Opmerkingen/verzoeken: {remarks}");
            Console.WriteLine();

            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("Is deze informatie correct? (ja/nee): ");
            string confirmation = ReadInputWithEscape()?.Trim().ToLower();
            Console.ResetColor();
            

            return confirmation == "ja";
            
        }

        private string ReadInputWithEscape()
        {
            var input = new StringBuilder();
            int cursorPosition = Console.CursorLeft;

            while (true)
            {
                var key = Console.ReadKey(intercept: true);
                if (key.Key == ConsoleKey.Enter)
                {
                    Console.WriteLine();
                    break;
                }
                if (key.Key == ConsoleKey.Escape)
                {
                    Menus.StartUp();
                    break;
                }
                if (key.Key == ConsoleKey.Backspace)
                {
                    if (input.Length > 0 && Console.CursorLeft > cursorPosition)
                    {
                        input.Remove(input.Length - 1, 1);
                        Console.Write("\b \b");
                    }
                }
                else if (char.IsWhiteSpace(key.KeyChar) && input.Length == 0)
                {
                    // Ignore space at the beginning
                    continue;
                }
                else
                {
                    input.Append(key.KeyChar);
                    Console.Write(key.KeyChar);
                }
            }
            return input.ToString();
        }
    }
}
