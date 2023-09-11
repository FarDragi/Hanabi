using Discord;
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
    private readonly ICandyService _candyService;
    private readonly ITreatingService _treatingService;
    private readonly IDiscordClient _discordClient;

    public InviteService(IInviteRepository inviteRepository, ICandyService candyService,
        ITreatingService treatingService, IDiscordClient discordClient)
    {
        _inviteRepository = inviteRepository;
        _candyService = candyService;
        _treatingService = treatingService;
        _discordClient = discordClient;
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

    private async Task<InviteDto> CheckInvites(IEnumerable<InviteDto> invites)
    {
        var entities = await _inviteRepository.GetAll(x => true);

        var entity = entities.FirstOrDefault(x => invites.Any(y => x.Id == y.Id && x.Uses < y.Uses));

        if (entity is null)
            throw new NotFoundException($"Invite not found");
        
        return entity;
    }

    private async Task<InviteDto> AddOneUse(string id)
    {
        var entity = await _inviteRepository.GetById(id);

        if (entity is null)
            throw new NotFoundException($"Invite with id {id} not found");

        entity.Uses++;
        
        _inviteRepository.Update(entity);
        await _inviteRepository.Commit();

        return entity;
    }

    public async Task<InviteDto> UpdateInvites(IEnumerable<InviteDto> invites)
    {
        var changeInvite = await CheckInvites(invites);

        await AddOneUse(changeInvite.Id);

        var user = await _discordClient.GetUserAsync(changeInvite.UserId);

        if (user.IsBot || !_candyService.IsCandyEvent())
            return changeInvite;
        
        await _candyService.AddCandies(new CandyDto(changeInvite.UserId, 15));
        await _treatingService.AddOneTreating(changeInvite.UserId);

        return changeInvite;
    }
}