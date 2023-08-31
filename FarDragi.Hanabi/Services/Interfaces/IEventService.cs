using FarDragi.Hanabi.Models;

namespace FarDragi.Hanabi.Services.Interfaces;

public interface IEventService
{
    Task<EventDto> Add(EventDto newEvent);
}