using System;
using System.Collections.Generic;
using Customer_Reservation_Deleter;

namespace ReservationApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            Database db = new Database();
            db.InitializeDatabase();

            CRD reservationDeleter = new CRD();

            string keuze = ""; 

            
            List<string> menuOpties = new List<string>
            {
                "Admin Login",
                "Reserveer een tafel",
                "Annuleer of bewerk je reservering",
                "Toon het menu",
                "Afsluiten"
            };

            int selectedIndex = 0; 

            do
            {
                Console.Clear(); 

                
                Console.WriteLine(@"
   __   __  _______  _______  _______  __  
|  | |  ||       ||       ||       ||  | 
|  |_|  ||    ___||  _____||  _____||  | 
|       ||   |___ | |_____ | |_____ |  | 
|_     _||    ___||_____  ||_____  ||__| 
  |   |  |   |___  _____| | _____| | __  
  |___|  |_______||_______||_______||__| ");

                Console.WriteLine("Welkom bij Yess Restaurant!");
                Console.WriteLine("Kies een optie:");

                
                for (int i = 0; i < menuOpties.Count; i++)
                {
                    if (i == selectedIndex)
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow; 
                    }
                    Console.WriteLine($"{i + 1}. {menuOpties[i]}");
                    Console.ResetColor();
                }

               
                ConsoleKeyInfo keyInfo = Console.ReadKey();

                
                if (keyInfo.Key == ConsoleKey.UpArrow)
                {
                    
                    selectedIndex = (selectedIndex - 1 + menuOpties.Count) % menuOpties.Count;
                }
                else if (keyInfo.Key == ConsoleKey.DownArrow)
                {
                    
                    selectedIndex = (selectedIndex + 1) % menuOpties.Count;
                }
                else if (keyInfo.Key == ConsoleKey.Enter)
                {
                    
                    keuze = (selectedIndex + 1).ToString(); 

                    
                    switch (keuze)
                    {
                        case "1":
                            Console.WriteLine("\nAdmin Login wordt gestart...");
                            Menus.AdminLogin();
                            break;
                        case "2":
                            Console.WriteLine("\nReserveer een tafel.");
                            TestApplicatie reservationApplication = new TestApplicatie();
                            reservationApplication.ReservationSystem();
                            break;
                        case "3":
                            Console.WriteLine("\nAnnuleer of bewerk je reservering.");
                            reservationDeleter.ReservationDeleter();
                            break;
                        case "4":
                            Console.WriteLine("\nToon het menu.");
                            FoodMenu foodMenu = new FoodMenu();
                            foodMenu.ToonMenu();
                            break;
                        case "5":
                            Console.WriteLine("\nFijne dag.");
                            break;
                        default:
                            Console.WriteLine("\nDit is geen geldige optie. Probeer opnieuw.");
                            break;
                    }

                   
                    Console.WriteLine("\nDruk op een toets om door te gaan...");
                    Console.ReadKey();
                }

            } while (keuze != "5"); 

            Console.Clear();
            Console.WriteLine("\nBedankt voor het gebruik van de applicatie!");
        }
    }
}
