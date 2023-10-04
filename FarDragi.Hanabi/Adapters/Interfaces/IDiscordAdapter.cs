using FarDragi.Hanabi.Models;

namespace FarDragi.Hanabi.Adapters.Interfaces;

public interface IDiscordAdapter
{
    Task<UserJoinDto?> GetUserInvite(ulong userId);
}