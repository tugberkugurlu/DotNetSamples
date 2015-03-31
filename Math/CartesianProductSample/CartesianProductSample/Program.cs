using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CartesianProductSample
{
    class Program
    {
        static void Main(string[] args)
        {
            Recipe recipe2 = new Recipe();
            IngredientGroup ingredientGroup3 = new IngredientGroup();
            IngredientGroup ingredientGroup4 = new IngredientGroup();
            IngredientGroup ingredientGroup5 = new IngredientGroup();
            IngredientGroup ingredientGroup6 = new IngredientGroup();

            recipe2.Name = "Recipe2";
            ingredientGroup3.Ingredients.Add(new Food { Name = "Food8", Category = "Categor8" });
            ingredientGroup3.Ingredients.Add(new Food { Name = "Food9", Category = "Categor9" });

            ingredientGroup4.Ingredients.Add(new Food { Name = "Food5", Category = "Categor5" });
            ingredientGroup4.Ingredients.Add(new Food { Name = "Food10", Category = "Categor10" });
            ingredientGroup4.Ingredients.Add(new Food { Name = "Food11", Category = "Category11" });

            ingredientGroup5.Ingredients.Add(new Food { Name = "Food3", Category = "Categor3" });
            ingredientGroup5.Ingredients.Add(new Food { Name = "Food4", Category = "Categor4" });

            ingredientGroup6.Ingredients.Add(new Food { Name = "Food5", Category = "Categor5" });
            ingredientGroup6.Ingredients.Add(new Food { Name = "Food10", Category = "Categor10" });
            ingredientGroup6.Ingredients.Add(new Food { Name = "Food11", Category = "Category11" });

            recipe2.IngredientGroups.Add(ingredientGroup3);
            recipe2.IngredientGroups.Add(ingredientGroup4);
            recipe2.IngredientGroups.Add(ingredientGroup5);
            recipe2.IngredientGroups.Add(ingredientGroup6);

            var recipes = new[] { recipe2 };
            WriteCombinationsByKnowingTheNumber(recipes);
            Console.WriteLine("==============================================");
            Console.WriteLine(Environment.NewLine);
            Console.WriteLine("==============================================");
            WriteCombinationsWithoutKnowingTheNumber(recipes);

            Console.ReadLine();
        }

        static void WriteCombinationsByKnowingTheNumber(IEnumerable<Recipe> recipes)
        {
            List<string> results = new List<string>();
            foreach (var rcp in recipes)
            {
                var group1 = rcp.IngredientGroups.ElementAt(0);
                var group2 = rcp.IngredientGroups.ElementAt(1);
                var group3 = rcp.IngredientGroups.ElementAt(2);

                foreach (var item1 in group1.Ingredients)
                    foreach (var item2 in group2.Ingredients)
                        foreach (var item3 in group3.Ingredients)
                        {
                            results.Add(string.Format("{0}, {1}, {2}", item1.Name, item2.Name, item3.Name));
                        }
            }

            foreach (string result in results)
            {
                Console.WriteLine(result);
            }
        }

        static void WriteCombinationsWithoutKnowingTheNumber(IEnumerable<Recipe> recipes)
        {
            IEnumerable<IEnumerable<string>> ingredientNames = recipes.SelectMany(recipe =>
                recipe.IngredientGroups.Select(ingredientGroup =>
                    ingredientGroup.Ingredients.Select(ingredient =>
                        ingredient.Name)));

            IEnumerable<string> results = from combination in ingredientNames.CombinationOf() 
                                         select string.Join(", ", combination);

            Console.WriteLine(results.Count());
            foreach (string result in results)
            {
                Console.WriteLine(result);
            }
        }
    }

    public static class EnumerableExtensions
    {
        public static IEnumerable<IEnumerable<T>> CombinationOf<T>(this IEnumerable<IEnumerable<T>> sets)
        {
            IEnumerable<IEnumerable<T>> tempSets = sets as IEnumerable<T>[] ?? sets.ToArray();
            IEnumerable<IEnumerable<T>> combinations = tempSets
                .ElementAt(0)
                .Select(value => new List<T> {value}).ToList();

            return tempSets.Skip(1).Aggregate(combinations, AddSet);
        }

        public static IEnumerable<IEnumerable<T>> AddSet<T>(this IEnumerable<IEnumerable<T>> combinations, IEnumerable<T> set)
        {
            IEnumerable<IEnumerable<T>> tempCombinations = combinations as IEnumerable<T>[] ?? combinations.ToArray();
            IEnumerable<List<T>> result = from value in set
                         from combination in tempCombinations
                         select new List<T>(combination) {value};

            return result;
        }
    }

    public class Food
    {
        public Food()
        {
            NutritionalValues = new Collection<FoodNutritionalValue>();
        }

        public string Name { get; set; }
        public string Category { get; set; }

        public ICollection<FoodNutritionalValue> NutritionalValues { get; set; }
    }

    public class FoodNutritionalValue
    {
        public string Type { get; set; }
        public decimal Value { get; set; }
    }

    public class Recipe
    {
        public Recipe()
        {
            IngredientGroups = new Collection<IngredientGroup>();
        }

        public string Name { get; set; }
        public ICollection<IngredientGroup> IngredientGroups { get; set; }
    }

    public class IngredientGroup
    {
        public IngredientGroup()
        {
            Ingredients = new Collection<Food>();
        }

        public ICollection<Food> Ingredients { get; set; }
    }
}
