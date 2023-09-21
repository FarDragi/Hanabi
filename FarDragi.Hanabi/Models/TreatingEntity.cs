using FarDragi.Hanabi.Exceptions;
using FarDragi.Hanabi.Models.Interfaces;
using Lina.Database.Models;

namespace FarDragi.Hanabi.Models;

public class TreatingEntity : BaseEntity<ulong>
{
    public TreatingEntity()
    {
    }

    public TreatingEntity(ulong id)
    {
        Id = id;
    }

    public int Count { get; private set; } = 0;

    public bool RemoveOne()
    {
        if (Count <= 0)
            return false;

        Count -= 1;
        return true;
    }
    
    public void AddTreating(int value, IAppConfig config)
    {
        if (config.Event.Exclude.Contains(Id))
            throw new HalloweenException("User is exclude from event");
        
        if (!HalloweenEntity.IsHalloween())
            throw new HalloweenException("It is not Halloween");

        Count += value;
    }

    #region Conversions

    public static implicit operator TreatingEntity(TreatingDto dto)
    {
        return new TreatingEntity()
        {
            Id = dto.Id,
            Count = dto.Count
        };
    }

    public static implicit operator TreatingDto(TreatingEntity entity)
    {
        return new TreatingDto(entity.Id, entity.Count);
    }

    #endregion
}