using FarDragi.Hanabi.Exceptions;
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

    public async Task<InviteDto> AddOrUpdateInvite(InviteDto dto)
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

    public async Task DeleteInvite(string id)
    {
        var entity = await _inviteRepository.GetById(id);
        
        if (entity is null)
            return;
        
        _inviteRepository.Delete(entity);
        await _inviteRepository.Commit();
    }

    public async Task<InviteDto> CheckInvites(IEnumerable<InviteDto> invites)
    {
        var entity = await _inviteRepository.Get(x => invites.Any(y => y.Id == x.Id && y.Uses > x.Uses));

        if (entity is null)
            throw new NotFoundException($"Invite not found");
        
        return entity;
    }

    public async Task<InviteDto> AddOneUse(string id)
    {
        var entity = await _inviteRepository.GetById(id);

        if (entity is null)
            throw new NotFoundException($"Invite with id {id} not found");

        entity.Uses++;
        
        _inviteRepository.Update(entity);
        await _inviteRepository.Commit();

        return entity;
    }
}