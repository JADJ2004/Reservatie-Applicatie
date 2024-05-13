using System;

public class FoodMenu
{
    public void ToonMenu()
    {
        ToonVoorgerecht();
        ToonHoofdgerecht();
        ToonDessert();
    }

    public void ToonVoorgerecht()
    {
        Console.WriteLine("VOORGERECHTEN\n1. Ceasar Salad\n2. Italiaanse Ham met Galiameloen\n3. Franse slakken");
    }

    public void ToonHoofdgerecht()
    {
        Console.WriteLine("HOOFDGERECHTEN\n1. Hamburger\n2. Biefstuk\n3. Kip met rodewijnsaus");
    }

    public void ToonDessert()
    {
        Console.WriteLine("DESSERTS\n1. Tiramisu\n2. Cheesecake\n3. Cr�me Br�l�e\n4. Schepijs");
    }
}