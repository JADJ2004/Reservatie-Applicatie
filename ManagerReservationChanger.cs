using System;
using ReservationApplication;
using System.Text.RegularExpressions;

namespace ReservationApplication
{
    public class ManagerReservationChanger
    {
        private const string ConnectionString = @"Data Source=C:\Users\noah\OneDrive\Documenten\sprint-5\Sprint5Local\Mydatabase.db";
        private Database db = new Database();

        public void ChangeReservation()
        {
            Console.WriteLine("Voer uw reserverings-ID in:");
            if (int.TryParse(Console.ReadLine(), out int reservationId))
            {
                var reservation = db.GetReservationById(reservationId);

                if (reservation != null)
                {
                    Console.WriteLine("Reserveringsdetails:");
                    Console.WriteLine($"Reservering ID: {reservation.ReservationId}");
                    Console.WriteLine($"Tafel ID: {reservation.TableId}");
                    Console.WriteLine($"Aantal Personen: {reservation.NumOfPeople}");
                    Console.WriteLine($"Naam: {reservation.FirstName} {reservation.Infix} {reservation.LastName}");
                    Console.WriteLine($"Telefoonnummer: {reservation.PhoneNumber}");
                    Console.WriteLine($"E-mail: {reservation.Email}");
                    Console.WriteLine($"Datum: {reservation.Date}");
                    Console.WriteLine($"Tijdslot: {reservation.TimeSlot}");
                    Console.WriteLine($"Opmerkingen: {reservation.Remarks}");

                    Console.WriteLine("Wilt u deze reservatie veranderen? (ja/nee)");
                    string ChangeConfirmation = Console.ReadLine()?.Trim().ToLower();

                    if (ChangeConfirmation == "ja")
                    {
                        ChangeReservationInformation(reservationId);
                    }
                    else
                    {
                        Console.WriteLine("Je gaat terug naar het hoofdmenu.");
                        ManagerMenu.StartUp();
                    }
                }
                else
                {
                    Console.WriteLine("Reservering niet gevonden.");
                    ManagerMenu.StartUp();
                }
            }
            else
            {
                Console.WriteLine("Ongeldige invoer. Voer een geldig reserverings-ID in.");
                Console.WriteLine("Je gaat terug naar het hoofdmenu.");
                ManagerMenu.StartUp();
            }

            Console.WriteLine("Druk op een toets om terug te keren naar het menu.");
            Console.ReadKey();
            ManagerMenu.StartUp();
        }

        private void ChangeReservationInformation(int reservationId)
        {
            bool CRC_checker = false;
            while (!CRC_checker)
            {
                bool CRC_date_checker = false;
                bool CRC_first_name_checker = false;
                bool CRC_last_name_checker = false;
                bool CRC_phoneNumber_checker = false;
                bool CRC_people_checker = false;
                bool CRC_mail_checker = false;

                string CRC_date = "";
                int CRC_numOfPeople = 0;
                string CRC_name = "";
                string CRC_surname = "";
                string CRC_phoneNumber = "";
                string CRC_email = "";
                string CRC_addition = "";
                string CRC_reservation_checker = "";

                while (!CRC_date_checker)
                {
                    Console.WriteLine("Welkom bij het reserveringsapplicatie van YES!");
                    Console.Write("Voer uw reserveringsdatum in (dd-MM-yyyy): ");
                    CRC_date = Console.ReadLine() ?? "";
                    DateTime parsedDate;
                    if (DateTime.TryParseExact(CRC_date, "dd-MM-yyyy", null, System.Globalization.DateTimeStyles.None, out parsedDate))
                    {
                        CRC_date_checker = true;
                    }
                    else
                    {
                        Console.WriteLine("Incorrecte formaat. Probeer: (dd-MM-yyyy)");
                    }
                }

                string[] CRC_timeSlots = { "18:00-19:59", "20:00-21:59", "22:00-23:59" };
                string CRC_timeSlot = "";
                Console.WriteLine("Selecteer een tijdslot:");
                for (int i = 0; i < CRC_timeSlots.Length; i++)
                {
                    Console.WriteLine($"{i + 1}. {CRC_timeSlots[i]}");
                }
                while (true)
                {
                    Console.Write("Kies een optie (1-3): ");
                    if (int.TryParse(Console.ReadLine(), out int slot) && slot >= 1 && slot <= 3)
                    {
                        CRC_timeSlot = CRC_timeSlots[slot - 1];
                        break;
                    }
                    Console.WriteLine("Ongeldige invoer. Kies een optie tussen 1 en 3.");
                }

                while (!CRC_people_checker)
                {
                    Console.Write("Aantal personen: ");
                    if (int.TryParse(Console.ReadLine(), out CRC_numOfPeople) && CRC_numOfPeople > 0 && CRC_numOfPeople < 48)
                    {
                        CRC_people_checker = true;
                    }
                    else
                    {
                        Console.WriteLine("Invalid aantal personen. Het aantal personen moet tussen 1 en 48 zijn.");
                    }
                }

                while (!CRC_first_name_checker)
                {
                    Console.WriteLine("\nGraag uw contactgegevens achterlaten:");
                    Console.Write("Voornaam: ");
                    CRC_name = Console.ReadLine() ?? "";
                    if (!string.IsNullOrWhiteSpace(CRC_name))
                    {
                        CRC_first_name_checker = true;
                    }
                    else
                    {
                        Console.WriteLine("Invalid input. Probeer alleen letters te gebruiken.");
                    }
                }

                Console.Write("Toevoeging: ");
                CRC_addition = Console.ReadLine() ?? "";

                while (!CRC_last_name_checker)
                {
                    Console.Write("Achternaam: ");
                    CRC_surname = Console.ReadLine() ?? "";
                    if (!string.IsNullOrWhiteSpace(CRC_surname))
                    {
                        CRC_last_name_checker = true;
                    }
                    else
                    {
                        Console.WriteLine("Invalid input. Probeer alleen letters te gebruiken.");
                    }
                }

                while (!CRC_phoneNumber_checker)
                {
                    Console.Write("Telefoonnummer: ");
                    CRC_phoneNumber = Console.ReadLine() ?? "";
                    if (CRC_phoneNumber.Length == 10 && long.TryParse(CRC_phoneNumber, out _))
                    {
                        CRC_phoneNumber_checker = true;
                    }
                    else
                    {
                        Console.WriteLine("Telefoonnummer moet 10 cijfers lang zijn.");
                    }
                }

                while (!CRC_mail_checker)
                {
                    Console.Write("E-mail: ");
                    CRC_email = Console.ReadLine() ?? "";
                    if (CRC_email.Contains("@") && CRC_email.Contains("."))
                    {
                        CRC_mail_checker = true;
                    }
                    else
                    {
                        Console.WriteLine("Invalid email. Probeer een echt email in te vullen.");
                    }
                }

                Console.WriteLine("\nHeeft u nog opmerkingen of verzoeken?");
                string CRC_comments = Console.ReadLine() ?? "";

                DateTime CRC_reservationDate;
                if (DateTime.TryParseExact(CRC_date, "dd-MM-yyyy", null, System.Globalization.DateTimeStyles.None, out CRC_reservationDate))
                {
                    var reservationChanger = new ReservationChanger(ConnectionString);
                    reservationChanger.UpdateReservation(CRC_numOfPeople, CRC_timeSlot, CRC_name, CRC_addition, CRC_surname, int.Parse(CRC_phoneNumber), CRC_email, CRC_reservationDate, reservationId);
                }

                Console.WriteLine("\nReserveringsgegevens:");
                Console.WriteLine("Datum: " + CRC_date);
                Console.WriteLine("Aantal Personen: " + CRC_numOfPeople);
                Console.WriteLine("Voornaam: " + CRC_name);
                Console.WriteLine("Toevoering: " + CRC_addition);
                Console.WriteLine("Achternaam: " + CRC_surname);
                Console.WriteLine("Telefoonnummer: " + CRC_phoneNumber);
                Console.WriteLine("E-mail: " + CRC_email);
                Console.WriteLine("Opmerkingen: " + CRC_comments);
                Console.WriteLine("Is dit je gewenste reservatie? (ja/nee)");
                string wijzigOptie = Console.ReadLine()?.Trim();

                switch (wijzigOptie)
                {
                    case "1":
                        // Datum wijzigen
                        CRC_date_checker = false;
                        while (!CRC_date_checker)
                        {
                            Console.Write("Voer uw nieuwe reserveringsdatum in (dd-MM-yyyy): ");
                            CRC_date = Console.ReadLine() ?? "";
                            if (DateTime.TryParseExact(CRC_date, "dd-MM-yyyy", null, System.Globalization.DateTimeStyles.None, out CRC_reservationDate))
                            {
                                CRC_date_checker = true;
                            }
                            else
                            {
                                Console.WriteLine("Ongeldige invoer. Probeer: (dd-MM-yyyy)");
                            }
                        }
                        break;

                    case "2":
                        // Tijdslot wijzigen
                        Console.WriteLine("Selecteer een nieuw tijdslot:");
                        for (int i = 0; i < CRC_timeSlots.Length; i++)
                        {
                            Console.WriteLine($"{i + 1}. {CRC_timeSlots[i]}");
                        }
                        while (true)
                        {
                            Console.Write("Kies een optie (1-3): ");
                            if (int.TryParse(Console.ReadLine(), out int slot) && slot >= 1 && slot <= 3)
                            {
                                CRC_timeSlot = CRC_timeSlots[slot - 1];
                                break;
                            }
                            Console.WriteLine("Ongeldige invoer. Kies een optie tussen 1 en 3.");
                        }
                        break;

                    case "3":
                        // Aantal personen wijzigen
                        CRC_people_checker = false;
                        while (!CRC_people_checker)
                        {
                            Console.Write("Nieuw aantal personen: ");
                            if (int.TryParse(Console.ReadLine(), out CRC_numOfPeople) && CRC_numOfPeople > 0 && CRC_numOfPeople <= 6)
                            {
                                CRC_people_checker = true;
                            }
                            else
                            {
                                Console.WriteLine("Ongeldig aantal personen.");
                            }
                        }
                        break;

                    case "4":
                        // Naam wijzigen
                        CRC_first_name_checker = false;
                        while (!CRC_first_name_checker)
                        {
                            Console.Write("Nieuwe voornaam: ");
                            CRC_name = Console.ReadLine() ?? "";
                            if (!string.IsNullOrWhiteSpace(CRC_name) && Regex.IsMatch(CRC_name, @"^[a-zA-Z]+$"))
                            {
                                CRC_first_name_checker = true;
                            }
                            else
                            {
                                Console.WriteLine("Ongeldige invoer. Probeer alleen letters te gebruiken.");
                            }
                        }

                        Console.Write("Nieuw tussenvoegsel (indien van toepassing, anders druk op Enter): ");
                        CRC_addition = Console.ReadLine() ?? "";

                        CRC_last_name_checker = false;
                        while (!CRC_last_name_checker)
                        {
                            Console.Write("Nieuwe achternaam: ");
                            CRC_surname = Console.ReadLine() ?? "";
                            if (!string.IsNullOrWhiteSpace(CRC_surname) && Regex.IsMatch(CRC_surname, @"^[a-zA-Z]+$"))
                            {
                                CRC_last_name_checker = true;
                            }
                            else
                            {
                                Console.WriteLine("Ongeldige invoer. Probeer alleen letters te gebruiken.");
                            }
                        }
                        break;

                    case "5":
                        // Telefoonnummer wijzigen
                        CRC_phoneNumber_checker = false;
                        while (!CRC_phoneNumber_checker)
                        {
                            Console.Write("Nieuw telefoonnummer: ");
                            CRC_phoneNumber = Console.ReadLine() ?? "";
                            if (CRC_phoneNumber.Length == 10 && long.TryParse(CRC_phoneNumber, out _))
                            {
                                CRC_phoneNumber_checker = true;
                            }
                            else
                            {
                                Console.WriteLine("Telefoonnummer moet 10 cijfers lang zijn.");
                            }
                        }
                        break;

                    case "6":
                        // E-mail wijzigen
                        CRC_mail_checker = false;
                        while (!CRC_mail_checker)
                        {
                            Console.Write("Nieuwe e-mail: ");
                            CRC_email = Console.ReadLine() ?? "";
                            if (CRC_email.Contains("@") && CRC_email.Contains("."))
                            {
                                CRC_mail_checker = true;
                            }
                            else
                            {
                                Console.WriteLine("Ongeldige e-mail. Probeer een echt e-mail in te vullen.");
                            }
                        }
                        break;

                    case "7":
                        // Opmerkingen/verzoeken wijzigen
                        Console.WriteLine("Nieuwe opmerkingen/verzoeken: ");
                        CRC_comments = Console.ReadLine() ?? "";
                        break;

                    default:
                        Console.WriteLine("Ongeldige optie. Probeer opnieuw.");
                        break;
                }
            }
            Console.WriteLine("\nDank u wel! We hopen u gauw te zien bij YES!");
        }
    }
}