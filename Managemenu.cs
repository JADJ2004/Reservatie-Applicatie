using System;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;

namespace ReservationApplication
{
    public class MenuManager
    {
        private readonly Database database;

        public MenuManager(Database database)
        {
            this.database = database;
        }

        private const string ConnectionString = @"Data Source = .\Mydatabase.db";

        public void AddMenuItem(string category, string name, double price)
        {
            // Voeg de nieuwe menu-item toe aan de database
            try
            {
                using (var connection = new SqliteConnection(ConnectionString))
                {
                    connection.Open();
                    var sqlQuery = @"
                        INSERT INTO MenuItems (Category, Name, Price)
                        VALUES (@Category, @Name, @Price)";
                    using (var command = new SqliteCommand(sqlQuery, connection))
                    {
                        command.Parameters.AddWithValue("@Category", category);
                        command.Parameters.AddWithValue("@Name", name);
                        command.Parameters.AddWithValue("@Price", price);
                        command.ExecuteNonQuery();
                    }
                }

                Console.WriteLine("Menu-item succesvol toegevoegd.");
                Console.WriteLine("Terug naar het managermenu...");
                Console.ReadKey(); // Wacht tot de gebruiker op een toets drukt
                Console.Clear();
                ManagerMenu.StartUp(); // Start het managermenu opnieuw
            }
            catch (Exception ex)
            {
                Console.WriteLine("Fout bij het toevoegen van het menu-item: " + ex.Message);
            }
        }

        public void RemoveMenuItem(string category, string name)
        {
            // Verwijder het menu-item uit de database op basis van categorie en naam
            try
            {
                using (var connection = new SqliteConnection(ConnectionString))
                {
                    connection.Open();
                    var sqlQuery = @"
                        DELETE FROM MenuItems 
                        WHERE Category = @Category AND Name = @Name";
                    using (var command = new SqliteCommand(sqlQuery, connection))
                    {
                        command.Parameters.AddWithValue("@Category", category);
                        command.Parameters.AddWithValue("@Name", name);
                        command.ExecuteNonQuery();
                    }
                }

                Console.WriteLine("Menu-item succesvol verwijderd.");
                Console.WriteLine("Terug naar het managermenu...");
                Console.ReadKey(); // Wacht tot de gebruiker op een toets drukt
                Console.Clear();
                ManagerMenu.StartUp(); // Start het managermenu opnieuw
            }
            catch (Exception ex)
            {
                Console.WriteLine("Fout bij het verwijderen van het menu-item: " + ex.Message);
            }
        }
    }
}
