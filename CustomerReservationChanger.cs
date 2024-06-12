using System;
using ReservationApplication;
using System.Text.RegularExpressions;
using System.Text;

public class CustomerReservationChanger
{
    private const string DbFilePath = @"Data Source=C:\Users\joey-\Documents\GitHub\ConsoleApp1\Mydatabase.db";
    private Database db;

    public CustomerReservationChanger()
    {
        db = new Database(); // Assuming Database constructor without arguments
    }

    public void ReservationChanger()
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("************************************************************************************************/");
        Console.WriteLine("█▀█ █▀▀ █▀ █▀▀ █▀█ █░█ █▀▀ █▀█ █ █▄░█ █▀▀   ▄▀█ ▄▀█ █▄░█ █▀█ ▄▀█ █▀ █▀ █▀▀ █▄░█");
        Console.WriteLine("█▀▄ ██▄ ▄█ ██▄ █▀▄ ▀▄▀ ██▄ █▀▄ █ █░▀█ █▄█   █▀█ █▀█ █░▀█ █▀▀ █▀█ ▄█ ▄█ ██▄ █░▀█");
        Console.WriteLine("************************************************************************************************/");
        Console.ResetColor();
        Console.WriteLine();

        bool CRC_checker = true;
        Console.WriteLine("Voer uw reserverings-ID in:");
        if (int.TryParse(ReadInputWithEscape(), out int reservationId))
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
                string ChangeConfirmation = ReadInputWithEscape()?.Trim().ToLower();

                if (ChangeConfirmation == "ja")
                {
                    while (CRC_checker)
                    {
                        Console.WriteLine("Welke informatie wilt u wijzigen?");
                        Console.WriteLine("1. Datum");
                        Console.WriteLine("2. Tijdslot");
                        Console.WriteLine("3. Aantal personen");
                        Console.WriteLine("4. Naam");
                        Console.WriteLine("5. Telefoonnummer");
                        Console.WriteLine("6. E-mail");
                        Console.WriteLine("7. Opmerkingen/verzoeken");
                        Console.Write("Kies een optie (1-7): ");
                        string wijzigOptie = ReadInputWithEscape()?.Trim();

                        bool validInput = false;
                        string CRC_date = string.Empty;
                        DateTime CRC_reservationDate = DateTime.MinValue;
                        int CRC_numOfPeople = reservation.NumOfPeople;
                        string CRC_timeSlot = reservation.TimeSlot;
                        string CRC_firstName = reservation.FirstName;
                        string CRC_infix = reservation.Infix;
                        string CRC_lastName = reservation.LastName;
                        int CRC_phoneNumber = int.Parse(reservation.PhoneNumber);
                        string CRC_email = reservation.Email;

                        switch (wijzigOptie)
                        {
                            case "1":
                                // Datum wijzigen
                                validInput = false;
                                while (!validInput)
                                {
                                    Console.Write("Voer uw nieuwe reserveringsdatum in (dd-MM-yyyy): ");
                                    CRC_date = ReadInputWithEscape() ?? "";
                                    if (DateTime.TryParseExact(CRC_date, "dd-MM-yyyy", null, System.Globalization.DateTimeStyles.None, out CRC_reservationDate))
                                    {
                                        validInput = true;
                                    }
                                    else
                                    {
                                        Console.WriteLine("Ongeldige invoer. Probeer: (dd-MM-yyyy)");
                                    }
                                }
                                break;

                            case "2":
                                // Tijdslot wijzigen
                                string[] CRC_timeSlots = { "18:00-19:59", "20:00-21:59", "22:00-23:59" };
                                Console.WriteLine("Selecteer een nieuw tijdslot:");
                                for (int i = 0; i < CRC_timeSlots.Length; i++)
                                {
                                    Console.WriteLine($"{i + 1}. {CRC_timeSlots[i]}");
                                }
                                validInput = false;
                                while (!validInput)
                                {
                                    Console.Write("Kies een optie (1-3): ");
                                    if (int.TryParse(ReadInputWithEscape(), out int slot) && slot >= 1 && slot <= 3)
                                    {
                                        CRC_timeSlot = CRC_timeSlots[slot - 1];
                                        validInput = true;
                                    }
                                    else
                                    {
                                        Console.WriteLine("Ongeldige invoer. Kies een optie tussen 1 en 3.");
                                    }
                                }
                                break;

                            case "3":
                                // Aantal personen wijzigen
                                validInput = false;
                                while (!validInput)
                                {
                                    Console.Write("Nieuw aantal personen: ");
                                    if (int.TryParse(ReadInputWithEscape(), out CRC_numOfPeople) && CRC_numOfPeople > 0 && CRC_numOfPeople <= 6)
                                    {
                                        validInput = true;
                                    }
                                    else
                                    {
                                        Console.WriteLine("Ongeldig aantal personen.");
                                    }
                                }
                                break;

                            case "4":
                                // Naam wijzigen
                                validInput = false;
                                while (!validInput)
                                {
                                    Console.Write("Nieuwe voornaam: ");
                                    CRC_firstName = ReadInputWithEscape() ?? "";
                                    if (!string.IsNullOrWhiteSpace(CRC_firstName) && Regex.IsMatch(CRC_firstName, @"^[a-zA-Z]+$"))
                                    {
                                        validInput = true;
                                    }
                                    else
                                    {
                                        Console.WriteLine("Ongeldige invoer. Probeer alleen letters te gebruiken.");
                                    }
                                }

                                Console.Write("Nieuw tussenvoegsel (indien van toepassing, anders druk op Enter): ");
                                CRC_infix = ReadInputWithEscape() ?? "";

                                validInput = false;
                                while (!validInput)
                                {
                                    Console.Write("Nieuwe achternaam: ");
                                    CRC_lastName = ReadInputWithEscape() ?? "";
                                    if (!string.IsNullOrWhiteSpace(CRC_lastName) && Regex.IsMatch(CRC_lastName, @"^[a-zA-Z]+$"))
                                    {
                                        validInput = true;
                                    }
                                    else
                                    {
                                        Console.WriteLine("Ongeldige invoer. Probeer alleen letters te gebruiken.");
                                    }
                                }
                                break;

                            case "5":
                                // Telefoonnummer wijzigen
                                validInput = false;
                                while (!validInput)
                                {
                                    Console.Write("Nieuw telefoonnummer: ");
                                    string phoneNumberInput = ReadInputWithEscape() ?? "";
                                    if (phoneNumberInput.Length == 10 && long.TryParse(phoneNumberInput, out long parsedPhoneNumber))
                                    {
                                        CRC_phoneNumber = (int)parsedPhoneNumber;
                                        validInput = true;
                                    }
                                    else
                                    {
                                        Console.WriteLine("Telefoonnummer moet 10 cijfers lang zijn.");
                                    }
                                }
                                break;

                            case "6":
                                // E-mail wijzigen
                                validInput = false;
                                while (!validInput)
                                {
                                    Console.Write("Nieuwe e-mail: ");
                                    CRC_email = ReadInputWithEscape() ?? "";
                                    if (CRC_email.Contains("@") && CRC_email.Contains("."))
                                    {
                                        validInput = true;
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
                                reservation.Remarks = ReadInputWithEscape() ?? "";
                                break;

                            default:
                                Console.WriteLine("Ongeldige optie. Probeer opnieuw.");
                                break;
                        }

                        // Save the updated reservation to the database
                        var reservationChanger = new ReservationChanger(DbFilePath);
                        reservationChanger.UpdateReservation(
                            CRC_numOfPeople, CRC_timeSlot, CRC_firstName, CRC_infix, CRC_lastName, CRC_phoneNumber, CRC_email, CRC_reservationDate, reservationId
                        );

                        Console.WriteLine("\nReservering succesvol veranderd!");
                        CRC_checker = false;
                    }
                }
                else
                {
                    Console.WriteLine("Je gaat terug naar het hoofdmenu.");
                    Menus.StartUp();
                }
            }
            else
            {
                Console.WriteLine("Reservering niet gevonden.");
                Menus.StartUp();
            }
        }
        else
        {
            Console.WriteLine("Ongeldige invoer. Voer een geldig reserverings-ID in.");
            Console.WriteLine("Je gaat terug naar het hoofdmenu.");
            Menus.StartUp();
        }

        Console.WriteLine("Druk op een toets om terug te keren naar het menu.");
        Console.ReadKey();
        Menus.StartUp();
    }

    private string ReadInputWithEscape()
    {
        var input = new StringBuilder();
        int cursorPosition = Console.CursorLeft;

        while (true)
        {
            var key = Console.ReadKey(intercept: true);
            if (key.Key == ConsoleKey.Enter)
            {
                Console.WriteLine();
                break;
            }
            if (key.Key == ConsoleKey.Escape)
            {
                Menus.StartUp();
                break;
            }
            if (key.Key == ConsoleKey.Backspace)
            {
                if (input.Length > 0 && Console.CursorLeft > cursorPosition + 0)
                {
                    input.Remove(input.Length - 1, 1);
                    Console.Write("\b \b");
                }
            }
            else if (char.IsWhiteSpace(key.KeyChar) && input.Length == 0)
            {
                // Ignore space at the beginning
                continue;
            }
            else
            {
                input.Append(key.KeyChar);
                Console.Write(key.KeyChar);
            }
        }
        return input.ToString();
    }
}
