using System;
using ReservationApplication;

namespace ReservationApplication
{
    public class ManagerReservationDeleter
    {
        private Database db = new Database();

        public void DeleteReservation()
        {
            Console.WriteLine("Voer uw reserverings-ID in:");
            if (int.TryParse(Console.ReadLine(), out int reservationId))
            {
                var reservation = db.GetReservationById(reservationId);

                if (reservation != null)
                {
                    Console.WriteLine("Reserveringsdetails:");
                    Console.WriteLine($"Reservering ID: {reservation.ReservationId}");
                    Console.WriteLine($"Tafel ID: {reservation.TableId}");
                    Console.WriteLine($"Aantal Personen: {reservation.NumOfPeople}");
                    Console.WriteLine($"Naam: {reservation.FirstName} {reservation.Infix} {reservation.LastName}");
                    Console.WriteLine($"Telefoonnummer: {reservation.PhoneNumber}");
                    Console.WriteLine($"E-mail: {reservation.Email}");
                    Console.WriteLine($"Datum: {reservation.Date}");
                    Console.WriteLine($"Tijdslot: {reservation.TimeSlot}");
                    Console.WriteLine($"Opmerkingen: {reservation.Remarks}");

                    Console.WriteLine("Wilt u deze reservatie verwijderen? (ja/nee)");
                    string deleteConfirmation = Console.ReadLine()?.Trim().ToLower();

                    if (deleteConfirmation == "ja")
                    {
                        db.DeleteReservation(reservationId);
                        Console.WriteLine("Reservering succesvol verwijderd.");
                    }
                    else
                    {
                        Console.WriteLine("Reservering niet verwijderd.");
                    }
                }
                else
                {
                    Console.WriteLine("Reservering niet gevonden.");
                }
            }
            else
            {
                Console.WriteLine("Ongeldige invoer. Voer een geldig reserverings-ID in.");
            }

            Console.WriteLine("Druk op een toets om terug te keren naar het menu.");
            Console.ReadKey();
            ManagerMenu.StartUp();
        }
    }
}