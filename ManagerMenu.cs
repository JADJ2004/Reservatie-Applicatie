using System;
using ReservationApplication;

public static class ManagerMenu
{
    public static void StartUp()
    {
        string prompt = @"
************************************************************************************************/
█▀▄▀█ ▄▀█ █▄░█ ▄▀█ █▀▀ █▀▀ █▀█   █▀▄▀█ █▀▀ █▄░█ █░█
█░▀░█ █▀█ █░▀█ █▀█ █▄█ ██▄ █▀▄   █░▀░█ ██▄ █░▀█ █▄█
************************************************************************************************/

Welkom Marcel wat wilt u vandaag gaan doen?";
        string[] options = { "Reserveringen inzien", "Reservering wijzigen", "Reservering verwijderen", "Menu veranderen", "Uitloggen" };

        UserInterface ManagerMenu = new UserInterface(prompt, options, () => Menus.StartUp());

        int selectedIndex = ManagerMenu.Run();

        switch (selectedIndex)
        {
            case 0:
                ManagerReservationViewer reservationViewer = new ManagerReservationViewer();
                reservationViewer.ViewReservationsByDate();
                break;
            case 1:
                ManagerReservationChanger reservationChanger = new ManagerReservationChanger();
                reservationChanger.ChangeReservation();
                break;
            case 2:
                ManagerReservationDeleter reservationDeleter = new ManagerReservationDeleter();
                reservationDeleter.DeleteReservation();
                break;
            case 3:
                Console.WriteLine("Not implemented");
                break;
            case 4:
                Console.WriteLine("U bent uitgelogd.");
                Menus.StartUp();
                break;
        }
    }
}
