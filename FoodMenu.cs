using System;
using System.Collections.Generic;
using System.Text;

namespace ReservationApplication
{
    class FoodMenu
    {
        public static List<List<Food>> Foods = new List<List<Food>>
        {
            new List<Food>
            {
                new Food("Salade", 8.99),
                new Food("Italiaanse Ham", 10.99),
                new Food("Franse slakken", 12.99)
            },
            new List<Food>
            {
                new Food("Hamburger", 9.99),
                new Food("Biefstuk", 15.99),
                new Food("Kip met rodeweijnsaus", 12.49)
            },
            new List<Food>
            {
                new Food("Tiramisu", 7.49),
                new Food("Cheesecake", 6.99),
                new Food("Creme Brulee", 8.49),
                new Food("Schepijs", 5.99)
            }
        };

        public void ToonMenu()
        {
            Console.Clear();
            Console.WriteLine("");
            Console.WriteLine("VOORGERECHTEN");
            PrintCategory(0);

            Console.WriteLine("HOOFDGERECHTEN");
            PrintCategory(1);

            Console.WriteLine("DESSERTS");
            PrintCategory(2);
            Console.WriteLine("");
            
        }

        private void PrintCategory(int categoryIndex)
        {
            foreach (Food foodItem in Foods[categoryIndex])
            {
                Console.WriteLine($"{foodItem.Name} - {foodItem.Price}");
            }
            Console.WriteLine(); 
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