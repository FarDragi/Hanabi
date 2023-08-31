using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using FarDragi.Hanabi.Events.Interfaces;
using Lina.DynamicServicesProvider;
using Lina.DynamicServicesProvider.Attributes;

namespace FarDragi.Hanabi.Events;

[Dependency(LifeTime.Transient, typeof(IAutoLoaderEvents))]
public class InteractionCreatedEvent : IAutoLoaderEvents
{
    private readonly IDiscordClient _discordClient;
    private readonly InteractionService _interactionService;
    private readonly IServiceProvider _serviceProvider;

    public InteractionCreatedEvent(IDiscordClient discordClient, InteractionService interactionService,
        IServiceProvider serviceProvider)
    {
        _discordClient = discordClient;
        _interactionService = interactionService;
        _serviceProvider = serviceProvider;
    }

    public void AddEvent(DiscordSocketClient discordClient)
    {
        discordClient.InteractionCreated += DiscordClientOnInteractionCreated;
    }

    private async Task DiscordClientOnInteractionCreated(SocketInteraction arg)
    {
        if (_discordClient is not DiscordSocketClient socketClient)
            return;

        var context = new SocketInteractionContext(socketClient, arg);
        await _interactionService.ExecuteCommandAsync(context, _serviceProvider);
    }
}