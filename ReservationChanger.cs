using System;
using Microsoft.Data.Sqlite;

public class ReservationChanger
{
    private string connectionString;
    

    public ReservationChanger(string dbFilePath)
    {
        connectionString = $"Data Source={dbFilePath};";
    }

    public void UpdateReservation(int numOfPeople, string timeSlot, string name, string addition, string surname, int phoneNumber, string email, DateTime date, int reservationId)
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            string query = "UPDATE Reservations SET Date = @ReservationDate, TimeSlot = @TimeSlot, NumOfPeople = @NumOfPeople, First_name = @First_name, Infix = @Addition, Last_name = @Last_name, PhoneNumber = @PhoneNumber, Email = @Email WHERE ReservationId = @ReservationId";
            var command = new SqliteCommand(query, connection);

            command.Parameters.AddWithValue("@ReservationDate", date.ToString("dd-MM-yyyy"));
            command.Parameters.AddWithValue("@TimeSlot", timeSlot);
            command.Parameters.AddWithValue("@NumOfPeople", numOfPeople);
            command.Parameters.AddWithValue("@First_name", name);
            command.Parameters.AddWithValue("@Addition", addition);
            command.Parameters.AddWithValue("@Last_name", surname);
            command.Parameters.AddWithValue("@PhoneNumber", phoneNumber);
            command.Parameters.AddWithValue("@Email", email);
            command.Parameters.AddWithValue("@ReservationId", reservationId);

            try
            {
                connection.Open();
                int rowsAffected = command.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    Console.WriteLine("Reservering succesvol veranderd.");
                }
                else
                {
                    Console.WriteLine("Geen reservering gevonden met het opgegeven ID.");
                }
            }
            catch (SqliteException ex)
            {
                throw new Exception("Error updating reservation: " + ex.Message);
            }
        }
    }
}
