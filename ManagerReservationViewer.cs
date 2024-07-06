using System;
using System.Collections.Generic;
using ReservationApplication;

namespace ReservationApplication
{
    public class ManagerReservationViewer
    {
        private Database db = new Database();

        public void ViewReservationsByDate()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("************************************************************************************************/");
            Console.WriteLine("█▀█ █▀▀ █▀ █▀▀ █▀█ █░█ █▀▀ █▀█ █ █▄░█ █▀▀    █ █▄░█ ▀█ █ █▀▀ █▄░█");
            Console.WriteLine("█▀▄ ██▄ ▄█ ██▄ █▀▄ ▀▄▀ ██▄ █▀▄ █ █░▀█ █▄█    █ █░▀█ █▄ █ ██▄ █░▀█");
            Console.WriteLine("************************************************************************************************/");
            Console.ResetColor();
            Console.WriteLine();

            DateTime startDate;
            while (true)
            {
                Console.Write("Voer de startdatum in (dd-MM-yyyy): ");
                string input = Console.ReadLine();
                if (DateTime.TryParseExact(input, "dd-MM-yyyy", null, System.Globalization.DateTimeStyles.None, out startDate))
                {
                    break;
                }
                Console.WriteLine("Ongeldige datum. Probeer het opnieuw.");
            }

            List<(DateTime date, string timeSlot)> next7DaysTimeSlots = GetNext7DaysTimeSlots(startDate);

            foreach (var (date, timeSlot) in next7DaysTimeSlots)
            {
                ShowReservationsForDateAndTimeSlot(date, timeSlot);
            }

            Console.WriteLine("Druk op een toets om terug te keren naar het menu.");
            Console.ReadKey();
            ManagerMenu.StartUp();
        }

        private List<(DateTime date, string timeSlot)> GetNext7DaysTimeSlots(DateTime startDate)
        {
            List<(DateTime date, string timeSlot)> datesAndTimeSlots = new List<(DateTime date, string timeSlot)>();
            string[] timeSlots = { "18:00-19:59", "20:00-21:59", "22:00-23:59" };

            for (int i = 0; i < 7; i++)
            {
                DateTime currentDate = startDate.AddDays(i);
                foreach (var timeSlot in timeSlots)
                {
                    datesAndTimeSlots.Add((currentDate, timeSlot));
                }
            }

            return datesAndTimeSlots;
        }

        private void ShowReservationsForDateAndTimeSlot(DateTime date, string timeSlot)
        {
            string formattedDate = date.ToString("dd-MM-yyyy");
            Console.WriteLine($"********* Reserveringen voor {formattedDate} tijdens {timeSlot} ************");
            var reservations = db.GetReservationsByDateAndTimeSlot(formattedDate, timeSlot);

            if (reservations.Count == 0)
            {
                Console.WriteLine("Er zijn geen reserveringen gevonden voor deze datum en tijdslot.");
            }
            else
            {
                foreach (var reservation in reservations)
                {
                    Console.WriteLine($"Reservering ID: {reservation.ReservationId}");
                    Console.WriteLine($"Tafel ID's: {string.Join(", ", reservation.TableIds)}");
                    Console.WriteLine($"Aantal Personen: {reservation.NumOfPeople}");
                    Console.WriteLine($"Naam: {reservation.FirstName} {reservation.Infix} {reservation.LastName}");
                    Console.WriteLine($"Telefoonnummer: {reservation.PhoneNumber}");
                    Console.WriteLine($"E-mail: {reservation.Email}");
                    Console.WriteLine($"Datum: {reservation.Date}");
                    Console.WriteLine($"Tijdslot: {reservation.TimeSlot}");
                    Console.WriteLine($"Opmerkingen: {reservation.Remarks}");
                    Console.WriteLine("-----------------------------------------");
                }
            }
            Console.WriteLine();
        }
    }
}
