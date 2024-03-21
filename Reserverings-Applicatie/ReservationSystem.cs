using System;
using System.Collections.Generic;
using System.Data.SQLite;

public class ReservationSystem
{
    private SQLiteConnection conn;
    private string connectionString = "Data Source=Mydatabase.db;Version=3;";

    public ReservationSystem()
    {
        conn = new SQLiteConnection(connectionString);
        conn.Open();
    }

    public void ReserveTableForGroup(int numberOfPeople, bool wantWindow)
    {
        List<int> assignedTables = new List<int>();
        int peopleToAccommodate = numberOfPeople;

        string sql = "SELECT TableId, Capacity FROM Tables WHERE IsAvailable = 1 AND WindowSeat = @WantWindow ORDER BY Capacity ASC";
        SQLiteCommand cmd = new SQLiteCommand(sql, conn);
        cmd.Parameters.AddWithValue("@WantWindow", wantWindow ? 1 : 0);

        using (var reader = cmd.ExecuteReader())
        {
            while (reader.Read() && peopleToAccommodate > 0)
            {
                int tableId = reader.GetInt32(0);
                int capacity = reader.GetInt32(1);

                if (capacity >= peopleToAccommodate)
                {
                    assignedTables.Add(tableId);
                    UpdateTableAvailability(tableId, false);
                    Console.WriteLine($"Table {tableId} reserved.");
                    peopleToAccommodate -= capacity;
                    break;
                }
            }
        }

        if (peopleToAccommodate > 0)
        {
            Console.WriteLine("Not enough capacity to accommodate everyone based on your preferences. Trying without window preference.");
            ReserveTableForGroupWithoutWindowPreference(numberOfPeople, assignedTables);
        }
    }

    private void ReserveTableForGroupWithoutWindowPreference(int numberOfPeople, List<int> alreadyAssignedTables)
    {
        int peopleToAccommodate = numberOfPeople;

        string sql = "SELECT TableId, Capacity FROM Tables WHERE IsAvailable = 1 ORDER BY Capacity ASC";
        SQLiteCommand cmd = new SQLiteCommand(sql, conn);

        using (var reader = cmd.ExecuteReader())
        {
            while (reader.Read() && peopleToAccommodate > 0)
            {
                int tableId = reader.GetInt32(0);
                int capacity = reader.GetInt32(1);

                if (!alreadyAssignedTables.Contains(tableId) && capacity >= peopleToAccommodate)
                {
                    UpdateTableAvailability(tableId, false);
                    Console.WriteLine($"Table {tableId} reserved.");
                    peopleToAccommodate -= capacity;
                    break;
                }
            }
        }

        if (peopleToAccommodate > 0)
        {
            Console.WriteLine("Unfortunately, we cannot accommodate your entire group with the current table setup.");
        }
    }

    private void UpdateTableAvailability(int tableId, bool isAvailable)
    {
        string sqlUpdateTable = "UPDATE Tables SET IsAvailable = @IsAvailable WHERE TableId = @TableId";
        SQLiteCommand cmdUpdateTable = new SQLiteCommand(sqlUpdateTable, conn);
        cmdUpdateTable.Parameters.AddWithValue("@IsAvailable", isAvailable ? 1 : 0);
        cmdUpdateTable.Parameters.AddWithValue("@TableId", tableId);
        cmdUpdateTable.ExecuteNonQuery();
    }
}
