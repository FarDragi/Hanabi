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
            throw new HalloweenException("Não é mais halloween");

        if (!treating.RemoveOne())
            return false;

        Count -= 10;
        return true;
    }

    public void AddCandy(int value, IAppConfig config)
    {
        if (!HalloweenEntity.IsHalloween())
            throw new HalloweenException("Não é mais halloween");
        
        if (config.Event.Exclude.Contains(Id))
            throw new HalloweenException("User is exclude from event");

        Count += value;
    }

    public void Transfer(CandyEntity target, int quantity, IAppConfig config)
    {
        if (!HalloweenEntity.IsHalloween())
            throw new HalloweenException("Não é mais halloween");

        if (quantity < 1)
            throw new HalloweenException("Não é possivel transferir menos que 1 doce");
        
        if (Count < quantity)
            throw new HalloweenException("Quantidade de doces insuficiente");
        
        if (config.Event.Exclude.Contains(Id))
            throw new HalloweenException("Usuario excluido do evento");

        Count -= quantity;
        target.Count += quantity;
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