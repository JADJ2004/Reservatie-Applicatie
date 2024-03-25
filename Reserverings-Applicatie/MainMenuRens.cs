using System;

class Program
{
    static void Main(string[] args)
    {
        // Create an instance of the database class
        Database db = new Database();

        // Create the table if it doesn't exist
        db.Database_con();
        db.CreateTableTable();

        // Add a reservation
        DateTime dateTime = new DateTime (2024, 4, 5);
        db.AddReservation(2, "Rens","", "Gijzen", 0640590516 , "rensgijzen@example.com", dateTime);
        
        Console.WriteLine("Reservation added successfully.");
        db.PrintReservations();
        
    }
}
