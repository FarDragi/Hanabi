using Discord;
using Discord.WebSocket;
using FarDragi.Hanabi.Events.Interfaces;
using FarDragi.Hanabi.Models.Interfaces;
using Lina.DynamicServicesProvider;
using Lina.DynamicServicesProvider.Attributes;

namespace FarDragi.Hanabi.Events;

[Dependency(LifeTime.Transient, typeof(IAutoLoaderEvents))]
public class StarReactEvent : IAutoLoaderEvents
{
    private readonly IAppConfig _appConfig;
    private readonly IDiscordClient _discordClient;

    public StarReactEvent(IAppConfig appConfig, IDiscordClient discordClient)
    {
        _appConfig = appConfig;
        _discordClient = discordClient;
    }

    public void AddEvent(DiscordSocketClient discordClient)
    {
        discordClient.ReactionAdded += DiscordClientOnReactionAdded;
    }

    private async Task DiscordClientOnReactionAdded(Cacheable<IUserMessage, ulong> userMessage,
        Cacheable<IMessageChannel, ulong> channelMessage, SocketReaction reaction)
    {
        var starEmoji = new Emoji("⭐");
        var starGlowEmoji = new Emoji("🌟");

        if (!Equals(reaction.Emote, starEmoji))
            return;
        
        if (await userMessage.GetOrDownloadAsync() is not IMessage message)
            return;
        
        if (reaction.User.Value is not IGuildUser user)
            return;

        if (message.Reactions.TryGetValue(starEmoji, out var starEmojiCount) && starEmojiCount.ReactionCount < 3)
            return;
        
        if (message.Reactions.TryGetValue(starGlowEmoji, out var starGlowEmojiCount) && starEmojiCount.ReactionCount > 0)
            return;
        
        var channel = await _discordClient.GetChannelAsync(_appConfig.Channels.StarBoard);

        if (channel is not IMessageChannel starBoard)
            return;

        var embed = new EmbedBuilder()
            .WithDescription(message.Content)
            .WithAuthor(new EmbedAuthorBuilder()
                .WithName(user.DisplayName)
                .WithIconUrl(user.GetDisplayAvatarUrl())
            )
            .WithColor(new Color(0xedcd2d))
            .WithTimestamp(DateTimeOffset.Now)
            .Build();

        await starBoard.SendMessageAsync(embed: embed);
        await message.AddReactionAsync(starGlowEmoji);
    }
}