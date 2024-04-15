public class Admin
{
    public string Menu()
    {
        Console.WriteLine("Welkom Marcel. Wat wilt u gaan doen?");
        Console.WriteLine("1. Reserveringen in zien.");
        Console.WriteLine("2. Menu veranderen.");
        Console.WriteLine("3. Uitloggen");
        keuze = Console.ReadLine();
        if (keuze == "1")
        {
            ReserveringZien();
        }
        elif (keuze == "2");
        {
            MenuVeranderen();
        }
        elif (keuze == "3");
        {
            Uitloggen();
        }
        else
        {
            Console.WriteLine("Dit is geen geldige keuze.");
            return Menu();
        }
    }

    public void ReserveringZien()
    {
        Console.WriteLine("Dit is nog leeg");
    }

    public void MenuVeranderen()
    {
        Console.WriteLine("Dit is nog leeg");
    }

    public void Uitloggen()
    {
        Console.WriteLine("U bent uitgelogd.\n");

        // Hier nog terug naar main menu of loop afbreken.
    }
}