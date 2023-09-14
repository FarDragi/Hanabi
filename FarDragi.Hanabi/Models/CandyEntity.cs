using FarDragi.Hanabi.Exceptions;
using Lina.Database.Models;

namespace FarDragi.Hanabi.Models;

public class CandyEntity : BaseEntity<ulong>
{
    public CandyEntity(ulong id, int count)
    {
        Id = id;
        Count = count;
    }

    public int Count { get; private set; }

    public bool TreatingCandy(TreatingEntity treating)
    {
        if (!HalloweenEntity.IsHalloween())
            throw new HalloweenException("It is not Halloween");
        
        if (!treating.RemoveOne())
            return false;
        
        Count -= 10;
        return true;
    }

    public void AddCandy(int value)
    {
        if (!HalloweenEntity.IsHalloween())
            throw new HalloweenException("It is not Halloween");

        Count += value;
    }

    #region Conversions

    public static implicit operator CandyEntity(CandyDto dto)
    {
        return new CandyEntity(dto.Id, dto.Count);
    }

    public static implicit operator CandyDto(CandyEntity entity)
    {
        return new CandyDto(entity.Id, entity.Count);
    }

    #endregion
}