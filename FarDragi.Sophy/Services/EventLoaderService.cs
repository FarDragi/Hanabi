﻿using Discord;
using Discord.WebSocket;
using FarDragi.Sophy.Events.Interfaces;
using FarDragi.Sophy.Services.Interfaces;
using Lina.DynamicServicesProvider.Attributes;
using Microsoft.Extensions.Logging;

namespace FarDragi.Sophy.Services;

[Service(typeof(IEventLoaderService))]
public class EventLoaderService : IEventLoaderService 
{
    private readonly IEnumerable<IAutoLoaderEvents> _events;
    private readonly IDiscordClient _discordClient;
    private readonly ILogger<EventLoaderService> _logger;

    public EventLoaderService(IEnumerable<IAutoLoaderEvents> events, IDiscordClient discordClient,
        ILogger<EventLoaderService> logger)
    {
        _events = events;
        _discordClient = discordClient;
        _logger = logger;
    }

    public void Init()
    {
        foreach (var @event in _events)
        {
            if (_discordClient is DiscordSocketClient client)
            {
                @event.AddEvent(client);
            }
            else
            {
                throw new Exception("Events cannot be loaded");
            }
        }
        
        _logger.LogInformation("Loaded {} Events", _events.Count());
    }
}