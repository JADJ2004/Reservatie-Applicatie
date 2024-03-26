public class Program
{
    static void Main(string[] args)
    {
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
                    ReserveringAanvragen.Reserveren();
                    break;
                case "3":
                    Console.WriteLine("Option 3 selected: Annuleer of bewerk je reservering.");
                    break; // later
                case "4":
                    Console.WriteLine("Option 4 selected: Check het menu.");
                    break; //
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