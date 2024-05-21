using ReservationApplication;

public static class ManagerTools
{
    private static string keuze;
    private static string AdminWachtwoord = "1234YES!";
    private static string Wachtwoord;
            
    public static void StartUp()
    {
        string prompt = @"
Welkom Marcel, wat wilt u vandaag gaan doen?";
        string[] options = { "Reserveringen inzien", "Menu veranderen", "Uitloggen" };

        UserInterface ManagerMenu = new UserInterface(prompt, options);

        int selectedIndex = ManagerMenu.Run();

        switch(selectedIndex)
        {
            case 0:
                Console.WriteLine("Not implemented");
                break;
            case 1:
                Console.WriteLine("Menu bewerken:");
                FoodMenu menu = new FoodMenu();
                menu.ToonMenu();
                Console.WriteLine("Selecteer een categorie om een gerecht te bewerken (0 = Voorgerechten, 1 = Hoofdgerechten, 2 = Desserts):");
                int categoryIndex = Convert.ToInt32(Console.ReadLine());
                Console.WriteLine("Selecteer het nummer van het gerecht dat u wilt bewerken:");
                int foodIndex = Convert.ToInt32(Console.ReadLine());
                Console.WriteLine("Voer de nieuwe naam van het gerecht in:");
                string newName = Console.ReadLine();
                Console.WriteLine("Voer de nieuwe prijs van het gerecht in:");
                double newPrice = Convert.ToDouble(Console.ReadLine());
                menu.EditFood(categoryIndex, foodIndex, newName, newPrice);
                Menus.StartUp();
                break;
            case 2:
                Console.WriteLine("Uitloggen...");
                break;
            default:
                Console.WriteLine("Ongeldige keuze");
                break;
        }
    }
}
