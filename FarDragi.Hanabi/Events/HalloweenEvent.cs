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
public class HalloweenEvent : IAutoLoaderEvents
{
    private readonly IAppConfig _appConfig;
    private readonly IHalloweenService _halloweenService;
    private readonly IDiscordClient _discordClient;
    
    private readonly string[] _candies = { "🍬", "🍭", "🍪", "🍩", "🧆", "🍫" };

    public HalloweenEvent(IAppConfig appConfig, IHalloweenService halloweenService, IDiscordClient discordClient)
    {
        _appConfig = appConfig;
        _halloweenService = halloweenService;
        _discordClient = discordClient;
    }

    public void AddEvent(DiscordSocketClient discordClient)
    {
        discordClient.Ready += DiscordClientOnReady;
        discordClient.InviteCreated += DiscordClientOnInviteCreated;
        discordClient.InviteDeleted += DiscordClientOnInviteDeleted;
        discordClient.UserJoined += DiscordClientOnUserJoined;
        discordClient.ReactionAdded += DiscordClientOnReactionAdded;
        discordClient.MessageReceived += DiscordClientOnMessageReceived;
    }
    
    private async Task DiscordClientOnReady()
    {
        var guild = await _discordClient.GetGuildAsync(_appConfig.Bot.OwnerGuild);

        if (guild is null)
            return;

        var invites = await guild.GetInvitesAsync();

        foreach (var invite in invites)
        {
            await _halloweenService.UpdateInvite(new InviteDto(invite.Id, invite.Uses ?? 0, invite.Inviter.Id));
        }
    }

    private async Task DiscordClientOnInviteCreated(SocketInvite invite)
    {
        await _halloweenService.UpdateInvite(new InviteDto(invite.Id, 0, invite.Inviter.Id));
    }

    private async Task DiscordClientOnInviteDeleted(SocketGuildChannel channel, string inviteCode)
    {
        await _halloweenService.DeleteInvite(inviteCode);
    }

    private async Task DiscordClientOnReactionAdded(Cacheable<IUserMessage, ulong> userMessage,
        Cacheable<IMessageChannel, ulong> channelMessage, SocketReaction reaction)
    {
        if (await userMessage.GetOrDownloadAsync() is not IMessage message)
            return;
        
        if (message.Reactions.TryGetValue(reaction.Emote, out var candyEmojiCount) && candyEmojiCount.ReactionCount != 2)
            return;
        
        if (!candyEmojiCount.IsMe)
            return;
        
        var emoji = _candies.FirstOrDefault(x => Equals(new Emoji(x), reaction.Emote));
        
        if (emoji is null)
            return;

        await _halloweenService.PickupCandy(reaction.UserId);
        await message.RemoveAllReactionsForEmoteAsync(new Emoji(emoji));
    }

    private async Task DiscordClientOnUserJoined(SocketGuildUser user)
    {
        var guildInvites = await user.Guild.GetInvitesAsync();

        await _halloweenService.AddTreating(guildInvites.Select(x => new InviteDto(x.Id, x.Uses ?? 0, x.Inviter.Id)));
    }

    private async Task DiscordClientOnMessageReceived(SocketMessage message)
    {
        if (!_halloweenService.IsHalloween())
            return;
        
        if (message.Channel.Id != _appConfig.Channels.CandyDrop)
            return;

#if DEBUG            
        await message.AddReactionAsync(new Emoji(_candies[new Random().Next(0, _candies.Length - 1)]));
#else
        var random = new Random().Next(1, 100);

        if (random is >= 0 and < 5)
        {
            await message.AddReactionAsync(new Emoji(_candies[new Random().Next(0, _candies.Length - 1)]));
        }
#endif
    }
}