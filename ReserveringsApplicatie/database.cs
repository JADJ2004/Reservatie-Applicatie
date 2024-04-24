using System;
using Microsoft.Data.Sqlite;

public partial class Database
{
    private const string ConnectionString = @"Data Source=Z:\Documenten\PROJECTEN\01\Mydatabase.db";

    public void InitializeDatabase()
    {
        using (var connection = new SqliteConnection(ConnectionString))
        {
            connection.Open();
            var createTablesSql = @"
                CREATE TABLE IF NOT EXISTS Tables (
                    TableId INTEGER PRIMARY KEY AUTOINCREMENT,
                    Capacity INTEGER NOT NULL,
                    WindowSeat BOOLEAN NOT NULL
                );";
            var createReservationsSql = @"
                CREATE TABLE IF NOT EXISTS Reservations (
                    ReservationId INTEGER PRIMARY KEY AUTOINCREMENT,
                    TableId INTEGER,
                    NumOfPeople INTEGER NOT NULL,
                    First_name TEXT NOT NULL,
                    Infix TEXT,
                    Last_name TEXT NOT NULL,
                    Phonenumber TEXT NOT NULL,
                    Date TEXT NOT NULL,
                    Email TEXT NOT NULL,
                    FOREIGN KEY (TableId) REFERENCES Tables(TableId),
                    UNIQUE (TableId, Date)
                );";

            using (var command = new SqliteCommand(createTablesSql + createReservationsSql, connection))
            {
                command.ExecuteNonQuery();
            }
        }
    }

    public (bool success, DateTime suggestedDate) AddReservation(int numOfPeople, string firstName, string infix, string lastName, string phoneNumber, string email, DateTime date, int tableId, string remarks)
    {
        using (var connection = new SqliteConnection(ConnectionString))
        {
            connection.Open();
            var formattedDate = date.ToString("dd-MM-yyyy");
            var sqlQuery = @"
                INSERT INTO Reservations (TableId, NumOfPeople, First_name, Infix, Last_name, Phonenumber, Email, Date, Remarks)
                VALUES (@TableId, @NumOfPeople, @First_name, @Infix, @Last_name, @Phonenumber, @Email, @Date, @Remarks)";
            using (var command = new SqliteCommand(sqlQuery, connection))
            {
                command.Parameters.AddWithValue("@TableId", tableId);
                command.Parameters.AddWithValue("@NumOfPeople", numOfPeople);
                command.Parameters.AddWithValue("@First_name", firstName);
                command.Parameters.AddWithValue("@Infix", infix);
                command.Parameters.AddWithValue("@Last_name", lastName);
                command.Parameters.AddWithValue("@Phonenumber", phoneNumber);
                command.Parameters.AddWithValue("@Email", email);
                command.Parameters.AddWithValue("@Date", formattedDate);
                command.Parameters.AddWithValue("@Remarks", remarks);

                try
                {
                    command.ExecuteNonQuery();
                    return (true, date);
                }
                catch (SqliteException e)
                {
                    if (e.Message.ToLower().Contains("unique constraint failed"))
                    {
                        DateTime nextAvailableDate = FindNextAvailableDate(tableId, date, connection);
                        return (false, nextAvailableDate);
                    }
                    throw;
                }
            }
        }
    }

    private DateTime FindNextAvailableDate(int tableId, DateTime startDate, SqliteConnection connection)
    {
        DateTime nextDate = startDate.AddDays(1);
        string formattedDate;
        while (true)
        {
            formattedDate = nextDate.ToString("dd-MM-yyyy");
            string sql = @"
                SELECT COUNT(*)
                FROM Reservations
                WHERE TableId = @TableId AND Date = @Date;";

            using (var cmd = new SqliteCommand(sql, connection))
            {
                cmd.Parameters.AddWithValue("@TableId", tableId);
                cmd.Parameters.AddWithValue("@Date", formattedDate);

                int count = Convert.ToInt32(cmd.ExecuteScalar());
                if (count == 0)
                {
                    break;
                }

                nextDate = nextDate.AddDays(1);
            }
        }
        return nextDate;
    }
}
