using Discord;

namespace FarDragi.Hanabi.Models;

public record InviteDto(string Id, int Uses, ulong UserId)
{
    public static explicit operator InviteEntity(InviteDto dto)
    {
        return new InviteEntity(dto.Id, dto.Uses, dto.UserId);
    }

    public static implicit operator InviteDto(InviteEntity entity)
    {
        return new InviteDto(entity.Id, entity.Uses, entity.UserId);
    }
}
