using System;
using Customer_Reservation_Deleter;
using System.Collections.Generic;
using System.Text;
using static System.Console;
using System.Net.Http.Headers;
using System.Threading;

<<<<<<< Updated upstream:Reserverings-Applicatie/Menus.cs
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
        string keuze = Console.ReadLine();
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
=======
>>>>>>> Stashed changes:ReserveringsApplicatie/Menus.cs

namespace ReservationApplication{

        public static class Menus
        {
            private static string keuze;
            private static string AdminWachtwoord = "1234YES!";
            private static string Wachtwoord;
            
            public static void StartUp()
            {
                Database db = new Database();
                    db.InitializeDatabase();

                    CRD reservationDeleter = new CRD();


                    string prompt = @"
Welkom bij het hoofdmenu van YES! Selecteer een optie.
Gebruik de pijltes toetsen om te selecteen en klik ENTER om het te kiezen.";
                    string[] options = {"Log in als admin", "Reserveer een tafel", "Annuleer of bewerk je reservering", "Toon het menu", "Verlaten"};

                    MenuTest mainMenu = new MenuTest(prompt, options);
                    int selectedIndex = mainMenu.Run();

                    switch(selectedIndex)
                    {
                        case 0:
                            Menus.AdminLogin();
                            break;
                        case 1:
                            TestAplicatie reservationApplication = new TestAplicatie();
                            reservationApplication.ReservationSystem();
                            break;
                        case 2: 
                            reservationDeleter.ReservationDeleter();
                            break;
                        case 3:
                            FoodMenu foodMenu = new FoodMenu();
                            foodMenu.ToonMenu();
                            break;
                        case 4:
                            Console.WriteLine("Tot ziens!");
                            Menus.ExitMenu();
                            break;
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
                    Thread.Sleep(2000);
                    Menus.StartUp();
                }
            }

            public static void ExitMenu()
            {
                Environment.Exit(0);
            }
            
        }
}

