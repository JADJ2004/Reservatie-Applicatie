using System;
using System.Text.RegularExpressions;
using System.Text;

namespace ReservationApplication
{
    public class Checker
    {
        public bool AdminWachtwoord(string password = null)
        {
            if (password == null)
            {
                Console.WriteLine("\nDit is het log in scherm voor de beheerder.");
                Console.WriteLine("Wat is het wachtwoord?");
                password = Console.ReadLine();
            }

            return password == "1234YES!";
        }

        public bool Persoonsgegevens(out string firstName, string inputFirstName = null)
        {
            if (inputFirstName == null)
            {
                Console.WriteLine("\nGraag uw contactgegevens achterlaten:");
                Console.Write("Voornaam: ");
                inputFirstName = ReadInputWithEscape() ?? "";
            }

            if (!string.IsNullOrWhiteSpace(inputFirstName) && Regex.IsMatch(inputFirstName, @"^[a-zA-Z\s]+$"))
            {
                firstName = inputFirstName;
                return true;
            }
            else
            {
                Console.WriteLine("Ongeldige invoer. Probeer alleen letters te gebruiken.");
                firstName = string.Empty;
                return false;
            }
        }

        public bool Achternaam(out string lastName, string inputLastName = null)
        {
            if (inputLastName == null)
            {
                Console.Write("Achternaam: ");
                inputLastName = ReadInputWithEscape() ?? "";
            }

            if (!string.IsNullOrWhiteSpace(inputLastName) && Regex.IsMatch(inputLastName, @"^[a-zA-Z]+$"))
            {
                lastName = inputLastName;
                return true;
            }
            else
            {
                Console.WriteLine("Ongeldige invoer. Probeer alleen letters te gebruiken.");
                lastName = string.Empty;
                return false;
            }
        }

        public bool peopleChecker(out int numberOfPeople, string input = null)
        {
            if (input == null)
            {
                Console.Write("Aantal personen: ");
                input = ReadInputWithEscape();
            }

            if (int.TryParse(input, out numberOfPeople) && numberOfPeople > 0 && numberOfPeople <= 6)
            {
                return true;
            }
            else
            {
                Console.WriteLine("Ongeldig aantal personen.");
                numberOfPeople = 0;
                return false;
            }
        }

        public bool phoneChecker(out string phoneNumber, string inputPhoneNumber = null)
        {
            if (inputPhoneNumber == null)
            {
                Console.Write("Telefoonnummer: ");
                inputPhoneNumber = ReadInputWithEscape() ?? "";
            }

            if (inputPhoneNumber.Length == 10 && long.TryParse(inputPhoneNumber, out _))
            {
                phoneNumber = inputPhoneNumber;
                return true;
            }
            else
            {
                Console.WriteLine("Telefoonnummer moet 10 cijfers lang zijn.");
                phoneNumber = string.Empty;
                return false;
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
