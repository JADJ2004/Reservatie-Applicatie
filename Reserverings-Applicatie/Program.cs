class Program
{
    static void Main(string[] args)
    {
        string choice = Menus.StartUp(); // Geeft de gekozen optie.

        switch (choice)
        {
            case "1":
                Menus.AdminLogin(); // If the user chooses option 1, attempt admin login
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
                Console.WriteLine("Invalid choice. Please select a valid option.");
                break;
        }

        // Here you can add more code to continue the program flow
    }
}
