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
            Console.WriteLine("");
            Console.WriteLine("VOORGERECHTEN");
            PrintCategory(0);

            Console.WriteLine("HOOFDGERECHTEN");
            PrintCategory(1);

            Console.WriteLine("DESSERTS");
            PrintCategory(2);
            Console.WriteLine("");
            
            Menus.StartUp();
        }

        private void PrintCategory(int categoryIndex)
        {
            foreach (Food foodItem in Foods[categoryIndex])
            {
                Console.WriteLine($"{foodItem.Name} - {foodItem.Price}");
            }
            Console.WriteLine(); 
        }

        public void EditFood(int categoryIndex, int foodIndex, string newName, double newPrice)
        {
            if (categoryIndex >= 0 && categoryIndex < Foods.Count && foodIndex >= 0 && foodIndex < Foods[categoryIndex].Count)
            {
                Foods[categoryIndex][foodIndex].Name = newName;
                Foods[categoryIndex][foodIndex].Price = newPrice;
                Console.WriteLine("Food item updated successfully!");
            }
            else
            {
                Console.WriteLine("Invalid category or food index!");
            }
        }

        public List<string> GetMenu()
        {
            List<string> menu = new List<string>();

            foreach (var category in Foods)
            {
                foreach (var foodItem in category)
                {
                    string menuItem = foodItem.Name;
                    menu.Add(menuItem);
                }
            }

            return menu;
        }
    }
} 