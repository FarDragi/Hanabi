using FarDragi.Hanabi.Exceptions;
using FarDragi.Hanabi.Models.Interfaces;
using Lina.Database.Models;

namespace FarDragi.Hanabi.Models;

public class CandyEntity : BaseEntity<ulong>
{
    public CandyEntity()
    {
    }

    public CandyEntity(ulong id)
    {
        Id = id;
    }

    public int Count { get; private set; } = 0;

    public bool TreatingCandy(TreatingEntity treating)
    {
        if (!HalloweenEntity.IsHalloween())
            throw new HalloweenException("It is not Halloween");

        if (!treating.RemoveOne())
            return false;

        Count -= 10;
        return true;
    }

    public void AddCandy(int value, IAppConfig config)
    {
        if (config.Event.Exclude.Contains(Id))
            throw new HalloweenException("User is exclude from event");
        
        if (!HalloweenEntity.IsHalloween())
            throw new HalloweenException("It is not Halloween");

        Count += value;
    }

    #region Conversions

    public static implicit operator CandyEntity(CandyDto dto)
    {
        return new CandyEntity()
        {
            Id = dto.Id,
            Count = dto.Count
        };
    }

    public static implicit operator CandyDto(CandyEntity entity)
    {
        return new CandyDto(entity.Id, entity.Count);
    }

    #endregion
}