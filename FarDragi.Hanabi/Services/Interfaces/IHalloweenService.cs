using FarDragi.Hanabi.Models;

namespace FarDragi.Hanabi.Services.Interfaces;

public interface IHalloweenService
{
    Task<(bool, TreatingDto?)> Treating(ulong userId, ulong targetUserId);
    Task<(CandyDto?, TreatingDto?)> UserInfo(ulong userId);
    Task<CandyDto> PickupCandy(ulong userId);
    bool IsHalloween();
    Task<InviteDto> UpdateInvite(InviteDto inviteDto);
    Task DeleteInvite(string inviteId);
    Task<InviteDto> AddTreating(IEnumerable<InviteDto> invitesDto);
    Task<CandyDto> AddManualCandies(ulong userId, int amount);
    Task<TreatingDto> AddManualTreating(ulong userId, int amount);
}