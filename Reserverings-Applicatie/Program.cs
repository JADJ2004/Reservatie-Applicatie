using System;
using Customer_Reservation_Deleter;
using System.Collections.Generic;
using System.Text;
using System.Console;

// namespace ReservationApplication
// {
//     class Program
//     {
//         static void Main(string[] args)
//         {
//             Database db = new Database();
//             db.InitializeDatabase();

//             CRD reservationDeleter = new CRD();

//             string keuze;
//             do
//             {
//                 keuze = Menus.StartUp();
//                 switch (keuze)
//                 {
//                     case "1":
//                         Menus.AdminLogin();
//                         break;
//                     case "2":
//                         Console.WriteLine("Reserveer een tafel.");
//                         TestAplicatie reservationApplication = new TestAplicatie();
//                         reservationApplication.ReservationSystem();
//                         break;
//                     case "3":
//                         Console.WriteLine("Annuleer of bewerk je reservering.");
//                         reservationDeleter.ReservationDeleter();
//                         break;
//                     case "4":
//                         Console.WriteLine("Toon het menu.");
//                         FoodMenu foodMenu = new FoodMenu();
//                         foodMenu.ToonMenu();
//                         break;
//                     case "5":
//                         Console.WriteLine("Fijne dag.");
//                         break;
//                     default:
//                         Console.WriteLine("\nDit is geen geldige optie.");
//                         break;
//                 }
//             } while (keuze != "5");
//         }
//     }
// }


namespace {Menu}
 {
        class MainMenu
        {
            public void Start()
            {
                Console.WriteLine("Welkom bij het keuzemenu van YES!");

                Console.KeyInfo keyPressed = ReadKey();

                if (keyPressed.Key == Console.Key.Enter)
                {
                    Console.WriteLine("You pressed ENTER");
                }
                else if (keyPressed.Key == Console.Key.UpArrow)
                {
                    WriteLine("You pressed the UP ARROW");
                }
                else 
                {
                    Console.WriteLine("You pressed another key");
                }

                WriteLine("Press key to exit.");
                Readkey(true);
            }
        }
 }