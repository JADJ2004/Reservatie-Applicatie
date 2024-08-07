using System;
using System.Text.RegularExpressions;
using System.Text;

namespace ReservationApplication
{
    class TestApplicatie
    {
        private Database db = new Database();
        private ReservationSystem reservationSystem = new ReservationSystem();

        public void StartReservationSystem()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("************************************************************************************************/");
            Console.WriteLine("█▀█ █▀▀ █▀ █▀▀ █▀█ █░█ █▀▀ █▀▀ █▀█   █▀▀ █▀▀ █▄░█   ▀█▀ ▄▀█ █▀▀ █▀▀ █░░");
            Console.WriteLine("█▀▄ ██▄ ▄█ ██▄ █▀▄ ▀▄▀ ██▄ ██▄ █▀▄   ██▄ ██▄ █░▀█   ░█░ █▀█ █▀░ ██▄ █▄▄");
            Console.WriteLine("************************************************************************************************/");
            Console.ResetColor();
            Console.WriteLine();

            bool reservationConfirmed = false;

            while (!reservationConfirmed)
            {
                Checker check = new Checker();
                bool dateChecker = false;
                bool firstNameChecker = false;
                bool lastNameChecker = false;
                bool phoneNumberChecker = false;
                bool peopleChecker = false;
                bool emailChecker = false;

                string dateString = "";
                DateTime reservationDate = DateTime.MinValue;
                int numberOfPeople = 0;
                string firstName = "";
                string lastName = "";
                string phoneNumber = "";
                string email = "";
                string infix = "";
                string remarks = "";

                DateTime today = DateTime.Now.Date;

                // Datum invoeren
                while (!dateChecker)
                {
                    Console.Write("Voer uw reserveringsdatum in (dd-MM-yyyy): ");
                    dateString = ReadInputWithEscape() ?? "";
                    if (DateTime.TryParseExact(dateString, "dd-MM-yyyy", null, System.Globalization.DateTimeStyles.None, out reservationDate) && today < reservationDate)
                    {
                        dateChecker = true;
                    }
                    else if(!DateTime.TryParseExact(dateString, "dd-MM-yyyy", null, System.Globalization.DateTimeStyles.None, out reservationDate))
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Ongeldige invoer. Probeer: (dd-MM-yyyy)");
                        Console.WriteLine("");
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Je kan geen tafel reserveren voor een datum in het verleden");
                        Console.WriteLine("");
                        Console.ResetColor();
                    }
                }


                // Tijdslot selecteren
                string[] timeSlots = { "18:00-19:59", "20:00-21:59", "22:00-23:59" };
                string timeSlot = "";
                Console.WriteLine("Selecteer een tijdslot:");
                for (int i = 0; i < timeSlots.Length; i++)
                {
                    Console.WriteLine($"{i + 1}. {timeSlots[i]}");
                }
                while (true)
                {
                    Console.Write("Kies een optie (1-3): ");
                    string slotInput = ReadInputWithEscape();
                    if (int.TryParse(slotInput, out int slot) && slot >= 1 && slot <= 3)
                    {
                        timeSlot = timeSlots[slot - 1];
                        break;
                    }
                    Console.WriteLine("Ongeldige invoer. Kies een optie tussen 1 en 3.");
                }

                // Aantal personen
                while (!peopleChecker)
                {
                    peopleChecker = check.peopleChecker(out numberOfPeople);
                }

                // Persoonsgegevens
                while (!firstNameChecker)
                {
                    firstNameChecker = check.Persoonsgegevens(out firstName);
                }

                Console.Write("Tussenvoegsel (indien van toepassing, anders druk op Enter): ");
                infix = ReadInputWithEscape() ?? "";

                while (!lastNameChecker)
                {
                    lastNameChecker = check.Achternaam(out lastName);
                }

                while (!phoneNumberChecker)
                {
                    phoneNumberChecker = check.phoneChecker(out phoneNumber);
                }

                while (!emailChecker)
                {
                    Console.Write("E-mail: ");
                    email = ReadInputWithEscape() ?? "";
                    if (email.Contains("@") && email.Contains("."))
                    {
                        emailChecker = true;
                    }
                    else
                    {
                        Console.WriteLine("Ongeldige e-mail. Probeer een echt e-mail in te vullen.");
                    }
                }

                Console.WriteLine("Opmerkingen:");
                remarks = ReadInputWithEscape() ?? "";

                // Reservering maken
                int tableId;
                DateTime nextAvailableDate;
                string nextAvailableTimeSlot;

                (tableId, nextAvailableDate, nextAvailableTimeSlot) = reservationSystem.ReserveTableForGroup(numberOfPeople, reservationDate, timeSlot);

                if (tableId == -1)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"\nGeen beschikbare tafels voor de opgegeven criteria op {reservationDate.ToShortDateString()} tijdens {timeSlot}.");
                    Console.WriteLine($"De eerstvolgende beschikbare datum en tijdslot zijn: {nextAvailableDate.ToShortDateString()} {nextAvailableTimeSlot}.");
                    Console.ResetColor();
                    continue;
                }

                bool correctInformation = false;

                while (!correctInformation)
                {
                    // Reserveringsgegevens bevestigen
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("*******************************************");
                    Console.WriteLine("* Reserveringsgegevens bevestigen          *");
                    Console.WriteLine("*******************************************");
                    Console.ResetColor();
                    Console.WriteLine($"Datum: {reservationDate.ToShortDateString()}");
                    Console.WriteLine($"Tijdslot: {timeSlot}");
                    Console.WriteLine($"Aantal personen: {numberOfPeople}");
                    Console.WriteLine($"Naam: {firstName} {infix} {lastName}");
                    Console.WriteLine($"Telefoonnummer: {phoneNumber}");
                    Console.WriteLine($"E-mail: {email}");
                    Console.WriteLine($"Opmerkingen/verzoeken: {remarks}");
                    Console.WriteLine();

                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write("Is deze informatie correct? (ja/nee): ");
                    string confirmation = ReadInputWithEscape()?.Trim().ToLower();
                    Console.ResetColor();

                    if (confirmation == "ja")
                    {
                        var (success, suggestedDate, suggestedTimeSlot, ReservationId) = db.AddReservation(numberOfPeople, firstName, infix, lastName, phoneNumber, email, reservationDate, timeSlot, tableId, remarks);
                        if (success)
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("\nUw reservering is succesvol verwerkt.");
                            Console.ResetColor();
                            reservationSystem.SendEmail(email, reservationDate, timeSlot, firstName, numberOfPeople, ReservationId);
                            correctInformation = true;    
                            reservationConfirmed = true; // Beëindig de lus als de reservering is bevestigd
                            Console.ResetColor();
                            Menus.StartUp();
                            
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine($"\nKon niet reserveren op {reservationDate.ToShortDateString()} tijdens {timeSlot}. Probeer op {suggestedDate.ToShortDateString()} tijdens {suggestedTimeSlot}.");
                            Console.ResetColor();
                            break;
                        }
                    }
                    else if (confirmation == "nee")
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

                        switch (wijzigOptie)
                        {
                            case "1":
                                // Datum wijzigen
                                dateChecker = false;
                                while (!dateChecker)
                                {
                                    Console.Write("Voer uw nieuwe reserveringsdatum in (dd-MM-yyyy): ");
                                    dateString = ReadInputWithEscape() ?? "";
                                    if (DateTime.TryParseExact(dateString, "dd-MM-yyyy", null, System.Globalization.DateTimeStyles.None, out reservationDate))
                                    {
                                        dateChecker = true;
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
                                for (int i = 0; i < timeSlots.Length; i++)
                                {
                                    Console.WriteLine($"{i + 1}. {timeSlots[i]}");
                                }
                                while (true)
                                {
                                    Console.Write("Kies een optie (1-3): ");
                                    string slotInput = ReadInputWithEscape();
                                    if (int.TryParse(slotInput, out int slot) && slot >= 1 && slot <= 3)
                                    {
                                        timeSlot = timeSlots[slot - 1];
                                        break;
                                    }
                                    Console.WriteLine("Ongeldige invoer. Kies een optie tussen 1 en 3.");
                                }
                                break;

                            case "3":
                                // Aantal personen wijzigen
                                peopleChecker = false;
                                while (!peopleChecker)
                                {
                                    Console.Write("Nieuw aantal personen: ");
                                    string input = ReadInputWithEscape();
                                    if (int.TryParse(input, out numberOfPeople) && numberOfPeople > 0 && numberOfPeople <= 6)
                                    {
                                        peopleChecker = true;
                                    }
                                    else
                                    {
                                        Console.WriteLine("Ongeldig aantal personen.");
                                    }
                                }
                                break;

                            case "4":
                                // Naam wijzigen
                                firstNameChecker = false;
                                while (!firstNameChecker)
                                {
                                    Console.Write("Nieuwe voornaam: ");
                                    firstName = ReadInputWithEscape() ?? "";
                                    if (!string.IsNullOrWhiteSpace(firstName) && Regex.IsMatch(firstName, @"^[a-zA-Z]+$"))
                                    {
                                        firstNameChecker = true;
                                    }
                                    else
                                    {
                                        Console.WriteLine("Ongeldige invoer. Probeer alleen letters te gebruiken.");
                                    }
                                }

                                Console.Write("Nieuw tussenvoegsel (indien van toepassing, anders druk op Enter): ");
                                infix = ReadInputWithEscape() ?? "";

                                lastNameChecker = false;
                                while (!lastNameChecker)
                                {
                                    Console.Write("Nieuwe achternaam: ");
                                    lastName = ReadInputWithEscape() ?? "";
                                    if (!string.IsNullOrWhiteSpace(lastName) && Regex.IsMatch(lastName, @"^[a-zA-Z]+$"))
                                    {
                                        lastNameChecker = true;
                                    }
                                    else
                                    {
                                        Console.WriteLine("Ongeldige invoer. Probeer alleen letters te gebruiken.");
                                    }
                                }
                                break;

                            case "5":
                                // Telefoonnummer wijzigen
                                phoneNumberChecker = false;
                                while (!phoneNumberChecker)
                                {
                                    Console.Write("Nieuw telefoonnummer: ");
                                    phoneNumber = ReadInputWithEscape() ?? "";
                                    if (phoneNumber.Length == 10 && long.TryParse(phoneNumber, out _))
                                    {
                                        phoneNumberChecker = true;
                                    }
                                    else
                                    {
                                        Console.WriteLine("Telefoonnummer moet 10 cijfers lang zijn.");
                                    }
                                }
                                break;

                            case "6":
                                // E-mail wijzigen
                                emailChecker = false;
                                while (!emailChecker)
                                {
                                    Console.Write("Nieuwe e-mail: ");
                                    email = ReadInputWithEscape() ?? "";
                                    if (email.Contains("@") && email.Contains("."))
                                    {
                                        emailChecker = true;
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
                                remarks = ReadInputWithEscape() ?? "";
                                break;

                            default:
                                Console.WriteLine("Ongeldige optie. Probeer opnieuw.");
                                break;
                        }

                        // Controleer de beschikbaarheid opnieuw na wijzigingen
                        (tableId, nextAvailableDate, nextAvailableTimeSlot) = reservationSystem.ReserveTableForGroup(numberOfPeople, reservationDate, timeSlot);

                        if (tableId == -1)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine($"\nGeen beschikbare tafels voor de opgegeven criteria op {reservationDate.ToShortDateString()} tijdens {timeSlot}.");
                            Console.WriteLine($"De eerstvolgende beschikbare datum en tijdslot zijn: {nextAvailableDate.ToShortDateString()} {nextAvailableTimeSlot}.");
                            Console.ResetColor();
                            break;
                        }
                    }
                    else
                    {
                        Console.WriteLine("Ongeldige invoer. Probeer opnieuw.");
                    }
                }
            }
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
                    if (input.Length > 0 && Console.CursorLeft > cursorPosition)
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
}
