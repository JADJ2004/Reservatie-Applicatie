using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using ReservationApplication;

public class ReservationSystem : IUseDatabase
{
    private SqliteConnection conn;
    private const string ConnectionString = @"Data Source=.\Mydatabase.db";

    public ReservationSystem()
    {
        conn = new SqliteConnection(ConnectionString);
        conn.Open();
    }

    public (int tableId, DateTime nextAvailableDate, string nextAvailableTimeSlot) ReserveTableForGroup(int numberOfPeople, DateTime reservationDate, string timeSlot)
    {
        using (var connection = new SqliteConnection(@"Data Source=.\Mydatabase.db"))
        {
            connection.Open();
            var availableTables = GetAvailableTables(reservationDate, timeSlot, connection);

            foreach (var table in availableTables)
            {
                if (table.Capacity >= numberOfPeople)
                {
                    return (table.TableId, DateTime.MinValue, string.Empty);
                }
            }

            var (nextAvailableDate, nextAvailableTimeSlot) = FindNextAvailableDateTime(reservationDate, timeSlot, connection);
            return (-1, nextAvailableDate, nextAvailableTimeSlot);
        }
    }
    private List<Table> GetAvailableTables(DateTime date, string timeSlot, SqliteConnection connection)
    {
        List<Table> availableTables = new List<Table>();

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
        return availableTables;
    }

    private List<int> FindAvailableTables(int numOfPeople, DateTime date, string timeSlot)
    {
        List<int> tableIds = new List<int>();
        string formattedDate = date.ToString("dd-MM-yyyy");
        string sql = @"
            SELECT t.TableId, t.Capacity
            FROM Tables t
            WHERE NOT EXISTS (
                SELECT 1 FROM Reservations r WHERE r.TableId = t.TableId AND r.Date = @Date AND r.TimeSlot = @TimeSlot
            )
            ORDER BY t.Capacity ASC, t.TableId ASC;";

        using (var cmd = new SqliteCommand(sql, conn))
        {
            cmd.Parameters.AddWithValue("@Date", formattedDate);
            cmd.Parameters.AddWithValue("@TimeSlot", timeSlot);

            using (var reader = cmd.ExecuteReader())
            {
                int remainingPeople = numOfPeople;
                while (reader.Read() && remainingPeople > 0)
                {
                    int tableId = reader.GetInt32(0);
                    int capacity = reader.GetInt32(1);

                    tableIds.Add(tableId);
                    remainingPeople -= capacity;
                }
            }
        }
        return tableIds;
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

    public void SendEmail(string customerEmail, DateTime reservationDate, string timeSlot, string firstName, int numOfPeople, int ReservationId)
    {
        string smtpServer = "smtp-mail.outlook.com";
        int port = 587;
        string username = "yessrestaurant@outlook.com";
        string password = "Marcel123456";

        string from = "yessrestaurant@outlook.com";
        string to = customerEmail;

        string subject = "Bevestiging van reservering bij YES! Restaurant";
        string body = $"Beste {firstName},\n\n" +
                      $"Bedankt voor uw reservering bij YES! Restaurant.\n" +
                      $"Hier zijn de details van uw reservering:\n" +
                      $"Reserverings nummer: {ReservationId}\n" +
                      $"Datum: {reservationDate.ToShortDateString()}\n" +
                      $"Tijdslot: {timeSlot}\n" +
                      $"Aantal personen: {numOfPeople}\n\n" +
                      $"We kijken ernaar uit om u te verwelkomen!\n\n" +
                      $"Met vriendelijke groet,\n" +
                      $"YES! Restaurant";

        SmtpClient client = new SmtpClient(smtpServer, port)
        {
            Credentials = new NetworkCredential(username, password),
            EnableSsl = true
        };

        MailMessage message = new MailMessage(from, to, subject, body);

        try
        {
            Console.ForegroundColor = ConsoleColor.Green;
            client.Send(message);
            Console.WriteLine("Bevestigingsmail is succesvol verstuurd.");
            Console.ResetColor();
        
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Er is een fout opgetreden bij het versturen van de e-mail: " + ex.Message);
            Console.ResetColor();
        }
    }
}
