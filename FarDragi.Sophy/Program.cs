using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using FarDragi.Sophy.Models.Interfaces;
using Lina.DynamicServicesProvider;
using Lina.LoaderConfig;
using Microsoft.Extensions.DependencyInjection;

var serviceCollection = new ServiceCollection();

var config = serviceCollection.AddLoaderConfig<IAppConfig>();
var client = new DiscordSocketClient();
var interaction = new InteractionService(client.Rest);

serviceCollection.AddSingleton<IDiscordClient>(client);
serviceCollection.AddSingleton(interaction);
serviceCollection.AddDynamicServices<Program>();
    
var services = serviceCollection.BuildServiceProvider();

await interaction.AddModulesAsync(typeof(Program).Assembly, services);

await client.LoginAsync(TokenType.Bot, config.Bot.Token);
await client.StartAsync();

await Task.Delay(Timeout.Infinite);
