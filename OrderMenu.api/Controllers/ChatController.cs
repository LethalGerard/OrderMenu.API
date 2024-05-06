using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;

namespace OrderMenu.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        readonly ChatService _kernel;
        public ChatController(ChatService kernel) => _kernel = kernel;        

        [HttpPost]
        public async Task<ActionResult<string>> PostChatResponse([FromBody] string input)
        {
               return Ok(await _kernel.ChatResponse(input));
        }
    }


}
