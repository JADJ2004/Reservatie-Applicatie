using System;

namespace ReservationApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            Database db = new Database();
            db.Database_con();
            db.CreateTableTable();
            DateTime dateTime = new DateTime(2024, 4, 5);

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
                        Console.WriteLine("Option 2 selected: Reserveer een tafel.");
                        TestAplicatie reservationApplication = new TestAplicatie();
                        reservationApplication.ReservationSystem();
                        break;
                    case "3":
                        Console.WriteLine("Option 3 selected: Annuleer of bewerk je reservering.");
                        break;
                    case "4":
                        Console.WriteLine("Option 4 selected: Check het menu.");
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
