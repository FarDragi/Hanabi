using FarDragi.Hanabi.Models;
using FarDragi.Hanabi.Repositories.Interfaces;
using FarDragi.Hanabi.Services.Interfaces;
using Lina.DynamicServicesProvider.Attributes;

namespace FarDragi.Hanabi.Services;

[Service(typeof(IInviteService))]
public class InviteService : IInviteService
{
    private readonly IInviteRepository _inviteRepository;

    public InviteService(IInviteRepository inviteRepository)
    {
        _inviteRepository = inviteRepository;
    }

    public async Task<InviteDto> AddOrUpdate(InviteDto dto)
    {
        var entity = await _inviteRepository.GetById(dto.Id);

        if (entity is null)
        {
            entity = (InviteEntity)dto;
            await _inviteRepository.Add(entity);
        }
        else
        {
            entity.Uses = dto.Uses;
            _inviteRepository.Update(entity);
        }

        await _inviteRepository.Commit();

        return entity;
    }
}