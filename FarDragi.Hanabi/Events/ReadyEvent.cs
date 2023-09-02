using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using FarDragi.Hanabi.Events.Interfaces;
using FarDragi.Hanabi.Models;
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
    private readonly IInviteService _inviteService;
    
    public ReadyEvent(IDiscordClient discordClient, IDatabaseMigrateService databaseMigrateService,
        InteractionService interactionService, IAppConfig appConfig, ILogger<ReadyEvent> logger,
        IInviteService inviteService)
    {
        _discordClient = discordClient;
        _databaseMigrateService = databaseMigrateService;
        _interactionService = interactionService;
        _appConfig = appConfig;
        _logger = logger;
        _inviteService = inviteService;
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

        var guild = await _discordClient.GetGuildAsync(_appConfig.Bot.OwnerGuild);

        if (guild is null)
            return;

        var invites = await guild.GetInvitesAsync();

        foreach (var invite in invites)
        {
            await _inviteService.AddOrUpdate(new InviteDto(invite.Id, invite.Uses ?? 0, invite.Inviter.Id));
        }
    }
}