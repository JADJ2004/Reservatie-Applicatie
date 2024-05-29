using System;

public class Admin
{
    public void StartMenu()
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("*******************************************");
        Console.WriteLine("*              ADMIN MENU                 *");
        Console.WriteLine("*******************************************");
        
        Console.WriteLine("Welkom Marcel. Wat wilt u gaan doen?");
        Console.WriteLine("1. Reserveringen inzien.");
        Console.WriteLine("2. Menu veranderen.");
        Console.WriteLine("3. Uitloggen");

        bool validChoice = false;

        while (!validChoice)
        {
            string keuze = Console.ReadLine();

            switch (keuze)
            {
                case "1":
                    ReserveringZien();
                    break;
                case "2":
                    MenuVeranderen();
                    break;
                case "3":
                    Uitloggen();
                    validChoice = true;
                    break;
                default:
                    Console.WriteLine("Dit is geen geldige keuze. Probeer opnieuw:");
                    break;
            }
        }
    }

    private void ReserveringZien()
    {
        Console.WriteLine("Hier worden de reserveringen getoond.");
        // Voeg hier de logica toe om reserveringen te tonen
    }

    private void MenuVeranderen()
    {
        Console.WriteLine("Hier kunt u het menu veranderen.");
        // Voeg hier de logica toe om het menu te wijzigen
    }

    private void Uitloggen()
    {
        Console.WriteLine("U bent uitgelogd.\n");
    }
}
