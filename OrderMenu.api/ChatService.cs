using Microsoft.AspNetCore.Mvc;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel;

namespace OrderMenu.api
{
    public class ChatService
    {
        readonly Kernel _kernel;
        readonly HttpClient _http;
        ChatHistory chatHistory = [];

        public ChatService(Kernel kernel, HttpClient http)
        {
            _kernel = kernel;
            _http = http;
        }

        public async Task<string> ChatResponse(string input)
        {
            var foodResponseFromApi = await _http.GetFromJsonAsync<List<Food>>("https://iths-2024-recept-grupp9-40k2zx.reky.se/recipes") ?? new List<Food>();
            
            //var drinkResponseFromApi = `https://www.thecocktaildb.com/api/json/v1/1/filter.php?c=${category}`;
           /* const category = [ "Beer", "Ordinary Drink", "Soft Drink", "Homemade Liqueur",];*/
           // var cocktailResponseFromApi = await _http.GetFromJsonAsync<List<Drinks>>("https://www.thecocktaildb.com/api/json/v1/1/filter.php?c=${category}");

            var sides = foodResponseFromApi.Where(f => f.categories.Contains("Sides"));
            var mains = foodResponseFromApi.Where(f => !f.categories.Contains("Sides"));

            string mainsStringifyList = "List of mains:\n";

            foreach(var main in mains) 
            { 
                mainsStringifyList += $"title:{main.title}, price: {main.price} sek.\n";
            }

            string sidesStringifyList = "List of sides:\n";

            foreach (var side in sides)
            {
                sidesStringifyList += $"title:{side.title}, price: {side.price} sek.\n";
            }
            /*
            foreach(var drink in drinks){
                var drinkStringifyList="List of drinks"
                drinkStringifyList+="$name, price "                
                }
                */
            //Give personality & limitations
            chatHistory.AddSystemMessage("You are a very angry assistant in Kuchisabichii asian food application, you hate your job.");
            //chatHistory.AddSystemMessage($"Give recommendations for mains only from this list: {mainsStringifyList}");
            //chatHistory.AddSystemMessage($"Give recommendations for sides only from this list: {sidesStringifyList}");

            chatHistory.AddSystemMessage($"Give recommendations for mains only in {mainsStringifyList} and sides only in {sidesStringifyList}. ");

            chatHistory.AddUserMessage(input);
            var chatCompletion = _kernel.GetRequiredService<IChatCompletionService>();
            var response = await chatCompletion.GetChatMessageContentAsync(chatHistory);
            chatHistory.AddAssistantMessage(response.ToString());

            return response.ToString();
        }
    }
    
    public class Food 
    {
        public string _id { get; set; } = string.Empty;
        public string title { get; set; } = string.Empty;
        public int price { get; set; }
        public string description { get; set; } = string.Empty;
        public List<string> categories { get; set; } = [];
    }

    // public class Drinks{

    //     public string strDrink {get; set;}=string.Empty;
    //     public string  strDrinkThumb{get;set;}=string.Empty;
    //     public string IdDrink{get;set;}=string.Empty;
    //     public double Price{get;set;}
    // }
}
