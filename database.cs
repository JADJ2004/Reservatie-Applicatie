using System;
using Microsoft.Data.Sqlite;

public partial class Database
{
    private const string ConnectionString = @"Data Source=Z:\Documenten\PROJECTEN\01\Mydatabase.db";

    public void Database_con()
    {
        using (var connection = new SqliteConnection(ConnectionString))
        {
            connection.Open();
            var createTableQuery = @"
                DROP TABLE IF EXISTS Reserveringen;
                CREATE TABLE IF NOT EXISTS Reserveringen (
                    ReservationId INTEGER PRIMARY KEY AUTOINCREMENT,
                    numOfPeople TEXT,
                    First_name TEXT,
                    Infix TEXT,
                    Last_name TEXT,
                    Phonenumber TEXT,
                    Date TEXT,
                    Email TEXT
                )";
            using (var command = new SqliteCommand(createTableQuery, connection))
            {
                command.ExecuteNonQuery();
            }
        }
    }


    public void CreateTableTable()
    {
        using (var connection = new SqliteConnection(ConnectionString))
        {
            connection.Open();
            var createTableQuery = @"
                CREATE TABLE IF NOT EXISTS Tables (
                    TableId INTEGER PRIMARY KEY,
                    Capacity INTEGER NOT NULL,
                    IsAvailable INTEGER NOT NULL,
                    WindowSeat INTEGER NOT NULL
                )";
            using (var command = new SqliteCommand(createTableQuery, connection))
            {
                command.ExecuteNonQuery();
            }
        }
    }

    public void AddReservation(int numOfPeople, string firstName, string infix, string lastName, int phoneNumber, string email, DateTime date)
    {
        using (var connection = new SqliteConnection(ConnectionString))
        {
            connection.Open();
            var formattedDate = date.ToString("dd-MM-yyyy");
            var sqlQuery = @"
                INSERT INTO Reserveringen (numOfPeople, First_name, Infix, Last_name, Phonenumber, Email, Date)
                VALUES (@numOfPeople, @First_name, @Infix, @Last_name, @Phonenumber, @Email, @Date)";
            using (var command = new SqliteCommand(sqlQuery, connection))
            {
                command.Parameters.AddWithValue("@numOfPeople", numOfPeople);
                command.Parameters.AddWithValue("@First_name", firstName);
                command.Parameters.AddWithValue("@Infix", infix);
                command.Parameters.AddWithValue("@Last_name", lastName);
                command.Parameters.AddWithValue("@Phonenumber", phoneNumber.ToString());
                command.Parameters.AddWithValue("@Email", email);
                command.Parameters.AddWithValue("@Date", formattedDate);

                command.ExecuteNonQuery();
            }
        }
    }

    public void PrintReservations()
    {
        using (var connection = new SqliteConnection(ConnectionString))
        {
            connection.Open();
            var sqlQuery = @"SELECT * FROM Reserveringen";
            using (var command = new SqliteCommand(sqlQuery, connection))
            {
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var reservationId = reader.GetInt32(0);
                        var numOfPeople = reader.GetInt32(1);
                        var firstName = reader.GetString(2);
                        var infix = reader.GetString(3);
                        var lastName = reader.GetString(4);
                        var phoneNumber = reader.GetString(5);
                        var date = reader.GetString(6);
                        var email = reader.GetString(7);

                        Console.WriteLine($"Reservation ID: {reservationId}, Amount of People: {numOfPeople}, First Name: {firstName}, Infix: {infix}, Last Name: {lastName}, Phone Number: {phoneNumber}, Date: {date}, Email: {email}");
                    }
                }
            }
        }
    }
}