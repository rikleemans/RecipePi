using System;
using System.Linq;

namespace ConstOverload
{
    class Recipe
    {
        string recipe;
        string[] ingredients;
        
        public Recipe() // Constructor with default values
        {
            recipe = "Pizza";
            ingredients = new string[] { "deeg", "kaas" };


            Console.WriteLine("Eerste recept: {0} met de ingredienten: {1}", recipe, string.Join(',', ingredients));
        }

        
        public Recipe(string recipe1, string[] ingredients1) // Constructor with overloaded parameters.
        {
            recipe = recipe1;
            ingredients = ingredients1;
            Console.WriteLine("Tweede recept: {0} met de ingredienten: {1}", recipe, string.Join(',', ingredients));
        }
    }

    class Overloading
    {
        static void Main(string[] args)
        {
            Recipe recipe1 = new Recipe(); // call default constructor
            Recipe recipe2 = new Recipe("Lasagne", new string[] { "kaas", "tomaat" }); // call constructor with overloaded parameters
            Console.ReadLine();
        }
    }
}
