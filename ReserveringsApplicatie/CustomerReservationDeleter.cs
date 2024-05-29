using System;
using Microsoft.Data.Sqlite;
using ReservationApplication;

public partial class Database
{
    public bool DeleteReservation(int reservationId)
    {
        bool deletionSuccess = false;
        using (var connection = new SqliteConnection(ConnectionString))
        {
            connection.Open();
            var sqlQuery = @"
                DELETE FROM Reservations 
                WHERE reservationId = @ReservationId";
            using (var command = new SqliteCommand(sqlQuery, connection))
            {
                command.Parameters.AddWithValue("@ReservationId", reservationId);

                int rowsAffected = command.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    deletionSuccess = true;
                }
            }
        }
        return deletionSuccess;
    }
}

namespace Customer_Reservation_Deleter
{
    class CRD
    {
        private Database db = new Database();

        public void ReservationDeleter()
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

                    Console.WriteLine("Wilt u deze reservatie verwijderen?");
                    string ChangeConfirmation = Console.ReadLine()?.Trim().ToLower();
                    
                    if (ChangeConfirmation == "ja")
                    {
                        db.DeleteReservation(reservationId);
                    }
                    else
                    {
                        Console.WriteLine("Je gaat terug naar het hoofdmenu.");
                        Menus.StartUp();
                    }
                }
                else
                {
                    Console.WriteLine("Reservering niet gevonden.");
                    Menus.StartUp();
                }
            }
            else
            {
                Console.WriteLine("Ongeldige invoer. Voer een geldig reserverings-ID in.");
                Console.WriteLine("Je gaat terug naar het hoofdmenu.");
                Menus.StartUp();
            }

            Console.WriteLine("Druk op een toets om terug te keren naar het menu.");
            Console.ReadKey();
            Menus.StartUp();
        }
    }
}
