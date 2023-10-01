using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using FarDragi.Hanabi.Models.Interfaces;
using FarDragi.Hanabi.Services.Interfaces;
using Lina.Database;
using Lina.DynamicServicesProvider;
using Lina.LoaderConfig;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

var client = new DiscordSocketClient(new DiscordSocketConfig()
{
    GatewayIntents = GatewayIntents.All
});
var interaction = new InteractionService(client.Rest);

var serviceCollection = new ServiceCollection();

var config = serviceCollection.AddLoaderConfig<IAppConfig>();
serviceCollection.AddSingleton<IDiscordClient>(client);
serviceCollection.AddSingleton(interaction);
serviceCollection.AddDynamicServices<Program>();
serviceCollection.AddLogging(builder => builder.AddConsole());
serviceCollection.AddLinaDbContext<Program>((builder, assembly) => builder.UseMySql(config.Database.Url,
    ServerVersion.AutoDetect(config.Database.Url), options => options.MigrationsAssembly(assembly)));
    
var services = serviceCollection.BuildServiceProvider();

services.GetRequiredService<IEventLoaderService>().LoadEvents();
await services.GetRequiredService<IDatabaseMigrateService>().Migrate();

await interaction.AddModulesAsync(typeof(Program).Assembly, services);

await client.LoginAsync(TokenType.Bot, config.Bot.Token);
await client.StartAsync();

await Task.Delay(Timeout.Infinite);
