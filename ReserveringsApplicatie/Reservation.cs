using System;

namespace ReservationApplication
{
    class TestAplicatie
    {
        private Database db = new Database();
        private ReservationSystem reservationSystem = new ReservationSystem();

        public void StartReservationSystem()
        {
            bool reservationConfirmed = false;

            while (!reservationConfirmed)
            {
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

                Console.WriteLine();
                Console.WriteLine("********* Reserveringsgegevens ************");
                Console.WriteLine();

                // Datum invoeren
                while (!dateChecker)
                {
                    Console.Write("Voer uw reserveringsdatum in (dd-MM-yyyy): ");
                    string dateString = Console.ReadLine() ?? "";
                    if (DateTime.TryParseExact(dateString, "dd-MM-yyyy", null, System.Globalization.DateTimeStyles.None, out reservationDate))
                    {
                        dateChecker = true;
<<<<<<< HEAD
=======
                    }
                    else
                    {
                        Console.WriteLine("Ongeldige invoer. Probeer: (dd-MM-yyyy)");
>>>>>>> 0386f2ee0b7f151f9574bc66a3b1efb4c4edc38d
                    }
                }

                // Aan het raam zitten?
                bool wantsWindow;
                while (true)
                {
                    Console.WriteLine("Wilt u aan het raam zitten? (ja/nee): ");
                    string input = Console.ReadLine()?.Trim().ToLower();
                    if (input == "ja")
                    {
                        wantsWindow = true;
                        break;
                    }
                    else if (input == "nee")
                    {
                        wantsWindow = false;
                        break;
                    }
                }

                // Aantal personen
                while (!peopleChecker)
                {
                    Console.Write("Aantal personen: ");
                    if (int.TryParse(Console.ReadLine(), out numberOfPeople) && numberOfPeople > 0)
                    {
                        peopleChecker = true;
                    }
                    else
                    {
                        Console.WriteLine("Ongeldig aantal personen.");
                    }
                }

                // Persoonsgegevens
                while (!firstNameChecker)
                {
                    Console.Write("Voornaam: ");
                    firstName = Console.ReadLine() ?? "";
                    if (!string.IsNullOrWhiteSpace(firstName))
                    {
                        firstNameChecker = true;
                    }
                    else
                    {
                        Console.WriteLine("Ongeldige invoer. Probeer alleen letters te gebruiken.");
                    }
                }

                // Tussenvoegsel
                Console.Write("Tussenvoegsel (indien van toepassing, anders druk op Enter): ");
                string infix = Console.ReadLine() ?? "";

                while (!lastNameChecker)
                {
                    Console.Write("Achternaam: ");
                    lastName = Console.ReadLine() ?? "";
                    if (!string.IsNullOrWhiteSpace(lastName))
                    {
                        lastNameChecker = true;
                    }
                    else
                    {
                        Console.WriteLine("Ongeldige invoer. Probeer alleen letters te gebruiken.");
                    }
                }

                while (!phoneNumberChecker)
                {
                    Console.Write("Telefoonnummer: ");
                    phoneNumber = Console.ReadLine() ?? "";
                    if (phoneNumber.Length == 10 && long.TryParse(phoneNumber, out _))
                    {
                        phoneNumberChecker = true;
                    }
                    Console.WriteLine("Een geldig telefoonnummer moet uit 10 cijfers bestaan.");
                }

                while (!emailChecker)
                {
                    Console.Write("E-mail: ");
                    email = Console.ReadLine() ?? "";
                    if (email.Contains("@") && email.Contains("."))
                    {
                        emailChecker = true;
                    }
                    else
                    {
                        Console.WriteLine("Ongeldige e-mail. Probeer een echt e-mail in te vullen.");
                    }
                }

                Console.WriteLine("Opmerkingen/verzoeken: ");
                remarks = Console.ReadLine() ?? "";

                // Reservering maken
                (int tableId, DateTime nextAvailableDate, string nextAvailableTimeSlot) = reservationSystem.ReserveTableForGroup(numberOfPeople, reservationDate, timeSlot);

                if (tableId == -1)
                {
                    Console.WriteLine($"Geen beschikbare tafels voor de opgegeven criteria op {reservationDate.ToShortDateString()}.");
                    Console.WriteLine($"De eerstvolgende beschikbare datum is: {nextAvailableDate.ToShortDateString()}.");
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
                    Console.WriteLine($"Tafelnummer: {tableId}");
                    Console.WriteLine($"Aantal personen: {numberOfPeople}");
                    Console.WriteLine($"Naam: {firstName} {infix} {lastName}");
                    Console.WriteLine($"Telefoonnummer: {phoneNumber}");
                    Console.WriteLine($"E-mail: {email}");
                    Console.WriteLine($"Opmerkingen/verzoeken: {remarks}");
                    Console.WriteLine();

                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write("Is deze informatie correct? (ja/nee): ");
                    string confirmation = Console.ReadLine()?.Trim().ToLower();
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
                            reservationConfirmed = true; // Beëindig de lus als de reservering is bevestigd
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine($"\nKon niet reserveren op {reservationDate.ToShortDateString()} tijdens {timeSlot}. Probeer op {suggestedDate.ToShortDateString()} tijdens {suggestedTimeSlot}.");
                            Console.ResetColor();
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
                        string wijzigOptie = Console.ReadLine()?.Trim();

                        switch (wijzigOptie)
                        {
                            case "1":
                                // Datum wijzigen
                                dateChecker = false;
                                while (!dateChecker)
                                {
                                    Console.Write("Voer uw nieuwe reserveringsdatum in (dd-MM-yyyy): ");
                                    dateString = Console.ReadLine() ?? "";
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
                                    if (int.TryParse(Console.ReadLine(), out int slot) && slot >= 1 && slot <= 3)
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
                                    if (int.TryParse(Console.ReadLine(), out numberOfPeople) && numberOfPeople > 0 && numberOfPeople <= 6)
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
                                    firstName = Console.ReadLine() ?? "";
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
                                infix = Console.ReadLine() ?? "";

                                lastNameChecker = false;
                                while (!lastNameChecker)
                                {
                                    Console.Write("Nieuwe achternaam: ");
                                    lastName = Console.ReadLine() ?? "";
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
                                    phoneNumber = Console.ReadLine() ?? "";
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
                                    email = Console.ReadLine() ?? "";
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
                                remarks = Console.ReadLine() ?? "";
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
                            continue;
                        }
                    }
                    else
                    {
                        Console.WriteLine("Ongeldige invoer. Probeer opnieuw.");
                    }
                }
            }
        }
    }
}