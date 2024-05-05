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

            string keuze = ""; // Initieer keuze met lege string

            // Definieer een lijst van menu-opties
            List<string> menuOpties = new List<string>
            {
                "Admin Login",
                "Reserveer een tafel",
                "Annuleer of bewerk je reservering",
                "Toon het menu",
                "Afsluiten"
            };

            int selectedIndex = 0; // Index van de geselecteerde optie

            do
            {
                Console.Clear(); // Scherm leegmaken voor een schone interface

                // ASCII-art logo voor "Yess Restaurant"
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

                // Toon menu-opties
                for (int i = 0; i < menuOpties.Count; i++)
                {
                    if (i == selectedIndex)
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow; // Markeer geselecteerde optie
                    }
                    Console.WriteLine($"{i + 1}. {menuOpties[i]}");
                    Console.ResetColor(); // Herstel de kleur naar standaard
                }

                // Wacht op een toetsaanslag
                ConsoleKeyInfo keyInfo = Console.ReadKey();

                // Verwerk de toetsaanslag
                if (keyInfo.Key == ConsoleKey.UpArrow)
                {
                    // Naar boven navigeren in de menu-opties
                    selectedIndex = (selectedIndex - 1 + menuOpties.Count) % menuOpties.Count;
                }
                else if (keyInfo.Key == ConsoleKey.DownArrow)
                {
                    // Naar beneden navigeren in de menu-opties
                    selectedIndex = (selectedIndex + 1) % menuOpties.Count;
                }
                else if (keyInfo.Key == ConsoleKey.Enter)
                {
                    // Gebruiker heeft Enter ingedrukt, selecteer de huidige optie
                    keuze = (selectedIndex + 1).ToString(); // Converteer naar keuze string

                    // Voer de geselecteerde actie uit op basis van de keuze
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

                    // Pauze om de gebruiker te laten zien wat er is gebeurd voordat het menu opnieuw wordt weergegeven
                    Console.WriteLine("\nDruk op een toets om door te gaan...");
                    Console.ReadKey();
                }

            } while (keuze != "5"); // Blijf doorgaan totdat "Afsluiten" is geselecteerd

            Console.Clear();
            Console.WriteLine("\nBedankt voor het gebruik van de applicatie!");
        }
    }
}
