using FarDragi.Hanabi.Models;

namespace FarDragi.Hanabi.Services.Interfaces;

public interface IHalloweenService
{
    Task<(bool, TreatingDto?)> Treating(ulong id, ulong targetId);
    Task<(CandyDto?, TreatingDto?)> UserInfo(ulong id);
}