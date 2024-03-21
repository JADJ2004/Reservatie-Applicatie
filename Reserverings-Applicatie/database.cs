using System;
using System.Data.SQLite;

public partial class database
{
    public void Database_con()
    {

        string connectionString = @"Data Source=C:\Users\rensg\OneDrive\Documenten\GitHub\Reservatie-Applicatie\Reserverings-Applicatie\Mydatabase.db";
        using (SQLiteConnection connection = new SQLiteConnection(connectionString))
        {
            connection.Open();


            string createTableQuery = @"
                CREATE TABLE IF NOT EXISTS Reserveringen (
                    ReservationId INTEGER PRIMARY KEY AUTOINCREMENT,
                    Amount_people TEXT,
                    First_name TEXT,
                    Last_name TEXT,
                    Phonenumber TEXT,
                    Date TEXT,
                    Email TEXT
                )";

            using (SQLiteCommand command = new SQLiteCommand(createTableQuery, connection))
            {
                
                command.ExecuteNonQuery();
                Console.WriteLine("Tabel 'Reserveringen' is succesvol aangemaakt.");
            }

            connection.Close();
        }
    }

    public void AddReservation(int amountPeople, string firstName, string lastName, int phoneNumber, string email, int date)
    {
        string connectionString = @"Data Source=C:\Users\rensg\OneDrive\Documenten\GitHub\Reservatie-Applicatie\Reserverings-Applicatie\Mydatabase.db";
        
        using (SQLiteConnection connection = new SQLiteConnection(connectionString))
        {
            connection.Open();

            string sqlQuery = @"INSERT INTO Reserveringen (Amount_people, First_name, Last_name, Phonenumber, Email, Date) VALUES (@Amount_people, @First_name, @Last_name, @Phonenumber, @Email, @Date)";

            using (SQLiteCommand command = new SQLiteCommand(sqlQuery, connection))
            {
                command.Parameters.AddWithValue("@Amount_people", amountPeople);
                command.Parameters.AddWithValue("@First_name", firstName);
                command.Parameters.AddWithValue("@Last_name", lastName);
                command.Parameters.AddWithValue("@Phonenumber", phoneNumber);
                command.Parameters.AddWithValue("@Email", email);
                command.Parameters.AddWithValue("@Date", date);

                command.ExecuteNonQuery();
            }
        }
    }
}
