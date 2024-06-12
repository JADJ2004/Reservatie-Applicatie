using System;
using System.Text.RegularExpressions;
using System.Text;

namespace ReservationApplication
{
public class Checker
{
    public bool Persoonsgegevens(string firstName = null)
    {
        if (firstName == null)
        {
            Console.WriteLine("\nGraag uw contactgegevens achterlaten:");
            Console.Write("Voornaam: ");
            firstName = ReadInputWithEscape() ?? "";
        }

        if (!string.IsNullOrWhiteSpace(firstName) && Regex.IsMatch(firstName, @"^[a-zA-Z\s]+$"))
        {
            return true;
        }
        else
        {
            Console.WriteLine("Ongeldige invoer. Probeer alleen letters te gebruiken.");
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
}



