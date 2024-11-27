using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using DotNetEnv;
using Azure;
using Azure.AI.Inference;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Logging;
using SockChatBotApi.Models;
using System;
using SockChatBotApi.Controllers;

Env.Load(".env");
string githubKey = Env.GetString("GITHUB_KEY");

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add the chat client
IChatClient innerChatClient = new ChatCompletionsClient(
    endpoint: new Uri("https://models.inference.ai.azure.com"),
    new AzureKeyCredential(githubKey))
    .AsChatClient("gpt-4o-mini");

builder.Services.AddChatClient(chatClientBuilder => chatClientBuilder
    .UseFunctionInvocation()
    .UseLogging()
    .Use(innerChatClient));

var cart = new Cart();
var getPriceTool = AIFunctionFactory.Create(cart.GetPrice);
var addToCartTool = AIFunctionFactory.Create(cart.AddSocksToCart);

var light = new LightController();
var getAllLightsTool = AIFunctionFactory.Create(light.GetLights);
var getLightTool = AIFunctionFactory.Create(light.GetLight);
var createLightTool = AIFunctionFactory.Create(light.AddLight);
var updateLightTool = AIFunctionFactory.Create(light.UpdateLight);
var deleteLightTool = AIFunctionFactory.Create(light.DeleteLight);

var chatOptions = new ChatOptions
{
    Tools = new[]
    {
        getPriceTool, 
        addToCartTool, 
        getAllLightsTool, 
        getLightTool, 
        createLightTool, 
        updateLightTool, 
        deleteLightTool
        }
};

builder.Services.AddSingleton(cart);
builder.Services.AddSingleton(chatOptions);

builder.Services.AddLogging(loggingBuilder =>
    loggingBuilder.AddConsole().SetMinimumLevel(LogLevel.Trace));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();
app.MapControllers();
app.Run();
