using Discord.WebSocket;
using FarDragi.Hanabi.Events.Interfaces;
using Lina.DynamicServicesProvider;
using Lina.DynamicServicesProvider.Attributes;

namespace FarDragi.Hanabi.Events;

[Dependency(LifeTime.Transient, typeof(IAutoLoaderEvents))]
public class JoinEvent : IAutoLoaderEvents
{
    public void AddEvent(DiscordSocketClient discordClient)
    {
        discordClient.UserJoined += DiscordClientOnUserJoined;
    }

    private async Task DiscordClientOnUserJoined(SocketGuildUser user)
    {
    }
}