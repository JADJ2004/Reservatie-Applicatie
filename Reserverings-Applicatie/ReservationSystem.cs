using System;
using Microsoft.Data.Sqlite;

public class ReservationSystem
{
    private SqliteConnection conn;
    private const string ConnectionString = @"Data Source=C:\Users\joeyc\OneDrive\Documents\GitHub\Test123\Mydatabase.db";

    public ReservationSystem()
    {
        conn = new SqliteConnection(ConnectionString);
        conn.Open();
    }

    public (int tableId, DateTime nextAvailableDate) ReserveTableForGroup(int numOfPeople, bool wantWindow, DateTime date)
    {
        int reservedTableId = FindAvailableTable(numOfPeople, wantWindow, date);
        
        if (reservedTableId == -1)
        {
            DateTime nextAvailableDate = FindNextAvailableDate(numOfPeople, wantWindow, date);
            reservedTableId = FindAvailableTable(numOfPeople, wantWindow, nextAvailableDate);
            return (reservedTableId, nextAvailableDate);
        }

        return (reservedTableId, date);
    }

    private int FindAvailableTable(int numOfPeople, bool wantWindow, DateTime date)
    {
        string formattedDate = date.ToString("yyyy-MM-dd");
        string sql = @"
            SELECT t.TableId, t.Capacity
            FROM Tables t
            WHERE t.WindowSeat = @WantWindow AND NOT EXISTS (
                SELECT 1 FROM Reservations r WHERE r.TableId = t.TableId AND r.Date = @Date
            )
            ORDER BY ABS(t.Capacity - @NumOfPeople) ASC, t.Capacity DESC
            LIMIT 1;";

        using (var cmd = new SqliteCommand(sql, conn))
        {
            cmd.Parameters.AddWithValue("@WantWindow", wantWindow ? 1 : 0);
            cmd.Parameters.AddWithValue("@NumOfPeople", numOfPeople);
            cmd.Parameters.AddWithValue("@Date", formattedDate);

            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read() && reader.GetInt32(1) >= numOfPeople)
                {
                    return reader.GetInt32(0);
                }
            }
        }
        return -1;
    }

    private DateTime FindNextAvailableDate(int numOfPeople, bool wantWindow, DateTime startDate)
    {
        DateTime nextDate = startDate.AddDays(1);
        string formattedDate;
        while (true)
        {
            formattedDate = nextDate.ToString("yyyy-MM-dd");
            string sql = @"
                SELECT COUNT(*)
                FROM Tables t
                WHERE t.WindowSeat = @WantWindow AND t.Capacity >= @NumOfPeople AND NOT EXISTS (
                    SELECT 1 FROM Reservations r WHERE r.TableId = t.TableId AND r.Date = @Date
                );";
            
            using (var cmd = new SqliteCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@WantWindow", wantWindow ? 1 : 0);
                cmd.Parameters.AddWithValue("@NumOfPeople", numOfPeople);
                cmd.Parameters.AddWithValue("@Date", formattedDate);

                int count = Convert.ToInt32(cmd.ExecuteScalar());
                if (count > 0)
                {
                    break;
                }

                nextDate = nextDate.AddDays(1);
            }
        }
        return nextDate;
    }
}
