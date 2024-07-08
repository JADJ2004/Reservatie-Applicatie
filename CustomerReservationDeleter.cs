using System;
using Microsoft.Data.Sqlite;
using ReservationApplication;
using System.Text;

namespace Customer_Reservation_Deleter
{
    public static class CRD
    {
        private static Database db = new Database();

        public static void ReservationDeleter()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("************************************************************************************************/");
            Console.WriteLine("█▀█ █▀▀ █▀ █▀▀ █▀█ █░█ █▀▀ █▀█ █ █▄░█ █▀▀   ▄▀█ █▄░█ █▄░█ █░█ █░░ █▀▀ █▀█ █▀▀ █▄░█");
            Console.WriteLine("█▀▄ ██▄ ▄█ ██▄ █▀▄ ▀▄▀ ██▄ █▀▄ █ █░▀█ █▄█   █▀█ █░▀█ █░▀█ █▄█ █▄▄ ██▄ █▀▄ ██▄ █░▀█");
            Console.WriteLine("************************************************************************************************/");
            Console.ResetColor();
            Console.WriteLine();
            
            Console.WriteLine("Voer uw reserverings-ID in:");
            string input = ReadInputWithEscape();
            if (int.TryParse(input, out int reservationId))
            {
                var reservation = db.GetReservationById(reservationId);

                if (reservation != null)
                {
                    Console.WriteLine("Reserveringsdetails:");
                    Console.WriteLine($"Reservering ID: {reservation.ReservationId}");
                    Console.WriteLine($"Aantal Personen: {reservation.NumOfPeople}");
                    Console.WriteLine($"Naam: {reservation.FirstName} {reservation.Infix} {reservation.LastName}");
                    Console.WriteLine($"Telefoonnummer: {reservation.PhoneNumber}");
                    Console.WriteLine($"E-mail: {reservation.Email}");
                    Console.WriteLine($"Datum: {reservation.Date}");
                    Console.WriteLine($"Tijdslot: {reservation.TimeSlot}");
                    Console.WriteLine($"Opmerkingen: {reservation.Remarks}");

                    Console.WriteLine("Wilt u deze reservatie verwijderen?");
                    string changeConfirmation = ReadInputWithEscape()?.Trim().ToLower();
                    
                    if (changeConfirmation == "ja")
                    {
                        db.DeleteReservation(reservationId);
                        Console.WriteLine("Reservering succesvol verwijderd.");
                    }
                    else
                    {
                        Console.WriteLine("Je gaat terug naar het hoofdmenu.");
                        Menus.StartUp();
                        return;
                    }
                }
                else
                {
                    Console.WriteLine("Reservering niet gevonden.");
                    Menus.StartUp();
                    return;
                }
            }
            else
            {
                Console.WriteLine("Ongeldige invoer. Voer een geldig reserverings-ID in.");
                Console.WriteLine("Je gaat terug naar het hoofdmenu.");
                Menus.StartUp();
                return;
            }

            Console.WriteLine("Druk op een toets om terug te keren naar het menu.");
            Console.ReadKey();
            Menus.StartUp();
        }

        private static string ReadInputWithEscape()
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
