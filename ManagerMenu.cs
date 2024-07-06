using System;
using ReservationApplication;

public static class ManagerMenu
{
    private static MenuManager menuManager;

    public static void StartUp()
    {
        string prompt = @"
************************************************************************************************/
█▀▄▀█ ▄▀█ █▄░█ ▄▀█ █▀▀ █▀▀ █▀█   █▀▄▀█ █▀▀ █▄░█ █░█
█░▀░█ █▀█ █░▀█ █▀█ █▄█ ██▄ █▀▄   █░▀░█ ██▄ █░▀█ █▄█
************************************************************************************************/

Welkom Marcel wat wilt u vandaag gaan doen?";
        string[] options = { "Reserveringen inzien", "Reservering wijzigen", "Reservering verwijderen", "Menu veranderen", "Groepsreservering", "Uitloggen" };

        menuManager = new MenuManager(new Database());

        UserInterface ManagerMenu = new UserInterface(prompt, options, () => Menus.StartUp());

        int selectedIndex = ManagerMenu.Run();

        switch (selectedIndex)
        {
            case 0:
                ManagerReservationViewer reservationViewer = new ManagerReservationViewer();
                reservationViewer.ViewReservationsByDate();
                break;
            case 1:
                ManagerReservationChanger reservationChanger = new ManagerReservationChanger();
                reservationChanger.ChangeReservation();
                break;
            case 2:
                ManagerReservationDeleter reservationDeleter = new ManagerReservationDeleter();
                reservationDeleter.DeleteReservation();
                break;
            case 3:
                ChangeMenu();
                break;
            case 4:
                LargeReservationManager largeReservationManager = new LargeReservationManager();
                largeReservationManager.MakeLargeReservation();
                break;
            case 5:
                Console.WriteLine("U bent uitgelogd.");
                Menus.StartUp();
                break;
        }
    }

    private static void ChangeMenu()
    {
        Console.Clear();
        Console.WriteLine("MENU VERANDEREN");
        Console.WriteLine("1. Voeg nieuw menu-item toe");
        Console.WriteLine("2. Verwijder menu-item");

        Console.Write("Selecteer een optie: ");
        string input = Console.ReadLine();

        switch (input)
        {
            case "1":
                AddMenuItem();
                break;
            case "2":
                RemoveMenuItem();
                break;
            default:
                Console.WriteLine("Ongeldige optie.");
                break;
        }
    }

    private static void AddMenuItem()
    {
        Console.Clear();
        Console.WriteLine("NIEUW MENU-ITEM TOEVOEGEN");

        Console.Write("Categorie: ");
        string category = Console.ReadLine();

        Console.Write("Naam: ");
        string name = Console.ReadLine();

        Console.Write("Prijs: ");
        double price;
        if (!double.TryParse(Console.ReadLine(), out price))
        {
            Console.WriteLine("Ongeldige prijs.");
            return;
        }

        menuManager.AddMenuItem(category, name, price);
        Console.WriteLine("Menu-item succesvol toegevoegd.");
    }

    private static void RemoveMenuItem()
    {
        Console.Clear();
        Console.WriteLine("MENU-ITEM VERWIJDEREN");

        Console.Write("Categorie: ");
        string category = Console.ReadLine();

        Console.Write("Naam: ");
        string name = Console.ReadLine();

        menuManager.RemoveMenuItem(category, name);
        Console.WriteLine("Menu-item succesvol verwijderd.");
    }
}
