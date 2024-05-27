<<<<<<< HEAD
<<<<<<< HEAD:ReserveringsApplicatie/ManagerTools.cs
public static class ManagerTools
=======
using ReservationApplication;

public static class ManagerMenu
>>>>>>> 0386f2ee0b7f151f9574bc66a3b1efb4c4edc38d:ReserveringsApplicatie/ManagerMenu.cs
{
    private static string keuze;
    private static string AdminWachtwoord = "1234YES!";
    private static string Wachtwoord;
            
=======
using ReservationApplication;

public static class ManagerMenu
{
>>>>>>> 0386f2ee0b7f151f9574bc66a3b1efb4c4edc38d
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
<<<<<<< HEAD
                Console.WriteLine("Not implemented");
                break;
            case 3:
                Console.WriteLine("Not implemented");
                break;
        }
    }
<<<<<<< HEAD:ReserveringsApplicatie/ManagerTools.cs
}
=======
}
>>>>>>> 0386f2ee0b7f151f9574bc66a3b1efb4c4edc38d:ReserveringsApplicatie/ManagerMenu.cs
=======
                Console.WriteLine("U bent uitgelogd.");
                Menus.StartUp();
                break;
        }
    }
}
>>>>>>> 0386f2ee0b7f151f9574bc66a3b1efb4c4edc38d
