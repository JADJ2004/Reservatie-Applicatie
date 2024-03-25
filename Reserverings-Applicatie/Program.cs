public class Program
{
    static void Main(string[] args)
    {
        string choice;

        do
        {
            choice = Menus.StartUp();

            switch (choice)
            {
                case "1":
                    Menus.AdminLogin();
                    break;
                case "2":
                    Console.WriteLine("Option 2 selected: Reserveer een tafel.");
                    break;
                case "3":
                    Console.WriteLine("Option 3 selected: Annuleer of bewerk je reservering.");
                    break;
                case "4":
                    Console.WriteLine("Option 4 selected: Check het menu.");
                    break;
                default:
                    Console.WriteLine("Dit is geen geldige optie.");
                    break;
            }
        } while (choice != "1" && choice != "2" && choice != "3" && choice != "4");
    }
}
