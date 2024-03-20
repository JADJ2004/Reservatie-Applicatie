using System;

public static class Menus
{
    private static bool Menu; // Menu loop variabele
    private static string Keuze; // Keuze van de user.

    private static string AdminWachtwoord = "1234YES!";
    private static string Wachtwoord;

    public static string StartUp()
    {
        Menu = true;

        while (Menu)
        {
            Console.WriteLine("\nWelkom bij het keuzemenu van YES!");
            Console.WriteLine("Maak een keuze (1/2/3).");
            Console.WriteLine("1. Inloggen als beheerder.");
            Console.WriteLine("2. Reserveer een tafel.");
            Console.WriteLine("3. Annuleer of bewerk je reservering.");
            Console.WriteLine("4. Check het menu.");

            Keuze = Console.ReadLine();

            if (Keuze != null)
            {
                return Keuze;
            }
        }

        return null; // Dit word nooit gebruikt maar het hoort er.
    }

    public static void AdminLogin()
    {
        Console.WriteLine("Dit is het log in scherm voor de beheerder.");
        Console.WriteLine("Wat is het wachtwoord?");

        Wachtwoord = Console.ReadLine();

        if (Wachtwoord == AdminWachtwoord)
        {
            Console.WriteLine("Je bent succesvol ingelogd als beheerder.");
            Menu = false; // Uit de menu loop.
            // Admin Menu
        }
        else
        {
            Console.WriteLine("Dit wachtwoord is fout. Je gaat terug naar het hoofdmenu.");
            StartUp(); // Terug naar het main menu
        }
    }
}
