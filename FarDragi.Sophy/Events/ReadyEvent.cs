using Discord;
using Discord.WebSocket;
using FarDragi.Sophy.Events.Interfaces;
using Lina.DynamicServicesProvider;
using Lina.DynamicServicesProvider.Attributes;
using Microsoft.Extensions.DependencyInjection;

namespace FarDragi.Sophy.Events;

[Dependency(LifeTime.Transient, typeof(IAutoLoaderEvents))]
public class ReadyEvent : IAutoLoaderEvents
{
    private readonly IDiscordClient _discordClient;
    
    public ReadyEvent(IDiscordClient discordClient)
    {
        _discordClient = discordClient;
    }

    public void AddEvent(DiscordSocketClient discordClient)
    {
        discordClient.Ready += DiscordClientOnReady;
    }

    private Task DiscordClientOnReady()
    {
        return Task.CompletedTask;
    }
}