using FarDragi.Hanabi.Models;

namespace FarDragi.Hanabi.Services.Interfaces;

public interface IInviteService
{
    Task<InviteDto> AddOrUpdateInvite(InviteDto dto);
    Task DeleteInvite(string id);
    Task<InviteDto> UpdateInvites(IEnumerable<InviteDto> invites);
}