using System;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;
using ReservationApplication;

public partial class Database : IUseDatabase
{
    private const string ConnectionString = @"Data Source = .\Mydatabase.db";

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
                    NumOfPeople INTEGER NOT NULL,
                    First_name TEXT NOT NULL,
                    Infix TEXT,
                    Last_name TEXT NOT NULL,
                    Phonenumber TEXT NOT NULL,
                    Date TEXT NOT NULL,
                    TimeSlot TEXT NOT NULL,
                    Email TEXT NOT NULL,
                    Remarks TEXT
                );";
            var createReservationTablesSql = @"
                CREATE TABLE IF NOT EXISTS ReservationTables (
                    ReservationId INTEGER,
                    TableId INTEGER,
                    FOREIGN KEY (ReservationId) REFERENCES Reservations(ReservationId),
                    FOREIGN KEY (TableId) REFERENCES Tables(TableId),
                    PRIMARY KEY (ReservationId, TableId)
                );";
            var createMenuItemsSql = @"
                CREATE TABLE IF NOT EXISTS MenuItems (
                    MenuItemId INTEGER PRIMARY KEY AUTOINCREMENT,
                    Category TEXT NOT NULL,
                    Name TEXT NOT NULL,
                    Price REAL NOT NULL
                );";

            using (var command = new SqliteCommand(createTablesSql + createReservationsSql + createReservationTablesSql + createMenuItemsSql, connection))
            {
                command.ExecuteNonQuery();
            }
        }
    }


    public List<MenuItem> GetMenuItemsByCategory(string category)
    {
        List<MenuItem> menuItems = new List<MenuItem>();

        using (var connection = new SqliteConnection(ConnectionString))
        {
            connection.Open();
            string sqlQuery = "SELECT * FROM MenuItems WHERE Category = @Category";
            using (var command = new SqliteCommand(sqlQuery, connection))
            {
                command.Parameters.AddWithValue("@Category", category);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string name = reader.GetString(2);
                        double price = reader.GetDouble(3);
                        menuItems.Add(new MenuItem(category, name, price)); // Hier wordt de MenuItem correct aangemaakt met de category
                    }
                }
            }
        }

        return menuItems;
    }

    public (bool success, DateTime suggestedDate, string suggestedTimeSlot, int reservationId) AddReservation(int numOfPeople, string firstName, string infix, string lastName, string phoneNumber, string email, DateTime date, string timeSlot, int tableId, string remarks)
    {
        using (var connection = new SqliteConnection(ConnectionString))
        {
            connection.Open();
            var formattedDate = date.ToString("dd-MM-yyyy");

            // Generate a unique ReservationId
            Random random = new Random();
            int reservationId;
            bool isUnique;

            do
            {
                reservationId = random.Next(100000, 999999);
                var checkSql = "SELECT COUNT(*) FROM Reservations WHERE ReservationId = @ReservationId";
                using (var checkCmd = new SqliteCommand(checkSql, connection))
                {
                    checkCmd.Parameters.AddWithValue("@ReservationId", reservationId);
                    int count = Convert.ToInt32(checkCmd.ExecuteScalar());
                    isUnique = (count == 0);
                }
            } while (!isUnique);

            var sqlQuery = @"
                INSERT INTO Reservations (ReservationId, NumOfPeople, First_name, Infix, Last_name, Phonenumber, Email, Date, TimeSlot, Remarks)
                VALUES (@ReservationId, @NumOfPeople, @First_name, @Infix, @Last_name, @Phonenumber, @Email, @Date, @TimeSlot, @Remarks)";
            using (var command = new SqliteCommand(sqlQuery, connection))
            {
                command.Parameters.AddWithValue("@ReservationId", reservationId);
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
                    var insertTableQuery = @"
                        INSERT INTO ReservationTables (ReservationId, TableId)
                        VALUES (@ReservationId, @TableId)";
                    using (var tableCommand = new SqliteCommand(insertTableQuery, connection))
                    {
                        tableCommand.Parameters.AddWithValue("@ReservationId", reservationId);
                        tableCommand.Parameters.AddWithValue("@TableId", tableId);
                        tableCommand.ExecuteNonQuery();
                    }
                    return (true, date, timeSlot, reservationId);
                }
                catch (SqliteException e)
                {
                    if (e.Message.Contains("UNIQUE constraint failed"))
                    {
                        var (nextAvailableDate, nextAvailableTimeSlot) = FindNextAvailableDateTime(date, timeSlot, connection);
                        return (false, nextAvailableDate, nextAvailableTimeSlot, -1);
                    }
                    throw;
                }
            }
        }
    }

    public List<Table> GetAvailableTablesForLargeReservation(DateTime date, string timeSlot)
    {
        List<Table> availableTables = new List<Table>();
        using (var connection = new SqliteConnection(ConnectionString))
        {
            connection.Open();
            string sqlQuery = @"
                SELECT t.TableId, t.Capacity
                FROM Tables t
                WHERE t.TableId NOT IN (
                    SELECT rt.TableId
                    FROM ReservationTables rt
                    INNER JOIN Reservations r ON rt.ReservationId = r.ReservationId
                    WHERE r.Date = @Date AND r.TimeSlot = @TimeSlot
                );";
            using (var command = new SqliteCommand(sqlQuery, connection))
            {
                command.Parameters.AddWithValue("@Date", date.ToString("dd-MM-yyyy"));
                command.Parameters.AddWithValue("@TimeSlot", timeSlot);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        availableTables.Add(new Table
                        {
                            TableId = reader.GetInt32(0),
                            Capacity = reader.GetInt32(1)
                        });
                    }
                }
            }
        }
        return availableTables;
    }
    public (bool success, DateTime suggestedDate, string suggestedTimeSlot, int reservationId) AddLargeReservation(List<int> tableIds, int numOfPeople, string firstName, string infix, string lastName, string phoneNumber, string email, DateTime date, string timeSlot, string remarks)
    {
        using (var connection = new SqliteConnection(ConnectionString))
        {
            connection.Open();
            var formattedDate = date.ToString("dd-MM-yyyy");

            // Generate a unique ReservationId
            Random random = new Random();
            int reservationId;
            bool isUnique;

            do
            {
                reservationId = random.Next(100000, 999999);
                var checkSql = "SELECT COUNT(*) FROM Reservations WHERE ReservationId = @ReservationId";
                using (var checkCmd = new SqliteCommand(checkSql, connection))
                {
                    checkCmd.Parameters.AddWithValue("@ReservationId", reservationId);
                    int count = Convert.ToInt32(checkCmd.ExecuteScalar());
                    isUnique = (count == 0);
                }
            } while (!isUnique);

            var sqlQuery = @"
                INSERT INTO Reservations (ReservationId, NumOfPeople, First_name, Infix, Last_name, Phonenumber, Email, Date, TimeSlot, Remarks)
                VALUES (@ReservationId, @NumOfPeople, @First_name, @Infix, @Last_name, @Phonenumber, @Email, @Date, @TimeSlot, @Remarks)";
            using (var command = new SqliteCommand(sqlQuery, connection))
            {
                command.Parameters.AddWithValue("@ReservationId", reservationId);
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

                    foreach (var tableId in tableIds)
                    {
                        var insertTableQuery = @"
                            INSERT INTO ReservationTables (ReservationId, TableId)
                            VALUES (@ReservationId, @TableId)";
                        using (var tableCommand = new SqliteCommand(insertTableQuery, connection))
                        {
                            tableCommand.Parameters.AddWithValue("@ReservationId", reservationId);
                            tableCommand.Parameters.AddWithValue("@TableId", tableId);
                            tableCommand.ExecuteNonQuery();
                        }
                    }

                    return (true, date, timeSlot, reservationId);
                }
                catch (SqliteException e)
                {
                    if (e.Message.Contains("UNIQUE constraint failed"))
                    {
                        var (nextAvailableDate, nextAvailableTimeSlot) = FindNextAvailableDateTime(date, timeSlot, connection);
                        return (false, nextAvailableDate, nextAvailableTimeSlot, -1);
                    }
                    throw;
                }
            }
        }
    }
    private (DateTime, string) FindNextAvailableDateTime(DateTime startDate, string startTimeSlot, SqliteConnection connection)
    {
        DateTime nextDate = startDate;
        string nextTimeSlot = startTimeSlot;
        string[] timeSlots = { "18:00-19:59", "20:00-21:59", "22:00-23:59" };
        int currentIndex = Array.IndexOf(timeSlots, startTimeSlot);

        while (true)
        {
            string sql = @"
                SELECT COUNT(*)
                FROM Tables t
                WHERE NOT EXISTS (
                    SELECT 1 FROM Reservations r
                    INNER JOIN ReservationTables rt ON r.ReservationId = rt.ReservationId
                    WHERE rt.TableId = t.TableId AND r.Date = @Date AND r.TimeSlot = @TimeSlot
                );";

            using (var cmd = new SqliteCommand(sql, connection))
            {
                cmd.Parameters.AddWithValue("@Date", nextDate.ToString("dd-MM-yyyy"));
                cmd.Parameters.AddWithValue("@TimeSlot", nextTimeSlot);

                int count = Convert.ToInt32(cmd.ExecuteScalar());
                if (count > 0)
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
            string sqlQuery = @"
                SELECT r.*, GROUP_CONCAT(rt.TableId) AS TableIds
                FROM Reservations r
                LEFT JOIN ReservationTables rt ON r.ReservationId = rt.ReservationId
                WHERE r.Date = @Date
                GROUP BY r.ReservationId";
            using (var command = new SqliteCommand(sqlQuery, connection))
            {
                command.Parameters.AddWithValue("@Date", date);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var tableIdsString = reader["TableIds"].ToString();
                        var tableIds = tableIdsString.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                        var tableIdList = new List<int>();
                        foreach (var id in tableIds)
                        {
                            tableIdList.Add(int.Parse(id));
                        }

                        reservations.Add(new ReservationModel
                        {
                            ReservationId = reader.GetInt32(0),
                            TableIds = tableIdList,
                            NumOfPeople = reader.GetInt32(1),
                            FirstName = reader.GetString(2),
                            Infix = reader.IsDBNull(3) ? null : reader.GetString(3),
                            LastName = reader.GetString(4),
                            PhoneNumber = reader.GetString(5),
                            Email = reader.GetString(8),
                            Date = reader.GetString(6),
                            TimeSlot = reader.GetString(7),
                            Remarks = reader.IsDBNull(9) ? null : reader.GetString(9)
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
            string sqlQuery = @"
                SELECT r.*, GROUP_CONCAT(rt.TableId) AS TableIds
                FROM Reservations r
                LEFT JOIN ReservationTables rt ON r.ReservationId = rt.ReservationId
                GROUP BY r.ReservationId";
            using (var command = new SqliteCommand(sqlQuery, connection))
            {
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var tableIdsString = reader["TableIds"].ToString();
                        var tableIds = tableIdsString.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                        var tableIdList = new List<int>();
                        foreach (var id in tableIds)
                        {
                            tableIdList.Add(int.Parse(id));
                        }

                        reservations.Add(new ReservationModel
                        {
                            ReservationId = reader.GetInt32(0),
                            TableIds = tableIdList,
                            NumOfPeople = reader.GetInt32(1),
                            FirstName = reader.GetString(2),
                            Infix = reader.IsDBNull(3) ? null : reader.GetString(3),
                            LastName = reader.GetString(4),
                            PhoneNumber = reader.GetString(5),
                            Email = reader.GetString(8),
                            Date = reader.GetString(6),
                            TimeSlot = reader.GetString(7),
                            Remarks = reader.IsDBNull(9) ? null : reader.GetString(9)
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

            using (var transaction = connection.BeginTransaction())
            {
                try
                {
                    string deleteReservationTablesQuery = "DELETE FROM ReservationTables WHERE ReservationId = @ReservationId";
                    using (var command = new SqliteCommand(deleteReservationTablesQuery, connection, transaction))
                    {
                        command.Parameters.AddWithValue("@ReservationId", reservationId);
                        command.ExecuteNonQuery();
                    }

                    string deleteReservationQuery = "DELETE FROM Reservations WHERE ReservationId = @ReservationId";
                    using (var command = new SqliteCommand(deleteReservationQuery, connection, transaction))
                    {
                        command.Parameters.AddWithValue("@ReservationId", reservationId);
                        command.ExecuteNonQuery();
                    }

                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }
    }

    public List<ReservationModel> GetReservationsByDateAndTimeSlot(string date, string timeSlot)
    {
        List<ReservationModel> reservations = new List<ReservationModel>();

        using (var connection = new SqliteConnection(ConnectionString))
        {
            connection.Open();
            string sqlQuery = @"
                SELECT r.*, GROUP_CONCAT(rt.TableId) AS TableIds
                FROM Reservations r
                LEFT JOIN ReservationTables rt ON r.ReservationId = rt.ReservationId
                WHERE r.Date = @Date AND r.TimeSlot = @TimeSlot
                GROUP BY r.ReservationId";
            using (var command = new SqliteCommand(sqlQuery, connection))
            {
                command.Parameters.AddWithValue("@Date", date);
                command.Parameters.AddWithValue("@TimeSlot", timeSlot);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var tableIdsString = reader["TableIds"].ToString();
                        var tableIds = tableIdsString.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                        var tableIdList = new List<int>();
                        foreach (var id in tableIds)
                        {
                            tableIdList.Add(int.Parse(id));
                        }

                        reservations.Add(new ReservationModel
                        {
                            ReservationId = reader.GetInt32(0),
                            TableIds = tableIdList,
                            NumOfPeople = reader.GetInt32(1),
                            FirstName = reader.GetString(2),
                            Infix = reader.IsDBNull(3) ? null : reader.GetString(3),
                            LastName = reader.GetString(4),
                            PhoneNumber = reader.GetString(5),
                            Email = reader.GetString(8),
                            Date = reader.GetString(6),
                            TimeSlot = reader.GetString(7),
                            Remarks = reader.IsDBNull(9) ? null : reader.GetString(9)
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
            string sqlQuery = @"
                SELECT r.*, GROUP_CONCAT(rt.TableId) AS TableIds
                FROM Reservations r
                LEFT JOIN ReservationTables rt ON r.ReservationId = rt.ReservationId
                WHERE r.ReservationId = @ReservationId
                GROUP BY r.ReservationId";
            using (var command = new SqliteCommand(sqlQuery, connection))
            {
                command.Parameters.AddWithValue("@ReservationId", reservationId);
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        var tableIdsString = reader["TableIds"].ToString();
                        var tableIds = tableIdsString.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                        var tableIdList = new List<int>();
                        foreach (var id in tableIds)
                        {
                            tableIdList.Add(int.Parse(id));
                        }

                        return new ReservationModel
                        {
                            ReservationId = reader.GetInt32(0),
                            TableIds = tableIdList,
                            NumOfPeople = reader.GetInt32(1),
                            FirstName = reader.GetString(2),
                            Infix = reader.IsDBNull(3) ? null : reader.GetString(3),
                            LastName = reader.GetString(4),
                            PhoneNumber = reader.GetString(5),
                            Email = reader.GetString(8),
                            Date = reader.GetString(6),
                            TimeSlot = reader.GetString(7),
                            Remarks = reader.IsDBNull(9) ? null : reader.GetString(9)
                        };
                    }
                }
            }
        }

        return null;
    }
}
