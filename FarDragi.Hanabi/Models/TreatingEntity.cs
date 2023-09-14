using FarDragi.Hanabi.Exceptions;
using Lina.Database.Models;

namespace FarDragi.Hanabi.Models;

public class TreatingEntity : BaseEntity<ulong>
{
    public TreatingEntity(ulong id, int count)
    {
        Id = id;
        Count = count;
    }

    public int Count { get; private set; }

    public bool RemoveOne()
    {
        if (Count <= 0)
            return false;
        
        Count -= 1;
        return true;
    }

    public void AddOne()
    {
        if (!HalloweenEntity.IsHalloween())
            throw new HalloweenException("It is not Halloween");

        Count += 1;
    }

    #region Conversions

    public static implicit operator TreatingEntity(TreatingDto dto)
    {
        return new TreatingEntity(dto.Id, dto.Count);
    }

    public static implicit operator TreatingDto(TreatingEntity entity)
    {
        return new TreatingDto(entity.Id, entity.Count);
    }

    #endregion
}