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

    public void AddReservation(int amountPeople, string firstName, string lastName, int phoneNumber, string email, DateTime date)
    {
        string connectionString = @"Data Source=C:\Users\rensg\OneDrive\Documenten\GitHub\Reservatie-Applicatie\Reserverings-Applicatie\Mydatabase.db";
        
        using (SQLiteConnection connection = new SQLiteConnection(connectionString))
        {
            connection.Open();

            string formattedDate = date.ToString("yyyy-MM-dd");

            string sqlQuery = @"INSERT INTO Reserveringen (Amount_people, First_name, Last_name, Phonenumber, Email, Date) VALUES (@Amount_people, @First_name, @Last_name, @Phonenumber, @Email, @Date)";

            using (SQLiteCommand command = new SQLiteCommand(sqlQuery, connection))
            {
                command.Parameters.AddWithValue("@Amount_people", amountPeople);
                command.Parameters.AddWithValue("@First_name", firstName);
                command.Parameters.AddWithValue("@Last_name", lastName);
                command.Parameters.AddWithValue("@Phonenumber", phoneNumber);
                command.Parameters.AddWithValue("@Email", email);
                command.Parameters.AddWithValue("@Date", formattedDate);

                command.ExecuteNonQuery();
            }
        }

    }
    public void PrintReservations()
    {
        string connectionString = @"Data Source=C:\Users\rensg\OneDrive\Documenten\GitHub\Reservatie-Applicatie\Reserverings-Applicatie\Mydatabase.db";
        
        using (SQLiteConnection connection = new SQLiteConnection(connectionString))
        {
            connection.Open();

            string sqlQuery = @"SELECT * FROM Reserveringen";

            using (SQLiteCommand command = new SQLiteCommand(sqlQuery, connection))
            {
                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        // Retrieve values from the current row
                        int reservationId = reader.GetInt32(0);
                        int amountPeople = int.Parse(reader.GetString(1));
                        string firstName = reader.GetString(2);
                        string lastName = reader.GetString(3);
                        int phoneNumber = int.Parse(reader.GetString(4));
                        string date = reader.GetString(5);
                        string email = reader.GetString(6);

                        // Print the values
                        Console.WriteLine($"Reservation ID: {reservationId}, Amount of People: {amountPeople}, First Name: {firstName}, Last Name: {lastName}, Phone Number: {phoneNumber}, Date: {date}, Email: {email}");
                    }
                }
            }
        }
    }

}

