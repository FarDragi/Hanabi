using FarDragi.Hanabi.Models;

namespace FarDragi.Hanabi.Services.Interfaces;

public interface ITreatingService
{
    Task<TreatingDto> AddOneTreating(ulong id);
}