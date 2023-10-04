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
    Task<InviteDto> UserJoin(IEnumerable<InviteDto> invitesDto, ulong userId);
    Task<CandyDto> AddManualCandies(ulong userId, int amount);
    Task<TreatingDto> AddManualTreating(ulong userId, int amount);
    Task<IEnumerable<CandyDto>> GetLeaderboard(int page);
    Task RemoveUser(ulong id);
    Task<CandyDto> Transfer(ulong current, ulong target, int quantity);
}