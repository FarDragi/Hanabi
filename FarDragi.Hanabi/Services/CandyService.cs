using FarDragi.Hanabi.Models;
using FarDragi.Hanabi.Repositories.Interfaces;
using FarDragi.Hanabi.Services.Interfaces;
using Lina.DynamicServicesProvider.Attributes;

namespace FarDragi.Hanabi.Services;

[Service(typeof(ICandyService))]
public class CandyService : ICandyService
{
    private readonly ICandyRepository _candyRepository;

    public CandyService(ICandyRepository candyRepository)
    {
        _candyRepository = candyRepository;
    }

    public bool IsCandyEvent()
    {
#if DEBUG
        return true;
#else
        return DateTimeOffset.Now.Month != 10;
#endif
    }

    public async Task<CandyDto> AddCandies(CandyDto dto)
    {
        var entity = await _candyRepository.GetById(dto.Id);

        if (entity is null)
        {
            entity = dto;
            await _candyRepository.Add(entity);
        }
        else
        {
            entity.Count += dto.Count;
            _candyRepository.Update(entity);
        }

        await _candyRepository.Commit();

        return entity;
    }
}