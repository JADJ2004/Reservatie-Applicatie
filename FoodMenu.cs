using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Data.Sqlite;

namespace ReservationApplication
{
    public class MenuItem
    {
        public int MenuItemId { get; set; }
        public string Category { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }

        public MenuItem(string category, string name, double price)
        {
            Category = category;
            Name = name;
            Price = price;
        }
    }

    public class FoodMenu : IFoodMenu
    {   
        private readonly Database database;

        // Constructor toegevoegd
        public FoodMenu(Database database)
        {
            this.database = database;
        }

        public void ToonMenu()
        {
            Console.Clear();
            Console.WriteLine("VOORGERECHTEN");
            PrintCategory("VOORGERECHTEN");

            Console.WriteLine("HOOFDGERECHTEN");
            PrintCategory("HOOFDGERECHTEN");

            Console.WriteLine("DESSERTS");
            PrintCategory("DESSERTS");
            Console.WriteLine("Druk op ESC om terug te gaan");
            string input = ReadInputWithEscape();
        }


        public void PrintCategory(string category)
        {
            List<MenuItem> menuItems = database.GetMenuItemsByCategory(category); // Hier is de correctie
            foreach (MenuItem menuItem in menuItems)
            {
                Console.WriteLine($"{menuItem.Name} - {menuItem.Price}");
            }
            Console.WriteLine();
        }
        private static string ReadInputWithEscape()
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
    

