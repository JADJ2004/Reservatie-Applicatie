using ReservationApplication;

public static class ManagerMenu
{
    public static void StartUp()
    {
        string prompt = @"
Welkom Marcel wat wilt u vandaag gaan doen?";
        string[] options = { "Reserveringen inzien", "Menu veranderen", "Uitloggen" };

        UserInterface ManagerMenu = new UserInterface(prompt, options);

        int selectedIndex = ManagerMenu.Run();

        switch (selectedIndex)
        {
            case 0:
                ReservationViewer reservationViewer = new ReservationViewer();
                reservationViewer.ViewReservationsByDate();
                break;
            case 1:
                Console.WriteLine("Not implemented");
                break;
            case 2:
                Console.WriteLine("U bent uitgelogd.");
                Menus.StartUp();
                break;
        }
    }
}
