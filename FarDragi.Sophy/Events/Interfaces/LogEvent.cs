using Discord;
using Discord.WebSocket;
using FarDragi.Sophy.Services.Interfaces;
using Lina.DynamicServicesProvider;
using Lina.DynamicServicesProvider.Attributes;

namespace FarDragi.Sophy.Events.Interfaces;

[Dependency(LifeTime.Transient, typeof(IAutoLoaderEvents))]
public class LogEvent : IAutoLoaderEvents
{private readonly ILogService _logService;

    public LogEvent(ILogService logService)
    {
        _logService = logService;
    }

    public void AddEvent(DiscordSocketClient discordClient)
    {
        discordClient.Log += DiscordClientOnLog;
    }

    private Task DiscordClientOnLog(LogMessage arg)
    {
        _logService.Log(arg);
        return Task.CompletedTask;
    }
}