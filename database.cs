using System;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;
using ReservationApplication;

public partial class Database
{
    private const string ConnectionString = @"Data Source=C:\Users\rensg\OneDrive\Documenten\GitHub\LOCAAL\lokaal\mm\Mydatabase.db";

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
                    ReservationId INTEGER PRIMARY KEY,
                    TableId INTEGER,
                    NumOfPeople INTEGER NOT NULL,
                    First_name TEXT NOT NULL,
                    Infix TEXT,
                    Last_name TEXT NOT NULL,
                    Phonenumber TEXT NOT NULL,
                    Date TEXT NOT NULL,
                    TimeSlot TEXT NOT NULL,
                    Email TEXT NOT NULL,
                    Remarks TEXT,
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
            
            // Genereer een willekeurige ReservationId
            Random random = new Random();
            int reservationId;
            bool isUnique;
            
            do
            {
                reservationId = random.Next(100000, 999999); // Verander het bereik als dat nodig is
                var checkSql = "SELECT COUNT(*) FROM Reservations WHERE ReservationId = @ReservationId";
                using (var checkCmd = new SqliteCommand(checkSql, connection))
                {
                    checkCmd.Parameters.AddWithValue("@ReservationId", reservationId);
                    int count = Convert.ToInt32(checkCmd.ExecuteScalar());
                    isUnique = (count == 0);
                }
            } while (!isUnique);
            
            var sqlQuery = @"
                INSERT INTO Reservations (ReservationId, TableId, NumOfPeople, First_name, Infix, Last_name, Phonenumber, Email, Date, TimeSlot, Remarks)
                VALUES (@ReservationId, @TableId, @NumOfPeople, @First_name, @Infix, @Last_name, @Phonenumber, @Email, @Date, @TimeSlot, @Remarks)";
            using (var command = new SqliteCommand(sqlQuery, connection))
            {
                command.Parameters.AddWithValue("@ReservationId", reservationId);
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

    public void DeleteReservation(int reservationId)
    {
        using (var connection = new SqliteConnection(ConnectionString))
        {
            connection.Open();
            string sqlQuery = "DELETE FROM Reservations WHERE ReservationId = @ReservationId";
            using (var command = new SqliteCommand(sqlQuery, connection))
            {
                command.Parameters.AddWithValue("@ReservationId", reservationId);
                command.ExecuteNonQuery();
            }
        }
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
