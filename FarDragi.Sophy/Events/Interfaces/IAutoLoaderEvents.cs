using Discord.WebSocket;

namespace FarDragi.Sophy.Events.Interfaces;

public interface IAutoLoaderEvents
{
    void AddEvent(DiscordSocketClient discordClient);
}