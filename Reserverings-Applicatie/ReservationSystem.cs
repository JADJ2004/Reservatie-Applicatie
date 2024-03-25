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

    public void ReserveTableForGroup(int numOfPeople, bool wantWindow)
    {
        List<int> assignedTables = new List<int>();
        int peopleToAccommodate = numOfPeople;

        string sql = "SELECT TableId, Capacity, WindowSeat FROM Tables WHERE IsAvailable = 1 AND WindowSeat = @WantWindow ORDER BY Capacity ASC";
        SQLiteCommand cmd = new SQLiteCommand(sql, conn);
        cmd.Parameters.AddWithValue("@WantWindow", wantWindow ? 1 : 0);

        using (var reader = cmd.ExecuteReader())
        {
            while (reader.Read() && peopleToAccommodate > 0)
            {
                int tableId = reader.GetInt32(0);
                int capacity = reader.GetInt32(1);
                bool isWindowSeat = reader.GetBoolean(2);

                string tableType = $"{capacity} personen";
                if (capacity >= peopleToAccommodate)
                {
                    assignedTables.Add(tableId);
                    UpdateTableAvailability(tableId, false);
                    Console.WriteLine($"Tafel {tableId} ({tableType}) gereserveerd. Aan het raam: {(isWindowSeat ? "Ja" : "Nee")}.");
                    peopleToAccommodate -= capacity;
                    break;
                }
            }
        }

        if (peopleToAccommodate > 0)
        {
            Console.WriteLine("Er zijn geen tafels meer aan het raam, u krijgt een tafel zonder raam.");
            ReserveTableForGroupWithoutWindowPreference(numOfPeople, assignedTables);
        }
    }

    private void ReserveTableForGroupWithoutWindowPreference(int numOfPeople, List<int> alreadyAssignedTables)
    {
        int peopleToAccommodate = numOfPeople;

        string sql = "SELECT TableId, Capacity, WindowSeat FROM Tables WHERE IsAvailable = 1 ORDER BY Capacity ASC";
        SQLiteCommand cmd = new SQLiteCommand(sql, conn);

        using (var reader = cmd.ExecuteReader())
        {
            while (reader.Read() && peopleToAccommodate > 0)
            {
                int tableId = reader.GetInt32(0);
                int capacity = reader.GetInt32(1);
                bool isWindowSeat = reader.GetBoolean(2);

                string tableType = $"{capacity} persoons";
                if (!alreadyAssignedTables.Contains(tableId) && capacity >= peopleToAccommodate)
                {
                    UpdateTableAvailability(tableId, false);
                    Console.WriteLine($"Tafel {tableId} ({tableType}) gereserveerd. Aan het raam: {(isWindowSeat ? "Ja" : "Nee")}.");
                    peopleToAccommodate -= capacity;
                    break;
                }
            }
        }

        if (peopleToAccommodate > 0)
        {
            Console.WriteLine("Helaas is er geen plek meer");
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
