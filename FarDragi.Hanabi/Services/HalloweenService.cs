using Discord;
using FarDragi.Hanabi.Exceptions;
using FarDragi.Hanabi.Models;
using FarDragi.Hanabi.Models.Interfaces;
using FarDragi.Hanabi.Repositories.Interfaces;
using FarDragi.Hanabi.Services.Interfaces;
using Lina.DynamicServicesProvider.Attributes;

namespace FarDragi.Hanabi.Services;

[Service(typeof(IHalloweenService))]
public class HalloweenService : IHalloweenService
{
    private readonly ICandyRepository _candyRepository;
    private readonly ITreatingRepository _treatingRepository;
    private readonly IInviteRepository _inviteRepository;
    private readonly IAppConfig _appConfig;
    private readonly IDiscordClient _discordClient;

    public HalloweenService(ICandyRepository candyRepository, ITreatingRepository treatingRepository,
        IInviteRepository inviteRepository, IAppConfig appConfig, IDiscordClient discordClient)
    {
        _candyRepository = candyRepository;
        _treatingRepository = treatingRepository;
        _inviteRepository = inviteRepository;
        _appConfig = appConfig;
        _discordClient = discordClient;
    }

    public bool IsHalloween()
    {
        return HalloweenEntity.IsHalloween();
    }
    
    public async Task<InviteDto> UpdateInvite(InviteDto inviteDto)
    {
        var invite = await _inviteRepository.GetById(inviteDto.Id);
        
        if (invite is null)
        {
            invite = inviteDto;
            await _inviteRepository.Add(invite);
        }
        else
        {
            invite.UpdateUses(inviteDto.Uses);
            _inviteRepository.Update(invite);
        }
        
        await _inviteRepository.Commit();

        return invite;
    }

    public async Task DeleteInvite(string inviteId)
    {
        _inviteRepository.DetachAllEntities();
        var invite = await _inviteRepository.GetById(inviteId);
        
        if (invite is null)
            return;
        
        _inviteRepository.Delete(invite);
        await _inviteRepository.Commit();
        _inviteRepository.DetachAllEntities();
    }

    public async Task<InviteDto> AddTreating(IEnumerable<InviteDto> invitesDto)
    {
        var invites = await _inviteRepository.GetAll(x => true);
        
        var invite = invites.FirstOrDefault(x => invitesDto.Any(y => x.Id == y.Id && x.Uses < y.Uses));

        if (invite is null)
            throw new NotFoundException($"Treating not found");
        
        var candy = await _candyRepository.GetById(invite.UserId);
        var treating = await _treatingRepository.GetById(invite.UserId);

        if (candy is null)
        {
            candy = new CandyEntity(invite.UserId);
            await _candyRepository.Add(candy);
            await _candyRepository.Commit();
        }

        if (treating is null)
        {
            treating = new TreatingEntity(invite.UserId);
            await _treatingRepository.Add(treating);
            await _treatingRepository.Commit();
        }

        try
        {
            var user = await _discordClient.GetUserAsync(invite.UserId);
            
            invite.AddOneUse(treating, candy, _appConfig, user.IsBot);

            _candyRepository.Update(candy);
            await _candyRepository.Commit();

            _treatingRepository.Update(treating);
            await _treatingRepository.Commit();
        }
        finally
        {
            _inviteRepository.Update(invite);
            await _inviteRepository.Commit();
        }
        
        return invite;
    }

    public async Task<CandyDto> PickupCandy(ulong userId)
    {
        var candy = await _candyRepository.GetById(userId);

        if (candy is null)
        {
            candy = new CandyEntity(userId);
            candy.AddCandy(1, _appConfig);
            await _candyRepository.Add(candy);
        }
        else
        {
            candy.AddCandy(1, _appConfig);
            _candyRepository.Update(candy);
        }

        await _candyRepository.Commit();
        _candyRepository.DetachAllEntities();

        return candy;
    }

    public async Task<(bool, TreatingDto?)> Treating(ulong userId, ulong targetUserId)
    {
        var treating = await _treatingRepository.GetById(userId);
        var candy = await _candyRepository.GetById(targetUserId);

        if (treating is null || candy is null)
            return (false, null);

        if (!candy.TreatingCandy(treating))
            return (false, treating);
        
        _treatingRepository.Update(treating);
        await _treatingRepository.Commit();
        
        _candyRepository.Update(candy);
        await _candyRepository.Commit();

        return (true, treating);
    }

    public async Task<(CandyDto?, TreatingDto?)> UserInfo(ulong userId)
    {
        var treating = await _treatingRepository.GetById(userId);
        var candy = await _candyRepository.GetById(userId);

        if (candy is not null && treating is not null)
            return (candy, treating);
        
        if (candy is null && treating is not null)
            return (null, treating);

        if (treating is null && candy is not null)
            return (candy, null);

        return (null, null);
    }

    public async Task<CandyDto> AddManualCandies(ulong userId, int amount)
    {
        
        var candy = await _candyRepository.GetById(userId);

        if (candy is null)
        {
            candy = new CandyEntity(userId);
            candy.AddCandy(amount, _appConfig);
            await _candyRepository.Add(candy);
        }
        else
        {
            candy.AddCandy(amount, _appConfig);
            _candyRepository.Update(candy);
        }

        await _candyRepository.Commit();

        return candy;
    }
    
    public async Task<TreatingDto> AddManualTreating(ulong userId, int amount)
    {
        var treating = await _treatingRepository.GetById(userId);

        if (treating is null)
        {
            treating = new TreatingEntity(userId);
            treating.AddTreating(amount, _appConfig);
            await _treatingRepository.Add(treating);
        }
        else
        {
            treating.AddTreating(amount, _appConfig);
            _treatingRepository.Update(treating);
        }

        await _treatingRepository.Commit();

        return treating;
    }

    public async Task<IEnumerable<CandyDto>> GetLeaderboard(int page)
    {
        var candies = await _candyRepository.GetLeaderBoard(page, 10);

        return candies.Select(x => (CandyDto)x);
    }

    public async Task RemoveUser(ulong id)
    {
        var candy = await _candyRepository.GetById(id);

        if (candy is not null)
        {
            _candyRepository.Delete(candy);
            await _candyRepository.Commit();
        }

        var treating = await _treatingRepository.GetById(id);

        if (treating is not null)
        {
            _treatingRepository.Delete(treating);
            await _treatingRepository.Commit();
        }
    }
}