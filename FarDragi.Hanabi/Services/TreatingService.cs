using FarDragi.Hanabi.Models;
using FarDragi.Hanabi.Repositories.Interfaces;
using FarDragi.Hanabi.Services.Interfaces;
using Lina.DynamicServicesProvider.Attributes;

namespace FarDragi.Hanabi.Services;

[Service(typeof(ITreatingService))]
public class TreatingService : ITreatingService
{
    private readonly ITreatingRepository _treatingRepository;

    public TreatingService(ITreatingRepository treatingRepository)
    {
        _treatingRepository = treatingRepository;
    }

    public async Task<TreatingDto> AddOneTreating(ulong id)
    {
        var entity = await _treatingRepository.GetById(id);

        if (entity is null)
        {
            entity = new TreatingEntity(id, 1);
            await _treatingRepository.Add(entity);
        }
        else
        {
            //entity.Count++;
            _treatingRepository.Update(entity);
        }
        
        await _treatingRepository.Commit();

        return entity;
    }
}