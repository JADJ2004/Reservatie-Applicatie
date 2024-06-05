using System;
using Microsoft.Data.Sqlite;
using ReservationApplication;
using System.Text;

public class ReservationChanger
{
    private string connectionString;

    public ReservationChanger(string dbFilePath)
    {
        connectionString = $"Data Source={dbFilePath};";
    }

    public void UpdateReservation(int numOfPeople, string name, string addition, string surname, int phoneNumber, string email, DateTime date, int reservationId)
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            string query = "UPDATE Reservations SET Date = @ReservationDate, NumOfPeople = @NumOfPeople, FirstName = @FirstName, Addition = @Addition, LastName = @LastName, PhoneNumber = @PhoneNumber, Email = @Email WHERE ReservationId = @ReservationId";
            var command = new SqliteCommand(query, connection);

            command.Parameters.AddWithValue("@ReservationDate", date);
            command.Parameters.AddWithValue("@NumOfPeople", numOfPeople);
            command.Parameters.AddWithValue("@FirstName", name);
            command.Parameters.AddWithValue("@Addition", addition);
            command.Parameters.AddWithValue("@LastName", surname);
            command.Parameters.AddWithValue("@PhoneNumber", phoneNumber);
            command.Parameters.AddWithValue("@Email", email);
            command.Parameters.AddWithValue("@ReservationId", reservationId);

            try
            {
                connection.Open();
                int rowsAffected = command.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    Console.WriteLine("Reservation updated successfully.");
                }
                else
                {
                    Console.WriteLine("No reservation found with the provided ID.");
                }
            }
            catch (SqliteException ex)
            {
                Console.WriteLine("Error updating reservation: " + ex.Message);
            }
        }
    }

private string ReadInputWithEscape()
{
    var input = new StringBuilder();
    int cursorPosition = Console.CursorLeft;

    while (true)
    {
        var key = Console.ReadKey(intercept: true);
        if (key.Key == ConsoleKey.Enter)
        {
            Console.WriteLine();
            break;
        }
        if (key.Key == ConsoleKey.Escape)
        {
            Menus.StartUp();
            break;
        }
        if (key.Key == ConsoleKey.Backspace)
        {
            if (input.Length > 0 && Console.CursorLeft > cursorPosition + 0)
            {
                input.Remove(input.Length - 1, 1);
                Console.Write("\b \b");
            }
        }
        else if (char.IsWhiteSpace(key.KeyChar) && input.Length == 0)
        {
            // Ignore space at the beginning
            continue;
        }
        else
        {
            input.Append(key.KeyChar);
            Console.Write(key.KeyChar);
        }
    }
    return input.ToString();
}
    }