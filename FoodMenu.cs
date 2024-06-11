using System;
using System.Collections.Generic;
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

    public class FoodMenu
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
            Thread.Sleep(3000);
            Console.WriteLine("");
            Menus.StartUp();
        }


        private void PrintCategory(string category)
        {
            List<MenuItem> menuItems = database.GetMenuItemsByCategory(category); // Hier is de correctie
            foreach (MenuItem menuItem in menuItems)
            {
                Console.WriteLine($"{menuItem.Name} - {menuItem.Price}");
            }
            Console.WriteLine();
        }
    }
}
