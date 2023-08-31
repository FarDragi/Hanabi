namespace FarDragi.Hanabi.Models;

public record EventDto(uint Id, string Name, DateTimeOffset Start, DateTimeOffset End)
{
    public static explicit operator EventEntity(EventDto dto)
    {
        return new EventEntity(dto.Name, dto.Start, dto.End)
        {
            Id = dto.Id
        };
    }

    public static implicit operator EventDto(EventEntity entity)
    {
        return new EventDto(entity.Id, entity.Name, entity.Start, entity.End);
    }
}