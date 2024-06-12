﻿using System;
using Customer_Reservation_Deleter;
using System.Collections.Generic;
using System.Text;
using static System.Console;
using System.Net.Http.Headers;
using System.Threading;

namespace ReservationApplication
{
    public static class Menus
    {
        private static string keuze;
        private static string AdminWachtwoord = "1234YES!";
        private static string Wachtwoord;

        public static void StartUp()
        {
            Database db = new Database();
            db.InitializeDatabase();
            Console.Clear();

            CRD reservationDeleter = new CRD();

            string prompt = @"

                                                                ██╗░░░██╗███████╗░██████╗██╗
                                                                ╚██╗░██╔╝██╔════╝██╔════╝██║
                                                                ░╚████╔╝░█████╗░░╚█████╗░██║
                                                                ░░╚██╔╝░░██╔══╝░░░╚═══██╗╚═╝
                                                                ░░░██║░░░███████╗██████╔╝██╗
                                                                ░░░╚═╝░░░╚══════╝╚═════╝░╚═╝
                                    █▀█ █▀▀ █▀ ▀█▀ ▄▀█ █░█ █▀█ ▄▀█ █▄░█ ▀█▀   █▀█ █▀▀ █▀ █▀▀ █▀█ █░█ ▄▀█ ▀█▀ █ █▀▀   ▀█▀ █▀█ █▀█ █░░
                                    █▀▄ ██▄ ▄█ ░█░ █▀█ █▄█ █▀▄ █▀█ █░▀█ ░█░   █▀▄ ██▄ ▄█ ██▄ █▀▄ ▀▄▀ █▀█ ░█░ █ ██▄   ░█░ █▄█ █▄█ █▄▄
**********************************************************************************************************************************************************/
Welkom bij het hoofdmenu van YES! Selecteer een optie.
Gebruik de pijltoetsen om te selecteren en klik ENTER om het te kiezen.
klik ESC om terug te gaan of het programma af te sluiten.";
            string[] options = { "Log in als admin", "Reserveer een tafel", "Annuleer je reservering", "Bewerk je reservering", "Bekijk je reservering", "Toon het menu", "Afsluiten" };

            UserInterface mainMenu = new UserInterface(prompt, options, () => ExitMenu());
            int selectedIndex = mainMenu.Run();
            Console.Clear();

            switch (selectedIndex)
            {
                case 0:
                    AdminLogin();
                    break;
                case 1:
                    TestApplicatie reservationApplication = new TestApplicatie();
                    reservationApplication.StartReservationSystem();
                    break;
                case 2:
                    reservationDeleter.ReservationDeleter();
                    break;
                case 3:
                    CustomerReservationChanger reservationChanger = new CustomerReservationChanger();
                    reservationChanger.ReservationChanger();
                    break;
                case 4:
                    CustomerReservationViewer reservationViewer = new CustomerReservationViewer();
                    reservationViewer.ViewReservationById();
                    break;
                case 5:
                    FoodMenu foodMenu = new FoodMenu();
                    foodMenu.ToonMenu();
                    break;
                case 6:
                    Console.WriteLine("Tot ziens!");
                    ExitMenu();
                    break;
            }
        }

    public static void AdminLogin()
        {
            Checker check = new Checker();

            bool access = check.AdminWachtwoord();

            if (access)
            {
                Console.WriteLine("Je bent succesvol ingelogd als beheerder.");
                ManagerMenu.StartUp();
            }
            else
            {
                Console.WriteLine("Dit wachtwoord is fout. Je gaat terug naar het hoofdmenu.");
                System.Threading.Thread.Sleep(2000);
                StartUp();
            }
        }
 

        public static void ExitMenu()
        {
            Environment.Exit(0);
        }
    }
}
