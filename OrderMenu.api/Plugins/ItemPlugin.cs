using Microsoft.SemanticKernel;
using System.ComponentModel;
using static System.Net.WebRequestMethods;

namespace OrderMenu.api.Plugins;

public class ItemPlugin
{
    [KernelFunction, Description("returns a list of drinks")] 
    public string GetDrinksPlugin()
    {
        var result = System.IO.File.ReadAllText("Drinks.txt");
        return result;
    }

    [KernelFunction, Description("returns a list of food items")]
    public async Task<string> GetFoodPlugin()
    {
        var _http = new HttpClient();

        var foodResponseFromApi = await _http.GetFromJsonAsync<List<Food>>("https://iths-2024-recept-grupp9-40k2zx.reky.se/recipes") ?? new List<Food>();

        //var drinkResponseFromApi = `https://www.thecocktaildb.com/api/json/v1/1/filter.php?c=${category}`;
        /* const category = [ "Beer", "Ordinary Drink", "Soft Drink", "Homemade Liqueur",];*/
        // var cocktailResponseFromApi = await _http.GetFromJsonAsync<List<Drinks>>("https://www.thecocktaildb.com/api/json/v1/1/filter.php?c=${category}");

        var sides = foodResponseFromApi.Where(f => f.categories.Contains("Sides"));
        var mains = foodResponseFromApi.Where(f => !f.categories.Contains("Sides"));

        string mainsStringifyList = "List of mains:\n";

        foreach (var main in mains)
        {
            mainsStringifyList += $"title:{main.title}, price: {main.price} sek.\n";
        }

        string sidesStringifyList = "List of sides:\n";

        foreach (var side in sides)
        {
            sidesStringifyList += $"title:{side.title}, price: {side.price} sek.\n";
        }

        return $"{mainsStringifyList} - {sidesStringifyList}";

    }

    public class Food
    {
        public string _id { get; set; } = string.Empty;
        public string title { get; set; } = string.Empty;
        public int price { get; set; }
        public string description { get; set; } = string.Empty;
        public List<string> categories { get; set; } = [];

    }
}
