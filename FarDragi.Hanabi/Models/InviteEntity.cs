using FarDragi.Hanabi.Exceptions;
using Lina.Database.Models;

namespace FarDragi.Hanabi.Models;

public class InviteEntity : BaseEntity<string>
{
    public InviteEntity(string id, int uses, ulong userId)
    {
        Id = id;
        Uses = uses;
        UserId = userId;
    }

    public int Uses { get; private set; }
    public ulong UserId { get; set; }

    public void UpdateUses(int uses)
    {
        if (Uses < 0)
            throw new ArgumentOutOfRangeException(nameof(uses), uses, "Cannot be less than 0");

        Uses = uses;
    }

    public void AddOneUse(TreatingEntity treating, CandyEntity candy)
    {
        if (!HalloweenEntity.IsHalloween())
            throw new HalloweenException("It is not Halloween");
        
        Uses++;

        treating.AddOne();
        candy.AddCandy(15);
    }

    #region Conversions

    public static implicit operator InviteEntity(InviteDto dto)
    {
        return new InviteEntity(dto.Id, dto.Uses, dto.UserId);
    }

    public static implicit operator InviteDto(InviteEntity entity)
    {
        return new InviteDto(entity.Id, entity.Uses, entity.UserId);
    }

    #endregion
}