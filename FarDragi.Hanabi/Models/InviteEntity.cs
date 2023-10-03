using FarDragi.Hanabi.Exceptions;
using FarDragi.Hanabi.Models.Interfaces;
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

    public void AddOneUse(TreatingEntity treating, CandyEntity candy, IAppConfig config, bool isBot)
    {
        Uses++;
        
        if (isBot)
            return;

        treating.AddTreating(1, config);
        candy.AddCandy(15, config);
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