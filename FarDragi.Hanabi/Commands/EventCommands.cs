using Discord.Interactions;
using FarDragi.Hanabi.Models;
using FarDragi.Hanabi.Services.Interfaces;

namespace FarDragi.Hanabi.Commands;

[Group("event", "Comandos relacionado a eventos")]
public class EventCommands : InteractionModuleBase
{
    private readonly IEventService _eventService;

    public EventCommands(IEventService eventService)
    {
        _eventService = eventService;
    }

    [SlashCommand("add", "Adicionar novo evento")]
    public async Task Add(string name, DateTime start, DateTime end)
    {
        await _eventService.Add(new EventDto(0, name, start, end));
    }
}