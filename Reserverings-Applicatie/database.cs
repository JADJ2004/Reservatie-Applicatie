using System;
using System.Data.SQLite;

class Program
{
    static void Main(string[] args)
    {
        // Verbinding maken met de database
        string connectionString = "Data Source=restaurant.db;Version=3;";
        using (SQLiteConnection connection = new SQLiteConnection(connectionString))
        {
            connection.Open();

            // Tabel aanmaken voor reserveringen
            string createTableQuery = @"CREATE TABLE IF NOT EXISTS Reservations (
                                            ReservationID INTEGER PRIMARY KEY AUTOINCREMENT,
                                            CustomerName TEXT NOT NULL,
                                            ReservationDate TEXT NOT NULL,
                                            NumberOfGuests INTEGER NOT NULL
                                        );";
            using (SQLiteCommand command = new SQLiteCommand(createTableQuery, connection))
            {
                command.ExecuteNonQuery();
            }

            // Voorbeeldgegevens invoegen
            string insertDataQuery = @"INSERT INTO Reservations (CustomerName, ReservationDate, NumberOfGuests)
                                        VALUES ('John Doe', '2024-03-18', 4),
                                               ('Jane Smith', '2024-03-20', 2),
                                               ('Michael Johnson', '2024-03-21', 6);";
            using (SQLiteCommand command = new SQLiteCommand(insertDataQuery, connection))
            {
                command.ExecuteNonQuery();
            }

            Console.WriteLine("Database en tabel zijn succesvol aangemaakt en voorbeeldgegevens zijn toegevoegd.");
        }
    }
}
