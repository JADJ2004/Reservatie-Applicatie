using Microsoft.Data.Sqlite;
using System;
using System.Net;
using System.Net.Mail;

public class ReservationSystem
{
    private SqliteConnection conn;
    private const string ConnectionString = @"Data Source=C:\Users\joey-\Documents\GitHub\Localtest\Mydatabase.db";

    public ReservationSystem()
    {
        conn = new SqliteConnection(ConnectionString);
        conn.Open();
    }

    public (int tableId, DateTime nextAvailableDate, string nextAvailableTimeSlot) ReserveTableForGroup(int numOfPeople, DateTime date, string timeSlot)
    {
        int reservedTableId = FindAvailableTable(numOfPeople, date, timeSlot);
        
        if (reservedTableId == -1)
        {
            var (nextAvailableDate, nextAvailableTimeSlot) = FindNextAvailableDateTime(numOfPeople, date, timeSlot);
            reservedTableId = FindAvailableTable(numOfPeople, nextAvailableDate, nextAvailableTimeSlot);
            return (reservedTableId, nextAvailableDate, nextAvailableTimeSlot);
        }

        return (reservedTableId, date, timeSlot);
    }

    private int FindAvailableTable(int numOfPeople, DateTime date, string timeSlot)
    {
        string formattedDate = date.ToString("yyyy-MM-dd");
        string sql = @"
            SELECT t.TableId
            FROM Tables t
            WHERE t.Capacity >= @NumOfPeople AND NOT EXISTS (
                SELECT 1 FROM Reservations r WHERE r.TableId = t.TableId AND r.Date = @Date AND r.TimeSlot = @TimeSlot
            )
            ORDER BY ABS(t.Capacity - @NumOfPeople) ASC, t.Capacity DESC
            LIMIT 1;";

        using (var cmd = new SqliteCommand(sql, conn))
        {
            cmd.Parameters.AddWithValue("@NumOfPeople", numOfPeople);
            cmd.Parameters.AddWithValue("@Date", formattedDate);
            cmd.Parameters.AddWithValue("@TimeSlot", timeSlot);

            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    return reader.GetInt32(0);
                }
            }
        }
        return -1;
    }

    private (DateTime, string) FindNextAvailableDateTime(int numOfPeople, DateTime startDate, string startTimeSlot)
    {
        DateTime nextDate = startDate;
        string[] timeSlots = { "18:00-19:59", "20:00-21:59", "22:00-23:59" };
        int currentIndex = Array.IndexOf(timeSlots, startTimeSlot);
        
        while (true)
        {
            string formattedDate = nextDate.ToString("yyyy-MM-dd");
            string sql = @"
                SELECT COUNT(*)
                FROM Tables t
                WHERE t.Capacity >= @NumOfPeople AND NOT EXISTS (
                    SELECT 1 FROM Reservations r WHERE r.TableId = t.TableId AND r.Date = @Date AND r.TimeSlot = @TimeSlot
                );";

            using (var cmd = new SqliteCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@NumOfPeople", numOfPeople);
                cmd.Parameters.AddWithValue("@Date", formattedDate);
                cmd.Parameters.AddWithValue("@TimeSlot", timeSlots[currentIndex]);

                int count = Convert.ToInt32(cmd.ExecuteScalar());
                if (count > 0)
                {
                    return (nextDate, timeSlots[currentIndex]);
                }

                currentIndex = (currentIndex + 1) % timeSlots.Length;
                if (currentIndex == 0)
                {
                    nextDate = nextDate.AddDays(1);
                }
            }
        }
    }

    public void SendEmail(string customerEmail, DateTime reservationDate, string timeSlot, string firstName, int numOfPeople, int ReservationId)
    {
        string smtpServer = "smtp-mail.outlook.com";
        int port = 587;
        string username = "yessrestaurant@outlook.com";
        string password = "Marcel12345";

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
