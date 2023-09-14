using FarDragi.Hanabi.Models;
using FarDragi.Hanabi.Repositories.Interfaces;
using FarDragi.Hanabi.Services.Interfaces;
using Lina.DynamicServicesProvider.Attributes;

namespace FarDragi.Hanabi.Services;

[Service(typeof(IHalloweenService))]
public class HalloweenService : IHalloweenService
{
    private readonly ICandyRepository _candyRepository;
    private readonly ITreatingRepository _treatingRepository;

    public HalloweenService(ICandyRepository candyRepository, ITreatingRepository treatingRepository)
    {
        _candyRepository = candyRepository;
        _treatingRepository = treatingRepository;
    }

    public async Task<(bool, TreatingDto?)> Treating(ulong id, ulong targetId)
    {
        var treating = await _treatingRepository.GetById(id);
        var candy = await _candyRepository.GetById(targetId);

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
}