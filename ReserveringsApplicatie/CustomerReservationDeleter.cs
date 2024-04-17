using System;
using Microsoft.Data.Sqlite;

public partial class Database
{
    public bool DeleteReservation(string firstName, string lastName, string date)
    {
        bool deletionSuccess = false;
        using (var connection = new SqliteConnection(ConnectionString))
        {
            connection.Open();
            var sqlQuery = @"
                DELETE FROM Reservations 
                WHERE First_name = @FirstName AND Last_name = @LastName AND Date = @Date";
            using (var command = new SqliteCommand(sqlQuery, connection))
            {
                command.Parameters.AddWithValue("@FirstName", firstName);
                command.Parameters.AddWithValue("@LastName", lastName);
                command.Parameters.AddWithValue("@Date", date);

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
            bool CRD_checker = false;
            while (!CRD_checker)
            {
                bool date_checker = false;
                bool first_name_checker = false;
                bool last_name_checker = false;

                string date_CRD = "";
                string name = "";
                string surname = "";

                while (!date_checker)
                {
                    Console.Write("Voer uw reserveringsdatum in (dd-MM-yyyy): ");
                    date_CRD = Console.ReadLine() ?? "";
                    DateTime parsedDate;
                    if (DateTime.TryParseExact(date_CRD, "dd-MM-yyyy", null, System.Globalization.DateTimeStyles.None, out parsedDate))
                    {
                        date_checker = true;
                    }
                    else
                    {
                        Console.WriteLine("Incorrect formaat. Probeer: (dd-MM-yyyy)");
                    }
                }

                while (!first_name_checker)
                {
                    Console.Write("Voornaam: ");
                    name = Console.ReadLine() ?? "";
                    if (!string.IsNullOrWhiteSpace(name))
                    {
                        first_name_checker = true;
                    }
                    else
                    {
                        Console.WriteLine("Ongeldige invoer. Probeer alleen letters te gebruiken.");
                    }
                }

                while (!last_name_checker)
                {
                    Console.Write("Achternaam: ");
                    surname = Console.ReadLine() ?? "";
                    if (!string.IsNullOrWhiteSpace(surname))
                    {
                        last_name_checker = true;
                    }
                    else
                    {
                        Console.WriteLine("Ongeldige invoer. Probeer alleen letters te gebruiken.");
                    }
                }

                Console.WriteLine("\nReserveringsgegevens:");
                Console.WriteLine("Datum: " + date_CRD);
                Console.WriteLine("Voornaam: " + name);
                Console.WriteLine("Achternaam: " + surname);
                Console.WriteLine("Weet u zeker dat u deze reservering wilt verwijderen? (ja/nee)");
                string CRD_confirmation = Console.ReadLine()?.Trim().ToLower();
                if (CRD_confirmation == "ja")
                {
                    if (db.DeleteReservation(name, surname, date_CRD))
                    {
                        CRD_checker = true;
                        Console.WriteLine("\nReservering succesvol verwijderd!");
                    }
                    else
                    {
                        Console.WriteLine("\nReservering niet gevonden of kon niet worden verwijderd.");
                    }
                }
            }
        }
    }
}