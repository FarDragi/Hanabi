using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using FarDragi.Hanabi.Models.Interfaces;
using FarDragi.Hanabi.Services.Interfaces;
using Lina.DynamicServicesProvider;
using Lina.LoaderConfig;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

var serviceCollection = new ServiceCollection();

var config = serviceCollection.AddLoaderConfig<IAppConfig>();
var client = new DiscordSocketClient();
var interaction = new InteractionService(client.Rest);

serviceCollection.AddSingleton<IDiscordClient>(client);
serviceCollection.AddSingleton(interaction);
serviceCollection.AddDynamicServices<Program>();
serviceCollection.AddLogging(builder => builder.AddConsole());
    
var services = serviceCollection.BuildServiceProvider();

services.GetRequiredService<IEventLoaderService>().Init();

await interaction.AddModulesAsync(typeof(Program).Assembly, services);

await client.LoginAsync(TokenType.Bot, config.Bot.Token);
await client.StartAsync();

await Task.Delay(Timeout.Infinite);
