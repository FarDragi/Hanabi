using Discord;
using Discord.WebSocket;
using FarDragi.Hanabi.Events.Interfaces;
using FarDragi.Hanabi.Models;
using FarDragi.Hanabi.Services.Interfaces;
using Lina.DynamicServicesProvider;
using Lina.DynamicServicesProvider.Attributes;

namespace FarDragi.Hanabi.Events;

[Dependency(LifeTime.Transient, typeof(IAutoLoaderEvents))]
public class CandyEvent : IAutoLoaderEvents
{
    private readonly ICandyService _candyService;
    private readonly string[] _candies;

    public CandyEvent(ICandyService candyService)
    {
        _candyService = candyService;
        _candies = new[] { "🍬", "🍭", "🍪", "🍩", "🧆", "🍫" };
    }

    public void AddEvent(DiscordSocketClient discordClient)
    {
        discordClient.ReactionAdded += DiscordClientOnReactionAdded;
        discordClient.MessageReceived += DiscordClientOnMessageReceived;
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
        
        if (!_candyService.IsCandyEvent())
            return;

        await _candyService.AddCandies(new CandyDto(reaction.UserId, 1));
        await message.RemoveAllReactionsForEmoteAsync(new Emoji(emoji));
    }

    private async Task DiscordClientOnMessageReceived(SocketMessage message)
    {
        if (!_candyService.IsCandyEvent())
            return;
            
        var random = new Random().Next(1, 100);

        if (random is >= 0 and < 5)
        {
            await message.AddReactionAsync(new Emoji(_candies[new Random().Next(0, _candies.Length - 1)]));
        }
    }
}