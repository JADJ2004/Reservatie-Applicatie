public class ManagerTools
{
     private static string keuze;
    private static string AdminWachtwoord = "1234YES!";
    private static string Wachtwoord;
            
    public static void StartUp()
    {
        string prompt = @"
Welkom Marcel wat wilt u vandaag gaan doen?";
        string[] options = {"Reserveringen in zien", "Menu veranderen", "Uitloggen"};

        UserInterface ManagerMenu = new UserInterface(prompt, options);

        int selectedIndex = ManagerMenu.Run();

        switch(selectedIndex)
        {
            case 0:
                Console.WriteLine("Not implemented");
                break;
            case 1:
                Console.WriteLine("Not implemented");
                break;
            case 2:
                Console.WriteLine("Not implemented");
                break;
            case 3:
                Console.WriteLine("Not implemented");
                break;
        }
    }
}