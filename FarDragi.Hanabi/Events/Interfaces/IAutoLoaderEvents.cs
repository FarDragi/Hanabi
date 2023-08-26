using Discord.WebSocket;

namespace FarDragi.Hanabi.Events.Interfaces;

public interface IAutoLoaderEvents
{
    void AddEvent(DiscordSocketClient discordClient);
}