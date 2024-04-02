using System;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;

public class ReservationSystem
{
    private SqliteConnection conn;
    private string connectionString = @"Data Source=Z:\Documenten\PROJECTEN\01\Mydatabase.db";

    public ReservationSystem()
    {
        conn = new SqliteConnection(connectionString);
        conn.Open();
    }

    public void ReserveTableForGroup(int numOfPeople, bool wantWindow)
    {
        List<int> assignedTables = new List<int>();
        int peopleToAccommodate = numOfPeople;

        string sql = "SELECT TableId, Capacity, WindowSeat FROM Tables WHERE IsAvailable = 1 AND WindowSeat = @WantWindow ORDER BY ABS(Capacity - @NumOfPeople) ASC, Capacity DESC";
        using (var cmd = new SqliteCommand(sql, conn))
        {
            cmd.Parameters.AddWithValue("@WantWindow", wantWindow ? 1 : 0);
            cmd.Parameters.AddWithValue("@NumOfPeople", numOfPeople);

            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read() && peopleToAccommodate > 0)
                {
                    int tableId = reader.GetInt32(0);
                    int capacity = reader.GetInt32(1);
                    bool isWindowSeat = reader.GetBoolean(2);

                    if (!assignedTables.Contains(tableId))
                    {
                        assignedTables.Add(tableId);
                        UpdateTableAvailability(tableId, false);
                        Console.WriteLine($"Tafel voor {capacity} personen gereserveerd. \nAan het raam: {(isWindowSeat ? "Ja" : "Nee")}.");
                        peopleToAccommodate -= capacity;
                        if (peopleToAccommodate <= 0) break;
                    }
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

        string sql = "SELECT TableId, Capacity FROM Tables WHERE IsAvailable = 1 ORDER BY ABS(Capacity - @NumOfPeople) ASC, Capacity DESC";
        using (var cmd = new SqliteCommand(sql, conn))
        {
            cmd.Parameters.AddWithValue("@NumOfPeople", numOfPeople);

            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read() && peopleToAccommodate > 0)
                {
                    int tableId = reader.GetInt32(0);
                    int capacity = reader.GetInt32(1);

                    if (!alreadyAssignedTables.Contains(tableId))
                    {
                        UpdateTableAvailability(tableId, false);
                        Console.WriteLine($"Tafel {tableId} ({capacity} persoons) gereserveerd.");
                        peopleToAccommodate -= capacity;
                        alreadyAssignedTables.Add(tableId);
                        if (peopleToAccommodate <= 0) break;
                    }
                }
            }
        }

        if (peopleToAccommodate > 0)
        {
            Console.WriteLine("Helaas is er geen plek meer beschikbaar.");
        }
    }

    private void UpdateTableAvailability(int tableId, bool isAvailable)
    {
        string sqlUpdateTable = "UPDATE Tables SET IsAvailable = @IsAvailable WHERE TableId = @TableId";
        using (var cmdUpdateTable = new SqliteCommand(sqlUpdateTable, conn))
        {
            cmdUpdateTable.Parameters.AddWithValue("@IsAvailable", isAvailable ? 1 : 0);
            cmdUpdateTable.Parameters.AddWithValue("@TableId", tableId);
            cmdUpdateTable.ExecuteNonQuery();
        }
    }
}


public void CancelReservation(int reservationId)
    {
        string sqlCancelReservation = "UPDATE Tables SET IsAvailable = 1 WHERE ReservationId = @ReservationId";
        using (var cmdCancelReservation = new SqliteCommand(sqlCancelReservation, conn))
        {
            cmdCancelReservation.Parameters.AddWithValue("@ReservationId", reservationId);
            int rowsAffected = cmdCancelReservation.ExecuteNonQuery();
            if (rowsAffected > 0)
            {
                Console.WriteLine("Reservation canceled successfully.");
            }
            else
            {
                Console.WriteLine("Failed to cancel reservation. Please check the reservation ID.");
            }
        }
    }