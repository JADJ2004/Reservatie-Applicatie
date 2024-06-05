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
            List<string> weekDates = db.GetCurrentWeekDates();
            var reservationsDetails = db.GetReservationsDetailsForWeek(weekDates);

            string prompt = "Selecteer een datum om reserveringen te bekijken:";
            List<string> options = new List<string>();

            foreach (var date in weekDates)
            {
                var (occupiedTables, totalPeople) = reservationsDetails[date];
                options.Add($"{date} ({occupiedTables} tafels bezet, {totalPeople} personen)");
            }

            UserInterface dateMenu = new UserInterface(prompt, options.ToArray(), () => ManagerMenu.StartUp());
            int selectedIndex = dateMenu.Run();

            string selectedDate = weekDates[selectedIndex];
            ShowReservationsForDate(selectedDate);
        }

        private void ShowReservationsForDate(string date)
        {
            Console.Clear();
            Console.WriteLine($"********* Reserveringen voor {date} ************");
            var reservations = db.GetReservationsByDate(date);

            if (reservations.Count == 0)
            {
                Console.WriteLine("Er zijn geen reserveringen gevonden voor deze datum.");
            }
            else
            {
                foreach (var reservation in reservations)
                {
                    Console.WriteLine($"Reservering ID: {reservation.ReservationId}");
                    Console.WriteLine($"Tafel ID: {reservation.TableId}");
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

            Console.WriteLine("Druk op een toets om terug te keren naar het menu.");
            Console.ReadKey();
            ManagerMenu.StartUp();
        }
    }
}
