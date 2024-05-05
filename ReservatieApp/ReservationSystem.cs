using Microsoft.Data.Sqlite;
using System;
using System.Net;
using System.Net.Mail;
public class ReservationSystem
{
    private SqliteConnection conn;
    private const string ConnectionString = @"Data Source=C:\Users\rensg\OneDrive\Documenten\GitHub\Reservatie-Applicatie\ReservatieApp\Mydatabase.db";

    public ReservationSystem()
    {
        conn = new SqliteConnection(ConnectionString);
        conn.Open();
    }

    public (int tableId, DateTime nextAvailableDate) ReserveTableForGroup(int numOfPeople, bool wantsWindow, DateTime date)
    {
        int reservedTableId = FindAvailableTable(numOfPeople,wantsWindow, date);
        
        if (reservedTableId == -1)
        {
            DateTime nextAvailableDate = FindNextAvailableDate(numOfPeople,wantsWindow, date);
            reservedTableId = FindAvailableTable(numOfPeople,wantsWindow, nextAvailableDate);
            return (reservedTableId, nextAvailableDate);
        }

        return (reservedTableId, date);
    }

    private int FindAvailableTable(int numOfPeople, bool wantsWindow, DateTime date)
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
            cmd.Parameters.AddWithValue("@NumOfPeople", numOfPeople);
            cmd.Parameters.AddWithValue("@Date", formattedDate);
            cmd.Parameters.AddWithValue("@WantWindow", wantsWindow ? 1 : 0); // Convert bool to 0 or 1

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

    private DateTime FindNextAvailableDate(int numOfPeople, bool wantsWindow, DateTime startDate)
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
                cmd.Parameters.AddWithValue("@NumOfPeople", numOfPeople);
                cmd.Parameters.AddWithValue("@Date", formattedDate);
                cmd.Parameters.AddWithValue("@WantWindow", wantsWindow ? 1 : 0); // Convert bool to 0 or 1

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



    public void SendEmail(string CostumerEmail, DateTime RerservationDate, string firstName, int NumOfPeople)
    {
        string smtpServer = "smtp-mail.outlook.com";
        int port = 587;
        string username = "yessrestaurant@outlook.com"; 
        string password = "Marcel12345";

        string from = "yessrestaurant@outlook.com";
        string to = $"{CostumerEmail}"; 

        
        string subject = "Bevestiging van reservering bij YES! Restaurant";
        string body = $"Beste {firstName},\n\n" +
                          $"Bedankt voor uw reservering bij YES! Restaurant.\n" +
                          $"Uw reserverings-ID is:\n" +
                          $"Datum: {RerservationDate.ToShortDateString()}\n" +
                          $"Aantal personen: {NumOfPeople}\n\n" +
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
            // Verstuur de e-mail
            client.Send(message);
            Console.WriteLine("Bevestigingsmail is succesvol verstuurd.");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Er is een fout opgetreden bij het versturen van de e-mail: " + ex.Message);
        }
    }
    
}