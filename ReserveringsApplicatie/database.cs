using System;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;
using ReservationApplication;

public partial class Database
{
    private const string ConnectionString = @"Data Source=C:\Users\joeyc\OneDrive\Documents\GitHub\Reservatie-Applicatie\Localtest\Mydatabase.db";

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
                    Remarks TEXT,
                    FOREIGN KEY (TableId) REFERENCES Tables(TableId),
                    UNIQUE (TableId, Date)
                );";

            using (var command = new SqliteCommand(createTablesSql + createReservationsSql, connection))
            {
                command.ExecuteNonQuery();
            }
        }
    }

<<<<<<< HEAD
<<<<<<< HEAD
    public (bool success, DateTime suggestedDate) AddReservation(int numOfPeople, string firstName, string infix, string lastName, string phoneNumber, string email, DateTime date, int tableId, string remarks)
=======
    public (bool success, DateTime suggestedDate, string suggestedTimeSlot, int reservationId) AddReservation(int numOfPeople, string firstName, string infix, string lastName, string phoneNumber, string email, DateTime date, string timeSlot, int tableId, string remarks)
>>>>>>> 0386f2ee0b7f151f9574bc66a3b1efb4c4edc38d
=======
    public (bool success, DateTime suggestedDate, string suggestedTimeSlot, int reservationId) AddReservation(int numOfPeople, string firstName, string infix, string lastName, string phoneNumber, string email, DateTime date, string timeSlot, int tableId, string remarks)
>>>>>>> 0386f2ee0b7f151f9574bc66a3b1efb4c4edc38d
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
<<<<<<< HEAD
<<<<<<< HEAD
                    return (true, date);
=======
                    command.CommandText = "SELECT last_insert_rowid()";
                    int reservationId = Convert.ToInt32(command.ExecuteScalar());
                    return (true, date, timeSlot, reservationId);
>>>>>>> 0386f2ee0b7f151f9574bc66a3b1efb4c4edc38d
=======
                    command.CommandText = "SELECT last_insert_rowid()";
                    int reservationId = Convert.ToInt32(command.ExecuteScalar());
                    return (true, date, timeSlot, reservationId);
>>>>>>> 0386f2ee0b7f151f9574bc66a3b1efb4c4edc38d
                }
                catch (SqliteException e)
                {
                    if (e.Message.ToLower().Contains("unique constraint failed"))
                    {
<<<<<<< HEAD
                        DateTime nextAvailableDate = FindNextAvailableDate(tableId, date, connection);
                        return (false, nextAvailableDate);
=======
                        var (nextAvailableDate, nextAvailableTimeSlot) = FindNextAvailableDateTime(tableId, date, timeSlot, connection);
                        return (false, nextAvailableDate, nextAvailableTimeSlot, -1);
<<<<<<< HEAD
>>>>>>> 0386f2ee0b7f151f9574bc66a3b1efb4c4edc38d
=======
>>>>>>> 0386f2ee0b7f151f9574bc66a3b1efb4c4edc38d
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

    public List<string> GetCurrentWeekDates()
    {
    List<string> weekDates = new List<string>();
    DateTime startOfWeek = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek + (int)DayOfWeek.Monday);
    
    for (int i = 0; i < 7; i++)
    {
        weekDates.Add(startOfWeek.AddDays(i).ToString("dd-MM-yyyy"));
    }

    return weekDates;
    }

    public Dictionary<string, (int occupiedTables, int totalPeople)> GetReservationsDetailsForWeek(List<string> dates)
    {
        Dictionary<string, (int occupiedTables, int totalPeople)> reservationsDetails = new Dictionary<string, (int occupiedTables, int totalPeople)>();

        using (var connection = new SqliteConnection(ConnectionString))
        {
            connection.Open();
            foreach (var date in dates)
            {
                string sqlQuery = "SELECT COUNT(*), SUM(NumOfPeople) FROM Reservations WHERE Date = @Date";
                using (var command = new SqliteCommand(sqlQuery, connection))
                {
                    command.Parameters.AddWithValue("@Date", date);
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            int occupiedTables = reader.GetInt32(0);
                            int totalPeople = reader.IsDBNull(1) ? 0 : reader.GetInt32(1);
                            reservationsDetails[date] = (occupiedTables, totalPeople);
                        }
                    }
                }
            }
        }

    return reservationsDetails;
}

    public List<ReservationModel> GetReservationsByDate(string date)
    {
        List<ReservationModel> reservations = new List<ReservationModel>();

        using (var connection = new SqliteConnection(ConnectionString))
        {
            connection.Open();
            string sqlQuery = "SELECT * FROM Reservations WHERE Date = @Date";
            using (var command = new SqliteCommand(sqlQuery, connection))
            {
                command.Parameters.AddWithValue("@Date", date);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        reservations.Add(new ReservationModel
                        {
                            ReservationId = reader.GetInt32(0),
                            TableId = reader.GetInt32(1),
                            NumOfPeople = reader.GetInt32(2),
                            FirstName = reader.GetString(3),
                            Infix = reader.IsDBNull(4) ? null : reader.GetString(4),
                            LastName = reader.GetString(5),
                            PhoneNumber = reader.GetString(6),
                            Email = reader.GetString(9),
                            Date = reader.GetString(7),
                            TimeSlot = reader.GetString(8),
                            Remarks = reader.IsDBNull(10) ? null : reader.GetString(10)
                        });
                    }
                }
            }
        }

        return reservations;
    }


    public List<ReservationModel> GetAllReservations()
    {
        List<ReservationModel> reservations = new List<ReservationModel>();

        using (var connection = new SqliteConnection(ConnectionString))
        {
            connection.Open();
            string sqlQuery = "SELECT * FROM Reservations";
            using (var command = new SqliteCommand(sqlQuery, connection))
            {
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        reservations.Add(new ReservationModel
                        {
                            ReservationId = reader.GetInt32(0),      
                            TableId = reader.GetInt32(1),             
                            NumOfPeople = reader.GetInt32(2),      
                            FirstName = reader.GetString(3),        
                            Infix = reader.IsDBNull(4) ? null : reader.GetString(4), 
                            LastName = reader.GetString(5),         
                            PhoneNumber = reader.GetString(6),        
                            Email = reader.GetString(9),             
                            Date = reader.GetString(7),            // 6-> 9 -> 7 -> 8 -> 10 genieters
                            TimeSlot = reader.GetString(8),            
                            Remarks = reader.IsDBNull(10) ? null : reader.GetString(10)
                        });
                    }
                }
            }
        }

        return reservations;
    }
    public ReservationModel GetReservationById(int reservationId)
    {
        using (var connection = new SqliteConnection(ConnectionString))
        {
            connection.Open();
            string sqlQuery = "SELECT * FROM Reservations WHERE ReservationId = @ReservationId";
            using (var command = new SqliteCommand(sqlQuery, connection))
            {
                command.Parameters.AddWithValue("@ReservationId", reservationId);
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new ReservationModel
                        {
                            ReservationId = reader.GetInt32(0),
                            TableId = reader.GetInt32(1),
                            NumOfPeople = reader.GetInt32(2),
                            FirstName = reader.GetString(3),
                            Infix = reader.IsDBNull(4) ? null : reader.GetString(4),
                            LastName = reader.GetString(5),
                            PhoneNumber = reader.GetString(6),
                            Email = reader.GetString(9),
                            Date = reader.GetString(7),
                            TimeSlot = reader.GetString(8),
                            Remarks = reader.IsDBNull(10) ? null : reader.GetString(10)
                        };
                    }
                }
            }
        }

        return null;
    }

}
