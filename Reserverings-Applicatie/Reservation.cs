using System;
using Microsoft.Data.Sqlite;
using System.Collections.Generic;

public class Database
{
    private const string ConnectionString = @"Data Source=Z:\Documenten\PROJECTEN\01\Mydatabase.db";

    public List<DateTime> GetNextAvailableDates(DateTime startDate, int numOfDaysToCheck)
    {
        List<DateTime> availableDates = new List<DateTime>();

        using (var connection = new SqliteConnection(ConnectionString))
        {
            connection.Open();

            for (int i = 0; i < numOfDaysToCheck; i++)
            {
                DateTime currentDate = startDate.AddDays(i);

                if (!IsDateTaken(currentDate, connection))
                {
                    availableDates.Add(currentDate);

                    if (availableDates.Count == 5)
                    {
                        break;
                    }
                }
            }
        }

        return availableDates;
    }

    public bool IsDateTaken(DateTime date, SqliteConnection connection)
    {
        string formattedDate = date.ToString("dd-MM-yyyy");

        var sqlQuery = @"SELECT COUNT(*) FROM Reserveringen WHERE Date = @Date";
        using (var command = new SqliteCommand(sqlQuery, connection))
        {
            command.Parameters.AddWithValue("@Date", formattedDate);
            int count = Convert.ToInt32(command.ExecuteScalar());
            return count > 0;
        }
    }
}
