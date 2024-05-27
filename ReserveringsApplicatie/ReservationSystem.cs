using System;
using Microsoft.Data.Sqlite;

public class ReservationSystem
{
    private SqliteConnection conn;
<<<<<<< HEAD
<<<<<<< HEAD
    private const string ConnectionString = @"Data Source=C:\Users\joeyc\OneDrive\Documents\GitHub\Reservatie-Applicatie\Localtest\Mydatabase.db";
=======
    private const string ConnectionString = @"Data Source=C:\Users\joey-\Documents\GitHub\LocalTest\Mydatabase.db";
>>>>>>> 0386f2ee0b7f151f9574bc66a3b1efb4c4edc38d
=======
    private const string ConnectionString = @"Data Source=C:\Users\joey-\Documents\GitHub\LocalTest\Mydatabase.db";
>>>>>>> 0386f2ee0b7f151f9574bc66a3b1efb4c4edc38d

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
<<<<<<< HEAD
        return nextDate;
=======
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
>>>>>>> 0386f2ee0b7f151f9574bc66a3b1efb4c4edc38d
    }
}
