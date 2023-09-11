namespace FarDragi.Hanabi.Models;

public record TreatingDto(ulong Id, int Count)
{
    public static implicit operator TreatingEntity(TreatingDto dto)
    {
        return new TreatingEntity(dto.Id, dto.Count);
    }

    public static implicit operator TreatingDto(TreatingEntity entity)
    {
        return new TreatingDto(entity.Id, entity.Count);
    }
}