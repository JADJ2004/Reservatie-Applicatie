using System;

// dit is eig placeholder
class ReserveringAanvragen
{
    public void Reserveren()
    {
        Console.WriteLine("Welcome to the reservation system.");

        Console.WriteLine("Do you prefer a table next to a window? (yes/no):");
        bool wantWindow = Console.ReadLine()?.Trim().ToLower() == "yes";

        Console.WriteLine("Please enter the number of people for the reservation:");
        int numOfPeople = int.Parse(Console.ReadLine() ?? "0");

        ReservationSystem rs = new ReservationSystem();
        rs.ReserveTableForGroup(numOfPeople, wantWindow);
    }
}
