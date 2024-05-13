using System;
using Customer_Reservation_Deleter;
using System.Collections.Generic;
using System.Text;
using static System.Console;
using System.Net.Http.Headers;
using System.Threading;


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
                    ManagerTools.StartUp();
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


