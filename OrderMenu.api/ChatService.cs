using Microsoft.AspNetCore.Mvc;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel;
using OrderMenu.api.Plugins;
using Microsoft.SemanticKernel.Connectors.OpenAI;

namespace OrderMenu.api
{
    public class ChatService
    {
        readonly Kernel _kernel;
        readonly HttpClient _http;
        ChatHistory _chatHistory = [];

        public ChatService(Kernel kernel, HttpClient http)
        {
            _kernel = kernel;
            _http = http;
            _kernel.ImportPluginFromType<ItemPlugin>();
            _chatHistory.AddSystemMessage(File.ReadAllText("AiBotPrompt.txt"));
        }

        public async Task<string> ChatResponse(string input)
        {
            var executionSettings = new OpenAIPromptExecutionSettings()
            {
                ToolCallBehavior = ToolCallBehavior.AutoInvokeKernelFunctions
            };

            

            //_chatHistory.AddSystemMessage($"Give recommendations for mains only in {mainsStringifyList} and sides only in {sidesStringifyList}. ");

            _chatHistory.AddUserMessage(input);
            var chatCompletion = _kernel.GetRequiredService<IChatCompletionService>();
            var response = await chatCompletion.GetChatMessageContentAsync(_chatHistory, executionSettings, _kernel);
            _chatHistory.AddAssistantMessage(response.ToString());


            return response.Content;
        }
    }
    


    // public class Drinks{

    //     public string strDrink {get; set;}=string.Empty;
    //     public string  strDrinkThumb{get;set;}=string.Empty;
    //     public string IdDrink{get;set;}=string.Empty;
    //     public double Price{get;set;}
    // }
}
