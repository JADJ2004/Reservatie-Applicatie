using System;

class Program
{
    static void Main(string[] args)
    {
        // Create an instance of the database class
        database db = new database();

        // Create the table if it doesn't exist
        db.Database_con();

        // Add a reservation
        db.AddReservation(4, "John", "Doe", 123456789, "john.doe@example.com", 20240321);
        
        Console.WriteLine("Reservation added successfully.");

        // Keep the console window open
        Console.ReadLine();
    }
}
