using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using FarDragi.Hanabi.Events.Interfaces;
using FarDragi.Hanabi.Models.Interfaces;
using FarDragi.Hanabi.Services.Interfaces;
using Lina.DynamicServicesProvider;
using Lina.DynamicServicesProvider.Attributes;
using Microsoft.Extensions.Logging;

namespace FarDragi.Hanabi.Events;

[Dependency(LifeTime.Transient, typeof(IAutoLoaderEvents))]
public class ReadyEvent : IAutoLoaderEvents
{
    private readonly IDiscordClient _discordClient;
    private readonly IDatabaseMigrateService _databaseMigrateService;
    private readonly InteractionService _interactionService;
    private readonly IAppConfig _appConfig;
    private readonly ILogger<ReadyEvent> _logger;
    
    public ReadyEvent(IDiscordClient discordClient, IDatabaseMigrateService databaseMigrateService,
        InteractionService interactionService, IAppConfig appConfig, ILogger<ReadyEvent> logger)
    {
        _discordClient = discordClient;
        _databaseMigrateService = databaseMigrateService;
        _interactionService = interactionService;
        _appConfig = appConfig;
        _logger = logger;
    }

    public void AddEvent(DiscordSocketClient discordClient)
    {
        discordClient.Ready += DiscordClientOnReady;
    }

    private async Task DiscordClientOnReady()
    {
        await _databaseMigrateService.Migrate();

        var commands = await _interactionService.RegisterCommandsToGuildAsync(_appConfig.Bot.OwnerGuild);
        _logger.LogInformation("Loaded {} Guild ({}) Commands", commands.Count, _appConfig.Bot.OwnerGuild);
    }
}