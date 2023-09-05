using Discord;
using Discord.WebSocket;
using FarDragi.Hanabi.Events.Interfaces;
using FarDragi.Hanabi.Models;
using FarDragi.Hanabi.Models.Interfaces;
using FarDragi.Hanabi.Services.Interfaces;
using Lina.DynamicServicesProvider;
using Lina.DynamicServicesProvider.Attributes;

namespace FarDragi.Hanabi.Events;

[Dependency(LifeTime.Transient, typeof(IAutoLoaderEvents))]
public class InviteEvent : IAutoLoaderEvents
{
    private readonly IInviteService _inviteService;
    private readonly IDiscordClient _discordClient;
    private readonly IAppConfig _appConfig;

    public InviteEvent(IInviteService inviteService, IDiscordClient discordClient, IAppConfig appConfig)
    {
        _inviteService = inviteService;
        _discordClient = discordClient;
        _appConfig = appConfig;
    }

    public void AddEvent(DiscordSocketClient discordClient)
    {
        discordClient.Ready += DiscordClientOnReady;
        discordClient.InviteDeleted += DiscordClientOnInviteDeleted;
        discordClient.InviteCreated += DiscordClientOnInviteCreated;
        discordClient.UserJoined += DiscordClientOnUserJoined;
    }
    
    private async Task DiscordClientOnReady()
    {
        var guild = await _discordClient.GetGuildAsync(_appConfig.Bot.OwnerGuild);

        if (guild is null)
            return;

        var invites = await guild.GetInvitesAsync();

        foreach (var invite in invites)
        {
            await _inviteService.AddOrUpdateInvite(new InviteDto(invite.Id, invite.Uses ?? 0, invite.Inviter.Id));
        }
    }

    private async Task DiscordClientOnInviteDeleted(SocketGuildChannel channel, string inviteCode)
    {
        await _inviteService.DeleteInvite(inviteCode);
    }

    private async Task DiscordClientOnInviteCreated(SocketInvite invite)
    {
        await _inviteService.AddOrUpdateInvite(new InviteDto(invite.Id, 0, invite.Inviter.Id));
    }

    private async Task DiscordClientOnUserJoined(SocketGuildUser user)
    {
        var guildInvites = await user.Guild.GetInvitesAsync();

        await _inviteService.UpdateInvites(guildInvites.Select(x => new InviteDto(x.Id, x.Uses ?? 0, x.Inviter.Id)));
    }
}