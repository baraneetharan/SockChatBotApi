using Microsoft.AspNetCore.Mvc;
using DotNetEnv;
using Azure;
using Azure.AI.Inference;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SockChatBotApi.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SockChatBotApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ChatController : ControllerBase
    {
        private readonly IChatClient _chatClient;
        private readonly Cart _cart;
        private readonly ChatOptions _chatOptions;

        public ChatController(IChatClient chatClient, Cart cart, ChatOptions chatOptions)
        {
            _chatClient = chatClient;
            _cart = cart;
            _chatOptions = chatOptions;
        }

        [HttpPost("chat")]
        public async Task<ActionResult<string>> Chat(string userMessage)
        {
            var messages = new List<ChatMessage>
            {
                new(Microsoft.Extensions.AI.ChatRole.System, "You answer any question, Hey there, I'm Lumina, your friendly lighting assistant! I can help you with all your lighting needs. You can ask me to turn on the living room light, get the status of the kitchen light, turn off all the lights, add a new light to the bedroom, or delete the light in the hallway. Just let me know what you need and I'll do my best to help!"),
                new(Microsoft.Extensions.AI.ChatRole.User, userMessage)
            };

            // Log User message
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"User -> {userMessage}");

            var response = await _chatClient.CompleteAsync(messages, _chatOptions);

            // Log Assistant message
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Assistant -> {response.Message.Text}");

            // // Log the function invoked, if any 
            // if (response.Message.FunctionInvocation != null)
            // {
            //     Console.ForegroundColor = ConsoleColor.Cyan;
            //     Console.WriteLine($"Function Invoked -> {response.Message.FunctionInvocation.Name}");
            // }

            return Ok(response.Message.Text);
        }
    }
}
