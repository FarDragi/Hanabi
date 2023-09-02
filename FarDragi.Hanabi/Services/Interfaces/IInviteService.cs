using FarDragi.Hanabi.Models;

namespace FarDragi.Hanabi.Services.Interfaces;

public interface IInviteService
{
    Task<InviteDto> AddOrUpdate(InviteDto dto);
}