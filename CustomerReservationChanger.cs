using System;
using ReservationApplication;
using System.Text;

public class CustomerReservationChanger
{
    private const string ConnectionString = @"Data Source=C:\Users\jibbe\Documents\sprint4demo\Mydatabase.db";
    private Database db;

    public CustomerReservationChanger()
    {
        db = new Database();
    }

    public void ChangeReservationInformation(int reservationId)
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
                CRC_date = ReadInputWithEscape() ?? "";
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

            while (!CRC_people_checker)
            {
                Console.Write("Aantal personen: ");
                string input = ReadInputWithEscape();
                if (int.TryParse(input, out CRC_numOfPeople) && CRC_numOfPeople > 0 && CRC_numOfPeople < 48)
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
                CRC_name = ReadInputWithEscape() ?? "";
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
            CRC_addition = ReadInputWithEscape() ?? "";

            while (!CRC_last_name_checker)
            {
                Console.Write("Achternaam: ");
                CRC_surname = ReadInputWithEscape() ?? "";
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
                CRC_phoneNumber = ReadInputWithEscape() ?? "";
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
                CRC_email = ReadInputWithEscape() ?? "";
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
            string CRC_comments = ReadInputWithEscape() ?? "";

            DateTime CRC_reservationDate;
            if (DateTime.TryParseExact(CRC_date, "dd-MM-yyyy", null, System.Globalization.DateTimeStyles.None, out CRC_reservationDate))
            {
                var reservationChanger = new ReservationChanger(ConnectionString);
                reservationChanger.UpdateReservation(CRC_numOfPeople, CRC_name, CRC_addition, CRC_surname, int.Parse(CRC_phoneNumber), CRC_email, CRC_reservationDate, reservationId);
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
            string CRC_confirmation = ReadInputWithEscape()?.Trim().ToLower();
            if (CRC_confirmation == "ja")
            {
                CRC_checker = true;
                Console.WriteLine("\nReservering succesvol veranderd!");
            }
            else
            {
                Console.WriteLine("Reservering niet veranderd. Probeer opnieuw.");
            }
        }

        Console.WriteLine("\nDank u wel! We hopen u gauw te zien bij YES!");
    }

    public void ReservationChanger()
    {
        Console.WriteLine("Voer uw reserverings-ID in:");
        string input = ReadInputWithEscape();
        if (int.TryParse(input, out int reservationId))
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
                    ChangeReservationInformation(reservationId);
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

