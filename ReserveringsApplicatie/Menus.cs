using System;

public static class Menus
{
    private static string keuze;
    private static string AdminWachtwoord = "1234YES!";
    private static string Wachtwoord;
    
    public static string StartUp()
    {
        Console.WriteLine("\nWelkom bij het keuzemenu van YES!");
        Console.WriteLine("Maak een keuze (1/2/3/4/5).");
        Console.WriteLine("1. Inloggen als beheerder.");
        Console.WriteLine("2. Reserveer een tafel.");
        Console.WriteLine("3. Annuleer of bewerk je reservering.");
        Console.WriteLine("4. Check het menu.");
        Console.WriteLine("5. Verlaat de applicatie.");
        keuze = Console.ReadLine();
        if (keuze == "1" || keuze == "2" || keuze == "3" || keuze == "4")
        {
            return keuze;
        }
        else if (keuze == "5")
        {
            Console.WriteLine("Fijne dag.");
            Environment.Exit(0);
            return null;
        }
        else
        {
            Console.WriteLine("Dit is geen geldige optie.");
            return StartUp();
        }
    }

    public static void AdminLogin()
    {
        Console.WriteLine("\nDit is het log in scherm voor de beheerder.");
        Console.WriteLine("Wat is het wachtwoord?");
        Wachtwoord = Console.ReadLine();
        if (Wachtwoord == AdminWachtwoord)
        {
            Console.WriteLine("Je bent succesvol ingelogd als beheerder.");
            new Admin().Menu();
        }
        else
        {
            Console.WriteLine("Dit wachtwoord is fout. Je gaat terug naar het hoofdmenu.");
        }
    }
}
