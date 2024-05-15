using System;
using Microsoft.Data.Sqlite;

public partial class Database
{
    private const string ConnectionString = @"Data Source=C:\Users\joey-\Documents\GitHub\LocalTest\Mydatabase.db";

    public void InitializeDatabase()
    {
        using (var connection = new SqliteConnection(ConnectionString))
        {
            connection.Open();
            var createTablesSql = @"
                CREATE TABLE IF NOT EXISTS Tables (
                    TableId INTEGER PRIMARY KEY AUTOINCREMENT,
                    Capacity INTEGER NOT NULL
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
                    TimeSlot TEXT NOT NULL,
                    Email TEXT NOT NULL,
                    FOREIGN KEY (TableId) REFERENCES Tables(TableId),
                    UNIQUE (TableId, Date, TimeSlot)
                );";

            using (var command = new SqliteCommand(createTablesSql + createReservationsSql, connection))
            {
                command.ExecuteNonQuery();
            }
        }
    }

    public (bool success, DateTime suggestedDate, string suggestedTimeSlot, int reservationId) AddReservation(int numOfPeople, string firstName, string infix, string lastName, string phoneNumber, string email, DateTime date, string timeSlot, int tableId, string remarks)
    {
        using (var connection = new SqliteConnection(ConnectionString))
        {
            connection.Open();
            var formattedDate = date.ToString("dd-MM-yyyy");
            var sqlQuery = @"
                INSERT INTO Reservations (TableId, NumOfPeople, First_name, Infix, Last_name, Phonenumber, Email, Date, TimeSlot, Remarks)
                VALUES (@TableId, @NumOfPeople, @First_name, @Infix, @Last_name, @Phonenumber, @Email, @Date, @TimeSlot, @Remarks)";
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
                command.Parameters.AddWithValue("@TimeSlot", timeSlot);
                command.Parameters.AddWithValue("@Remarks", remarks);

                try
                {
                    command.ExecuteNonQuery();
                    command.CommandText = "SELECT last_insert_rowid()";
                    int reservationId = Convert.ToInt32(command.ExecuteScalar());
                    return (true, date, timeSlot, reservationId);
                }
                catch (SqliteException e)
                {
                    if (e.Message.Contains("UNIQUE constraint failed"))
                    {
                        var (nextAvailableDate, nextAvailableTimeSlot) = FindNextAvailableDateTime(tableId, date, timeSlot, connection);
                        return (false, nextAvailableDate, nextAvailableTimeSlot, -1);
                    }
                    throw;
                }
            }
        }
    }

    private (DateTime, string) FindNextAvailableDateTime(int tableId, DateTime startDate, string startTimeSlot, SqliteConnection connection)
    {
        DateTime nextDate = startDate;
        string nextTimeSlot = startTimeSlot;
        string[] timeSlots = { "18:00-19:59", "20:00-21:59", "22:00-23:59" };
        int currentIndex = Array.IndexOf(timeSlots, startTimeSlot);

        while (true)
        {
            string sql = @"
                SELECT COUNT(*)
                FROM Reservations
                WHERE TableId = @TableId AND Date = @Date AND TimeSlot = @TimeSlot;";

            using (var cmd = new SqliteCommand(sql, connection))
            {
                cmd.Parameters.AddWithValue("@TableId", tableId);
                cmd.Parameters.AddWithValue("@Date", nextDate.ToString("dd-MM-yyyy"));
                cmd.Parameters.AddWithValue("@TimeSlot", nextTimeSlot);

                int count = Convert.ToInt32(cmd.ExecuteScalar());
                if (count == 0)
                {
                    return (nextDate, nextTimeSlot);
                }

                currentIndex = (currentIndex + 1) % timeSlots.Length;
                if (currentIndex == 0)
                {
                    nextDate = nextDate.AddDays(1);
                }
                nextTimeSlot = timeSlots[currentIndex];
            }
        }
    }
}
