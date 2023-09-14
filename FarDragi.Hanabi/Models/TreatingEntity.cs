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

    public bool HasTreating()
    {
        return Count > 0;
    }

    public bool RemoveOne()
    {
        if (!HasTreating())
            return false;
        
        Count -= 1;
        return true;
    }

    #region Conversions

    public static implicit operator TreatingEntity(TreatingDto dto)
    {
        return new TreatingEntity(dto.Id, dto.Count);
    }

    public static implicit operator TreatingDto?(TreatingEntity? entity)
    {
        return entity is null ? null : new TreatingDto(entity.Id, entity.Count);
    }

    #endregion
}