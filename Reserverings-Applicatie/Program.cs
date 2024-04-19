using System;
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

            string keuze;
            do
            {
                keuze = Menus.StartUp();
                switch (keuze)
                {
                    case "1":
                        Menus.AdminLogin();
                        break;
                    case "2":
                        Console.WriteLine("Reserveer een tafel.");
                        TestAplicatie reservationApplication = new TestAplicatie();
                        reservationApplication.ReservationSystem();
                        break;
                    case "3":
                        Console.WriteLine("Annuleer of bewerk je reservering.");
                        reservationDeleter.ReservationDeleter();
                        break;
                    case "4":
                        Console.WriteLine("Toon het menu.");
                        FoodMenu foodMenu = new FoodMenu();
                        foodMenu.ToonMenu();
                        break;
                    case "5":
                        Console.WriteLine("Fijne dag.");
                        break;
                    default:
                        Console.WriteLine("\nDit is geen geldige optie.");
                        break;
                }
            } while (keuze != "5");
        }
    }
}
